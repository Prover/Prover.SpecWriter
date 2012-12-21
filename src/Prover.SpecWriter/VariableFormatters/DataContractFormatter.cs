using System.IO;
using System.Xml;

namespace Prover.SpecWriter.VariableFormatters {
  public class DataContractFormatter
    : VariableFormatter {
    public bool CanFormat(object o) {
      return true;
    }

    public string Format(object o, string targetVarName) {
      return string.Format("var {0} = @\"{1}\";", targetVarName, GetXml(o));
    }

    internal static string GetXml(object o) {
      var formatter = new System.Runtime.Serialization.DataContractSerializer(o.GetType());
      using (var stringWriter = new StringWriter())
      using (var xmlWriter = new XmlTextWriter(stringWriter)) {
        xmlWriter.Formatting = Formatting.Indented;
        xmlWriter.QuoteChar = '\'';
        formatter.WriteObject(xmlWriter, o);
        return stringWriter.ToString();
      }
    }
  }
}