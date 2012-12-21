namespace Prover.SpecWriter.Formatters {
  public class NullFormatter 
    : Formatter{
    public bool CanFormat(object o) {
      return ReferenceEquals(o, null);
    }

    public string Format(object o) {
      return "null";
    }
  }
}