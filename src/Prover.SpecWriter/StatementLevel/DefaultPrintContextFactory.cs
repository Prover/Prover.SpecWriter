using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prover.SpecWriter.StatementLevel {
  public class DefaultPrintContextFactory 
    : PrintContextFactory {
    readonly VariableFormatter variableFormatter;
    readonly IEnumerable<Formatter> formatters;

    public DefaultPrintContextFactory(
      Logger logger,
      VariableFormatter variableFormatter,
      IEnumerable<Formatter> formatters)
    {
      logger.Debug(string.Format("using default formatter: {0}", variableFormatter.GetType().Name));
      logger.Debug(string.Format("using other formatters: {0}",
        string.Join(", ", formatters
          .Select(f => f.GetType().Name)
          .ToArray())));
      this.variableFormatter = variableFormatter;
      this.formatters = formatters;
    }

    PrintContext PrintContextFactory.Create(CalledMethodInfo info,
                                            NameEnumeratorFactory nameEnumeratorFac,
                                            PrintFormatting formatting,
                                            PrintNaming naming) {
      return new PrintContextImpl(
        nameEnumeratorFac, new StringBuilder(),
        info, variableFormatter, formatters, naming.SubjectName,
        naming.SerializerName, formatting.SpaceIndentation,
        null);
    }
  }
}