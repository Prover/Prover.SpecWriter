using System;
using Prover.SpecWriter.Formatters;

namespace Prover.SpecWriter.Specs {
  [Serializable]
  public class FormattingDTO
    : OwnFormatter {
    readonly int val;

    public int Val {
      get { return val; }
    }

    public FormattingDTO(int val) {
      this.val = val;
    }

    string OwnFormatter.FormatCreate() {
      return string.Format("new FormattingDTO({0})", val);
    }
  }
}