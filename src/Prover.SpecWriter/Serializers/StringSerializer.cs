using System.IO;
using System.Xml;
using Prover.SpecWriter.VariableFormatters;

namespace Prover.SpecWriter.Serializers {
  public class DataContractSerializer 
    : StringSerializer {
    public T Deserialize<T>(string data) {
      var formatter = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
      using (var stringReader = new StringReader(data))
      using (var xmlReader = new XmlTextReader(stringReader)) {
        return (T)formatter.ReadObject(xmlReader, true);
      }
    }

    public string Serialize<T>(T data) {
      return DataContractFormatter.GetXml(data);
    }
  }
}