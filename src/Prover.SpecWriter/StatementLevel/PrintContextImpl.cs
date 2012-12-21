using System;
using System.Collections.Generic;
using System.Text;

namespace Prover.SpecWriter.StatementLevel {

  public delegate NameEnumerator NameEnumeratorFactory(string variableName);

  public class PrintContextImpl
    : PrintContext {
    readonly LinkedList<Formatter> customFormatters;
    readonly NameEnumeratorFactory nameEnumeratorFac;

    public PrintContextImpl(NameEnumeratorFactory nameEnumeratorFac, StringBuilder builder, CalledMethodInfo info, VariableFormatter variableFormatter, IEnumerable<Formatter> customFormatters, string subjectName, string serializerVarName, int spaceIndentation, string[] writtenVars) {
      if (nameEnumeratorFac == null) throw new ArgumentNullException("nameEnumeratorFac");

      SpaceIndentation = spaceIndentation;
      Builder = builder;
      Info = info;
      SubjectName = subjectName;
      VariableFormatter = variableFormatter;
      WrittenVars = writtenVars;
      this.customFormatters = new LinkedList<Formatter>(customFormatters);
      SerializerName = serializerVarName;
      this.nameEnumeratorFac = nameEnumeratorFac;
    }

    public int SpaceIndentation { get; private set; }
    public StringBuilder Builder { get; private set; }
    public string SubjectName { get; private set; }
    public CalledMethodInfo Info { get; private set; }
    public string[] WrittenVars { get; private set; }
    public VariableFormatter VariableFormatter { get; private set; }

    public IEnumerable<Formatter> CustomFormatters {
      get { return customFormatters; }
    }

    public string SerializerName { get; private set; }

    public IEnumerable<Formatter> AllFormatters {
      get { return CustomFormatters; }
    }

    public NameEnumerator ForBaseName(string baseName) {
      return nameEnumeratorFac(baseName);
    }

    public PrintContext SetWrittenVars(string[] writtenVars) {
      return new PrintContextImpl(nameEnumeratorFac, Builder, Info, VariableFormatter, customFormatters, SubjectName, SerializerName, SpaceIndentation, writtenVars);
    }
  }
}