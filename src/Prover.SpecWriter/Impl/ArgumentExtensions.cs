using System;

namespace Prover.SpecWriter.Impl {
  /// <summary>Internal helper class for formatting arguments.</summary>
  static class ArgumentExtensions {
    public static string OwnFormatOr(this object item, Func<object, string> alt) {
      if (ReferenceEquals(item, null)) return "null";
      return item is OwnFormatter ? (item as OwnFormatter).FormatCreate() : alt(item);
    }
  }
}