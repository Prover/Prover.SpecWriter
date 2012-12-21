using System.Globalization;

namespace Prover.SpecWriter.Formatters {
  public class DoubleFormatter
    : Formatter {
    public bool CanFormat(object o) {
      return o is double;
    }

    public string Format(object o) {
      var d = (double) o;
      if (double.IsPositiveInfinity(d)) return "double.PositiveInfinity";
      if (double.IsNegativeInfinity(d)) return "double.NegativeInfinity";
      if (double.IsNaN(d)) return "double.NaN";
      return d.ToString(CultureInfo.InvariantCulture) + "d";
    }
  }
}