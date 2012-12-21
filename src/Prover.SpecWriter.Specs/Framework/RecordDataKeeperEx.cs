using System.Linq;
using Castle.Windsor;
using Prover.SpecWriter.Naming;
using Prover.SpecWriter.StatementLevel;

namespace Prover.SpecWriter.Specs.Framework {
  static class RecordDataKeeperEx {
    public static PrintContext CreatePrintContext(
      this RecordDataKeeper dataSubject, IWindsorContainer container) {
      var f = container.Resolve<PrintContextFactory>();
      try {
        return f.Create(
          dataSubject.GetCalledMethods().First(), 
          s => new SimpleNameEnumerator(s),
          new PrintFormattingImpl(),
          new PrintNamingImpl());
      }
      finally {
        container.Release(f);
      }
    }

    class PrintFormattingImpl : PrintFormatting {
      public int SpaceIndentation { get { return 0; } }
    }
    class PrintNamingImpl : PrintNaming {
      public string SubjectName { get { return "subject"; } }
      public string SerializerName { get { return "serializer"; } }
    }
  }
}