using System.Globalization;

namespace Prover.SpecWriter.Formatters {
  public class SingleFormatter
    : Formatter{
    public bool CanFormat(object o) {
      return o is float;
    }

    public string Format(object o) {
      var f = ((float) o);
      if (float.IsNaN(f)) return "float.NaN";
      if (float.IsPositiveInfinity(f)) return "float.PositiveInfinity";
      if (float.IsNegativeInfinity(f)) return "float.NegativeInfinity";
      return f.ToString(CultureInfo.InvariantCulture) + "f";
    }
  }
}