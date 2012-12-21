using System.Globalization;

namespace Prover.SpecWriter.Formatters {
  public class PrimitiveFormatter
    : Formatter {
    public bool CanFormat(object o) {
      return o != null && o.GetType().IsPrimitive;
    }

    public string Format(object o) {
      return string.Format(CultureInfo.InvariantCulture, "{0}", o);
    }
  }
}