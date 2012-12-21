using System;
using System.Text.RegularExpressions;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using Prover.SpecWriter.Naming;
using Prover.SpecWriter.Serializers;
using Prover.SpecWriter.SpecLevel;
using Prover.SpecWriter.SpecWriters;
using Prover.SpecWriter.Specs.Framework;
using System.Linq;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Prover.SpecWriter.Specs {
  [Subject(typeof(MSpecWriter))]
  public class When_printing_interceptions_to_spec {
    static RecordingControlPanel control;
    static S actor;

    Establish context = () => {
      container = WindsorContainerFactory.GetTracingContainer<S, A>(
        f => { },
        out dataKeeper,
        out control,
        out actor);

      control.Trace(() => {
        actor.Do5(new FormattingDTO(354));
        actor.Do5(new FormattingDTO(45));
      });

      subject = container.Resolve<ContentWriter>();
    };

    static IWindsorContainer container;

    Cleanup after = () => {
      container.Release(subject);
      container.Dispose(actor, control, dataKeeper);
    };

    Because printing_with_context = () => {
      var ctx = new SpecContextImpl(
        _subjectName,
        _bootstrapName,
        "Prover.SpecWriter.Specs",
        "calling A some times".Replace(" ", "_"),
        dataKeeper.GetCalledMethods(),
        dataKeeper.GetCalledMethods().First().Target,
        variable => new SimpleNameEnumerator(variable),
        typeof(DataContractSerializer));

      output = subject.WriteContext(ctx);
    };

    It should_output_subject_name = () => output.ShouldMatch(string.Format("{0} = Bootstrap", _subjectName));
    It should_output_call_to_subject_1 = () => output.ShouldContain("subject.Do5(new FormattingDTO(354));");
    It should_output_call_to_subject_2 = () => output.ShouldContain("subject.Do5(new FormattingDTO(45));");
    It should_output_call_to_Boostrappy = () => output.ShouldContain("= Bootstrappy.AUnderTest(out cleanup);");

    [Ignore("for debugging only")] 
    It can_print_to_console = () => Console.WriteLine(output);

    static RecordDataKeeper dataKeeper;
    static ContentWriter subject;
    static string output;
    const string _subjectName = "subject";
    const string _bootstrapName = "Bootstrappy";

    public interface S {
      int Do5(FormattingDTO factor);
    }

    [RecordInteractions]
    class A : S {
      public int Do5(FormattingDTO factor) {
        return 5*factor.Val;
      }
    }
  }

  [Subject(typeof(MSpecWriter))]
  public class When_printing_multiple_same_values
  {
    Establish context = () => {
      container = WindsorContainerFactory.GetTracingContainer(Component.For<S>().ImplementedBy<A>());

      var actor = container.Resolve<S>();

      RecordingControlPanel control;
      RecordingInterceptorFinder.InterceptorsFor(actor, out dataKeeper, out control)
        .ShouldBeTrue();

      control.Trace(() => {
        actor.Do5(new ADTO(354, 35, 56, "abc"));
        actor.Do5(new ADTO(354, 35, 56, "abc"));
      });

      container.Release(actor);

      subject = container.Resolve<ContentWriter>();
    };

    static IWindsorContainer container;

    Cleanup after = () => container.Dispose();

    Because printing_with_context = () => {
      output = subject.WriteContext(
        new SpecContextImpl(
          dataKeeper.GetCalledMethods().First().Target,
          dataKeeper.GetCalledMethods(),
          s => new SimpleNameEnumerator(s)));
    };

    static RecordDataKeeper dataKeeper;
    static ContentWriter subject;
    static string output;

    It should_only_print_factor_at_least_once =
      () => Regex.Matches(output,
                          "var factor =")
                 .Count
                 .ShouldBeGreaterThan(0);

    It should_generate_new_names =
      () => {
        output.ShouldMatch("var factor =");
        output.ShouldMatch("var factor1 =");
      };

    public interface S {
      int Do5(ADTO factor);
    }

    [RecordInteractions]
    class A : S {
      public int Do5(ADTO factor) {
        return 5 * factor.A;
      }
    }
  }
}