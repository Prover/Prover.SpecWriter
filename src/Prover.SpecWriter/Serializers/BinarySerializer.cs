using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Prover.SpecWriter.Serializers {
  public class BinarySerializer
    : Serializer {
    readonly BinaryFormatter inner;

    public BinarySerializer() {
      inner = new BinaryFormatter();
    }

    public byte[] Serialize<T>(T obj) {
      using (var ms = new MemoryStream()) {
        inner.Serialize(ms, obj);
        return ms.ToArray();
      }
    }

    public T Deserialize<T>(byte[] bytes) {
      using (var ms = new MemoryStream(bytes))
        return (T)inner.Deserialize(ms);
    }
  }
}