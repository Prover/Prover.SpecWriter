using System.Linq;
using System.Text.RegularExpressions;

namespace Prover.SpecWriter.Templating {
  public static class SimpleTemplateEngine {
    public static string Render(string template, 
      Impl.Lookup<string, string> dataLookup) {
      return Regex
        .Matches(template, @"\$[\w\.]+\$")
        .Cast<Match>()
        //.Select(x => x.Groups.Cast<Group>().Skip(1).First())
        .Aggregate(
          template,
          (str, match) => str.Replace(match.Value, dataLookup.Get(match.Value)));
    }
  }
}