using System;
using System.Collections.Generic;
using Prover.SpecWriter.Impl;
using Prover.SpecWriter.Serializers;

namespace Prover.SpecWriter.SpecLevel {
  /// <summary>
  /// You can use this implementation to pass data to the spec writers,
  /// or you could implement your own parameter carrying object. I don't care which.
  /// </summary>
  public class SpecContextImpl
    : SpecContext {

    readonly Func<string, NameEnumerator> nameEnumeratorFac;

    readonly Cache<string, NameEnumerator> existingNameGivers 
      = new DictionaryCache<string, NameEnumerator>();

    public SpecContextImpl(Type targetType, IEnumerable<CalledMethodInfo> calledMethodInfos, Func<string, NameEnumerator> nameEnumeratorFac) {
      this.nameEnumeratorFac = nameEnumeratorFac;
      Namespace = "Empty.Namespace";
      BoostrapClassName = "Bootstrap";
      Title = "No_title";
      TargetType = targetType;
      CalledMethods = calledMethodInfos;
      SerializerType = typeof (DataContractSerializer);
    }

    public SpecContextImpl(
      string subjectName,
      string boostrapClassName, 
      string ns,
      string title,
      IEnumerable<CalledMethodInfo> calledMethods,
      Type targetType,
      Func<string, NameEnumerator> nameEnumeratorFac,
      Type serializerType)
    {
      SubjectName = subjectName;
      BoostrapClassName = boostrapClassName;
      Namespace = ns;
      Title = title;
      CalledMethods = calledMethods;
      TargetType = targetType;
      this.nameEnumeratorFac = nameEnumeratorFac;
      SerializerType = serializerType;
    }

    public string BoostrapClassName { get; private set; }

    public string SubjectName { get; private set; }
    public string Namespace { get; private set; }
    public string Title { get; private set; }
    public Type TargetType { get; private set; }
    public Type SerializerType { get; private set; }
    public IEnumerable<CalledMethodInfo> CalledMethods { get; private set; }

    NameEnumerator SpecContext.NameEnumeratorFor(string baseName) {
      return existingNameGivers.Get(baseName, () => nameEnumeratorFac(baseName));
    }
  }
}