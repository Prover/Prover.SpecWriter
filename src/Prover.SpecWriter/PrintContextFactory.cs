using Prover.SpecWriter.Impl;
using Prover.SpecWriter.StatementLevel;

namespace Prover.SpecWriter {
  public interface PrintContextFactory {
    PrintContext Create(CalledMethodInfo info, NameEnumeratorFactory nameEnumeratorFac, PrintFormatting formatting, PrintNaming naming);
  }
}