using System;
using System.Linq;
using System.Text;
using Prover.SpecWriter.StatementLevel;
using Prover.SpecWriter.Templating;

namespace Prover.SpecWriter.SpecWriters {
  public class MSpecWriter
    : ContentWriter {
    readonly PrintContextFactory printCtxFac;

    const string MSpecDefault = @"
using System;
using Machine.Specifications;
using Prover.SpecWriter;
using Prover.SpecWriter.Serializers;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace $Namespace$ {
  [Subject(typeof($TargetType.FullName$))]
  public class When_$Title$ {
    Establish context = () => {
      $SubjectName$ = $BoostrapClassName$.$TargetType.Name$UnderTest(out cleanup);
      serializer = new $SerializerType.Name$();
    };

    Cleanup after = () => cleanup.Dispose();

    static IDisposable cleanup;
    static StringSerializer serializer;
    static $TargetType.FullName$ subject;

    Because calling_$CalledMethods.Count$_methods = () => {
$METHOD_CALLS$
    };

    It should_not_throw_exceptions = 
      () => true.ShouldBeTrue();
  }
}";

    public const int SpaceIndentation = 6;
    public const string SerializerName = "serializer";

    public MSpecWriter(PrintContextFactory printCtxFac) {
      if (printCtxFac == null) throw new ArgumentNullException("printCtxFac");
      this.printCtxFac = printCtxFac;
    }

    /// <summary>
    /// You can override the template if you wish; this is what the MSpecWriter uses
    /// as its template when rendering.
    /// </summary>
    public virtual string MSpecTemplate {
      get { return MSpecDefault; }
    }

    PrintContext PrintContextFactory(SpecContext ctx, CalledMethodInfo cmi) {
      return printCtxFac.Create(cmi, 
        ctx.NameEnumeratorFor, 
        new PrintFormattingImpl(SpaceIndentation),
        new PrintNamingImpl(ctx.SubjectName, SerializerName));
    }

    class PrintFormattingImpl
      : PrintFormatting {
      readonly int spaceIndentation;

      public PrintFormattingImpl(int spaceIndentation) {
        this.spaceIndentation = spaceIndentation;
      }

      int PrintFormatting.SpaceIndentation {
        get { return spaceIndentation; }
      }
    }

    class PrintNamingImpl
      : PrintNaming {
      readonly string subjectName, serializerName;

      public PrintNamingImpl(string subjectName, string serializerName) {
        this.subjectName = subjectName;
        this.serializerName = serializerName;
      }

      string PrintNaming.SubjectName {
        get { return subjectName; }
      }

      string PrintNaming.SerializerName {
        get { return serializerName; }
      }
    }

    string ContentWriter.WriteContext(SpecContext context) {
      return SimpleTemplateEngine.Render(
        MSpecTemplate,
        new DataLookup(varName => Lookup(varName, context, cmi => PrintContextFactory(context, cmi))));
    }

    static string Lookup(string placeholder, SpecContext specCtx, Func<CalledMethodInfo, PrintContext> printCtx) {
      switch (placeholder) {
        case "$METHOD_CALLS$":
          return specCtx.CalledMethods
            .Aggregate(new StringBuilder(), (str, cmi) => {
              str.AppendLine(printCtx(cmi).Print());
              return str;
            }, sb => sb.ToString());

        default:
          return SimpleObjectReader.GetProperty(
            placeholder.Trim('$').Split('.'), 
            specCtx);
      }
    }

    class DataLookup : Impl.Lookup<string, string> {
      readonly Func<string, string> f;

      public DataLookup(Func<string, string> f) {
        this.f = f;
      }

      public string Get(string key) {
        return f(key);
      }
    }
  }
}