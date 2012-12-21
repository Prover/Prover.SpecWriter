namespace Prover.SpecWriter {
  /// <summary>
  /// A spec content writer is responsible for taking a trace and
  /// outputting a string that corresponds to a file that is a stand-alone
  /// spec/test fixture.
  /// </summary>
  public interface ContentWriter {
    /// <summary>
    /// Create the spec file data from a <see cref="SpecContext"/>.
    /// </summary>
    /// <param name="context">Non null spec context</param>
    /// <returns>A string containing the spec</returns>
    string WriteContext(SpecContext context);
  }
}