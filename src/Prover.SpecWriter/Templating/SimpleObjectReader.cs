using System;
using System.Linq;

namespace Prover.SpecWriter.Templating {
  public static class SimpleObjectReader {
    /// <summary>Reads the property value from the SpecContext (into the template)</summary>
    public static string GetProperty(string[] split, object o) {
      if (split.Length == 0) return Convert.ToString(o);
      var prop = split[0];
      var t = o.GetType().GetProperty(prop);
      return GetProperty(
        split.Skip(1).ToArray(),
        t.GetValue(o, null));
    }
  }
}