using System.Globalization;

namespace Prover.SpecWriter.Formatters {
  public class DecimalFormatter 
    : Formatter {
    public bool CanFormat(object o) {
      return o is decimal;
    }

    public string Format(object o) {
      var d = (decimal) o;
      if (d.Equals(decimal.MaxValue)) return "decimal.MaxValue";
      if (d.Equals(decimal.MinValue)) return "decimal.MinValue";
      if (d.Equals(decimal.MinusOne)) return "decimal.MinusOne";
      if (d.Equals(decimal.Zero)) return "decimal.Zero";
      return ((decimal) o).ToString(CultureInfo.InvariantCulture) + "M";
    }
  }
}