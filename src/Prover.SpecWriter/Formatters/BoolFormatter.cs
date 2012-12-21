namespace Prover.SpecWriter.Formatters {
  public class BoolFormatter 
    : Formatter {
    public bool CanFormat(object o) {
      return o is bool;
    }

    public string Format(object o) {
      return ((bool) o) ? "true" : "false";
    }
  }
}