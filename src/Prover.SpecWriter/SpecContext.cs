using System;
using System.Collections.Generic;

namespace Prover.SpecWriter {
  // Properties are found with reflection:
  // ReSharper disable UnusedMemberInSuper.Global
  /// <summary>
  /// The context used to print a specification;
  /// use as input settings to the <see cref="ContentWriter"/>.
  /// </summary>
  public interface SpecContext {
    string SubjectName { get; }
    string Namespace { get; }
    string Title { get; }
    Type TargetType { get; }
    Type SerializerType { get; }
    IEnumerable<CalledMethodInfo> CalledMethods { get; }
    string BoostrapClassName { get; }

    NameEnumerator NameEnumeratorFor(string baseName);
  }
  // ReSharper restore UnusedMemberInSuper.Global
}