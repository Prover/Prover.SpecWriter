namespace Prover.SpecWriter {
  public interface StringSerializer {
    T Deserialize<T>(string data);
    
    /// <summary>
    /// Serialize the data to a string.
    /// </summary>
    string Serialize<T>(T data);
  }
}