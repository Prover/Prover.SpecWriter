namespace Prover.SpecWriter {
  /// <summary>
  /// Serializers should be able to serialize/deserialize
  /// object graphs into byte arrays. They are used when serializing
  /// an incoming parameter value into something that is placed in the test,
  /// and as such, is the last-resort when trying to represent the data that enters
  /// the methods. If you have a custom data type that you want formatted nicely,
  /// then give it the interface <see cref="OwnFormatter"/> or provide a generic formatter
  /// for it by registering it in the container.
  /// </summary>
  public interface Serializer {
    /// <summary>
    /// Create a byte array from the object graph.
    /// </summary>
    byte[] Serialize<T>(T obj);

    /// <summary>Deserialize the object graph into an instance with a type.</summary>
    T Deserialize<T>(byte[] bytes);
  }
}