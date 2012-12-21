using System.Collections.Generic;
using System.Text;

namespace Prover.SpecWriter.StatementLevel {
  public interface PrintContext 
    : PrintNaming, PrintFormatting {
    StringBuilder Builder { get; }
    CalledMethodInfo Info { get; }
    string[] WrittenVars { get; }
    VariableFormatter VariableFormatter { get; }
    IEnumerable<Formatter> CustomFormatters { get; }
    IEnumerable<Formatter> AllFormatters { get; }
    
    /// <summary>
    /// Get new variable name
    /// </summary>
    NameEnumerator ForBaseName(string baseName);
    
    PrintContext SetWrittenVars(string[] writtenVars);
  }
}