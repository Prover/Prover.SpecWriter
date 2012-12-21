namespace Prover.SpecWriter {
  /// <summary>
  /// Interface that DTOs can implement if they want to advice the SpecWriter
  /// how they should be instantiated. Right now, that help is by using a string
  /// corresponding to the C# that creates the object.
  /// </summary>
  public interface OwnFormatter {
    string FormatCreate();
  }
}