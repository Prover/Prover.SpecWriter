using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using Prover.SpecWriter.Specs.Framework;
using Prover.SpecWriter.StatementLevel;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Prover.SpecWriter.Specs {
  [Subject(typeof(StmtPrinter))]
  public class When_printing_call_to_unit_method {
    Establish context = () =>
      container = WindsorContainerFactory.GetTracingContainer<S, A>(
        f => { },
        out dataSubject,
        out control,
        out actor);

    static IWindsorContainer container;

    Cleanup after = () => container.Dispose(actor, control, dataSubject);

    Because calling_unit_method = () => control.Trace(() => actor.X());

    It should_format_call_with_unit_with_explicit_interface_comment = () =>
      dataSubject.CreatePrintContext(container).Print().ShouldEqual(@"(subject as Prover.SpecWriter.Specs.S).X();
");

    static S actor;
    static RecordDataKeeper dataSubject;
    static RecordingControlPanel control;
  }

  [Subject(typeof(StmtPrinter))]
  public class When_printing_call_to_method_with_primitives {
    Establish context = () => {
      container = WindsorContainerFactory.GetTracingContainer(Component.For<S>().ImplementedBy<A>());
      actor = container.Resolve<S>();
      printContextFactory = container.Resolve<PrintContextFactory>();

      RecordingInterceptorFinder.InterceptorsFor(actor, out dataSubject, out control)
        .ShouldBeTrue();
    };

    static IWindsorContainer container;
    static PrintContextFactory printContextFactory;

    Cleanup after = () => {
      container.Release(printContextFactory);
      container.Dispose();
    };

    Because calling_primitive_method = () => control.Trace(() => actor.Y(5, false, 45.0222M));

    It should_format_call_with_primitives = 
      () => dataSubject.CreatePrintContext(container).Print().ShouldEqual(@"subject.Y(5, false, 45.0222M);
");

    static S actor;
    static RecordDataKeeper dataSubject;
    static RecordingControlPanel control;
  }

  [Subject(typeof(StmtPrinter))]
  public class When_printing_call_to_DTO_method {

    Establish context = () => {
      container = WindsorContainerFactory.GetTracingContainer<S, A>(f => { },
        out dataSubject, 
        out control,
        out actor);
    };

    static IWindsorContainer container;

    Cleanup after = () => container.Dispose(actor, control, dataSubject);

    Because calling_Z = 
      () => {
        control.Trace(() => actor.Z(new ADTO(4, 5, 6, "abc"), new FormattingDTO(6)));
        print = dataSubject.CreatePrintContext(container).Print();
      };

    static S actor;
    static RecordDataKeeper dataSubject;
    static RecordingControlPanel control;
    static string print;

    // about serialization: http://msdn.microsoft.com/en-us/library/72hyey7b%28v=vs.90%29.aspx
    It should_format_call_with_dto = () => print.ShouldEqual(
      @"var dto = new[]{ (byte) 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 12, 2, 0, 0, 0, 78, 80, 114, 111, 118, 101, 114, 46, 83, 112, 101, 99, 87, 114, 105, 116, 101, 114, 46, 83, 112, 101, 99, 115, 44, 32, 86, 101, 114, 115, 105, 111, 110, 61, 49, 46, 48, 46, 48, 46, 48, 44, 32, 67, 117, 108, 116, 117, 114, 101, 61, 110, 101, 117, 116, 114, 97, 108, 44, 32, 80, 117, 98, 108, 105, 99, 75, 101, 121, 84, 111, 107, 101, 110, 61, 110, 117, 108, 108, 5, 1, 0, 0, 0, 28, 80, 114, 111, 118, 101, 114, 46, 83, 112, 101, 99, 87, 114, 105, 116, 101, 114, 46, 83, 112, 101, 99, 115, 46, 65, 68, 84, 79, 4, 0, 0, 0, 1, 97, 1, 98, 1, 99, 3, 97, 98, 99, 0, 0, 0, 1, 8, 8, 8, 2, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 6, 0, 0, 0, 6, 3, 0, 0, 0, 3, 97, 98, 99, 11 };
subject.Z(serializer.Deserialize<Prover.SpecWriter.Specs.ADTO>(dto), new FormattingDTO(6));
");
  }

  public interface S {
    void X();
    void Y(int a, bool b, decimal c);
    void Z(ADTO dto, object itemz);
  }

  [RecordInteractions]
  public class A : S {
    void S.X()
    {
    }
    public void Y(int a, bool b, decimal c)
    {
    }

    public void Z(ADTO dto, object itemz)
    {
    }
  }
}