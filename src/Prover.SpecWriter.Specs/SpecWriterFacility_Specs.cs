using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using System.Linq;
using Prover.SpecWriter.Specs.Framework;
using Prover.SpecWriter.StatementLevel;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Prover.SpecWriter.Specs {
  [Subject(typeof(SpecWriterFacility))]
  public class When_getting_formatters_from_container {
    Establish context = () => {
      container = new WindsorContainer();
      expected = typeof (SpecWriterFacility)
        .Assembly
        .GetTypes()
        .Where(t => t.IsAbstract == false)
        .Where(t => t.Namespace.EndsWith(".Formatters"))
        .ToList();
    };

    static WindsorContainer container;
    
    Cleanup after = () => container.Dispose();

    Because adding_facility = () => {
      container.AddFacility<SpecWriterFacility>();
      actual = container
        .ResolveAll<Formatter>()
        .Select(x => x.GetType())
        .ToList();
    };

    static List<Type> actual;
    static List<Type> expected;

    It should_not_have_empty_count = 
      () => actual.ShouldNotBeEmpty();
    
    It should_have_same_count = 
      () => expected.Count.ShouldEqual(actual.Count);
    
    It should_have_all_formatters_in_formatters_namespace = 
      () => actual.ShouldContain(expected);

    It should_have_only_formatters = 
      () => expected.ShouldEachConformTo(t => typeof (Formatter).IsAssignableFrom(t));
  }


  [Subject(typeof (SpecWriterFacility))]
  public class When_getting_enumerable_of_formatters {
    Establish context = () => {
      container = new WindsorContainer();
      expected = typeof(SpecWriterFacility)
        .Assembly
        .GetTypes()
        .Where(t => t.IsAbstract == false)
        .Where(t => t.Namespace.EndsWith(".Formatters"))
        .ToList();
    };

    static WindsorContainer container;

    Cleanup after = () => container.Dispose();

    Because adding_facility = () => {
      container.AddFacility<SpecWriterFacility>();
      actual = container
        .Resolve<IEnumerable<Formatter>>()
        .Select(x => x.GetType())
        .ToList();
    };

    static List<Type> actual;
    static List<Type> expected;

    It should_not_have_empty_count =
      () => actual.ShouldNotBeEmpty();

    It should_have_same_count =
      () => expected.Count.ShouldEqual(actual.Count);

    It should_have_all_formatters_in_formatters_namespace =
      () => actual.ShouldContain(expected);

    It should_have_only_formatters =
      () => expected.ShouldEachConformTo(t => typeof(Formatter).IsAssignableFrom(t));
  }

  [Subject(typeof(SpecWriterFacility))]
  public class When_registering_component_before_facility {
    Establish context = () => {
      container = new WindsorContainer();
      container.Register(Component.For<S>().ImplementedBy<A>());
      container.AddFacility<SpecWriterFacility>();
      actor = container.Resolve<S>();
      RecordingInterceptorFinder.InterceptorsFor(actor, out data, out control)
        .ShouldBeTrue();
    };

    static IWindsorContainer container;

    Cleanup after = 
      () => container.Dispose(actor, control, data);

    Because calling_DoOp =
      () => control.Trace(() => actor.DoOp("hi"));

    static S actor;
    static RecordingControlPanel control;
    static RecordDataKeeper data;

    It should_be_same_reference =
      () => control.ShouldBeTheSameAs(control);

    It should_have_called_once =
      () => data.GetCalledMethods()
                .ShouldNotBeEmpty();

    public interface S {
      void DoOp(string x);
    }

    [RecordInteractions]
    class A : S {
      public void DoOp(string x) {
      }
    }
  }

  [Subject(typeof(SpecWriterFacility))]
  public class When_overriding_variable_formatter {

    class DumbFormatter 
      : VariableFormatter {
      public bool CanFormat(object o) {
        return o is ADTO;
      }
      public string Format(object o, string targetVarName) {
        return string.Format("var {0} = new ADTO(1,2,3,\"a\"); // 'w00t-ADTO'", targetVarName);
      }
    }

    static IWindsorContainer container;

    Establish context =
      () => container = WindsorContainerFactory.GetTracingContainer<S, A>(
        f => f.UseVariableFormatter<DumbFormatter>(),
        out data,
        out control,
        out actor);

    Cleanup afterwards = () => container.Dispose(actor, control, data);

    static S actor;
    static RecordingControlPanel control;
    static RecordDataKeeper data;

    Because calling_DoOp =
      () => control.Trace(() => actor.DoOp(new ADTO(6, 6, 6, "not what it gave")));

    It should_still_be_dumb_and_output_hardcoded_value =
      () => data.CreatePrintContext(container).Print().ShouldMatch(@"var \w+ = new ADTO\(1,2,3,""a""\); \/\/ 'w00t");

// ReSharper disable MemberCanBePrivate.Global
    public interface S {
      void DoOp(ADTO x);
    }
// ReSharper restore MemberCanBePrivate.Global

    [RecordInteractions]
    class A : S {
      public void DoOp(ADTO x)
      {
      }
    }
  }
}