namespace Prover.SpecWriter {
  /// <summary>
  /// Implement formatters for your own types and
  /// register them in the container.
  /// </summary>
  public interface Formatter {

    /// <summary>
    /// Return true of the formatter is capable of formatting the object
    /// passed.
    /// </summary>
    bool CanFormat(object o);

    /// <summary>
    /// Format the object with an optional variable
    /// name.
    /// </summary>
    string Format(object o);
  }
}