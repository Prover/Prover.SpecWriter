using System;
using System.Globalization;
using System.Linq;

namespace Prover.SpecWriter.VariableFormatters {
  public class BinaryVarFormatter 
    : VariableFormatter {

    readonly Serializer ser;

    public BinaryVarFormatter(Serializer ser) {
      if (ser == null) throw new ArgumentNullException("ser");
      this.ser = ser;
    }

    public bool CanFormat(object o) {
      return !ReferenceEquals(o, null);
    }

    public string Format(object o, string targetVarName) {
      var bytes = ser.Serialize(o)
                     .Select(x => x.ToString(CultureInfo.InvariantCulture))
                     .ToArray();

      // TODO: should this not be moved to a 'VariableFormatter' or the like?
      return string.Format("var {0} = new[]{{ (byte) {1} }};",
                           targetVarName,
                           string.Join(", ", bytes));
    }
  }
}