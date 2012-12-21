namespace Prover.SpecWriter.Formatters {
  public class StringFormatter
    : Formatter{
    public bool CanFormat(object o) {
      return o is string;
    }

    public string Format(object o) {
      return "\"" + (string) o + "\"";
    }
  }
}