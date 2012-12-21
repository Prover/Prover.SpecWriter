namespace Prover.SpecWriter {
  /// <summary>
  /// Formatter that outputs the value in a variable.
  /// 
  /// Variable formatters are used when the parameter object cannot easily be 
  /// converted to a short and succinct string, and it would be better to let the 
  /// object data be specified outside the method invocation in the spec.
  /// </summary>
  public interface VariableFormatter {
    bool CanFormat(object o);
    string Format(object o, string targetVarName);
  }
}