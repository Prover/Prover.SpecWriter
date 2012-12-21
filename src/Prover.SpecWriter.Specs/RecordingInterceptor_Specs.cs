using System;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using Prover.SpecWriter.Impl;
using Prover.SpecWriter.Specs.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Prover.SpecWriter.Specs {
  [Subject(typeof (RecordingInterceptorImpl))]
  public class When_listing_calls {

    static BService _bService;
    static AService _aService;
    static RecordDataKeeper sut;
    static RecordingControlPanel control;
    static IWindsorContainer container;

    Establish context = () => {
      container = WindsorContainerFactory.GetTracingContainer(
        Component.For<AService, BService>().ImplementedBy<Service>()
        );

      _aService = container.Resolve<AService>();
      _bService = container.Resolve<BService>();
      _aService.ShouldBeTheSameAs(_bService);

      RecordingInterceptorFinder.InterceptorsFor(_aService, out sut, out control)
        .ShouldBeTrue();
    };

    Cleanup after = () => container.Dispose();

    Because calling_methods_on_b = () => {
      control.StartTrace();
      _aService.DoA(56, new ADTO(5, 6, 7, "Hello World"));
      _bService.DoB("nuff said, here's the dough!", new Money {Amount = 42.0M});
      _aService.DoA(-5, new ADTO(4, 2, 0, "OK, then."));
      control.EndTrace();
    };

    It should_be_able_to_print_called_methods =
      () => sut.GetCalledMethods().Select(x => x.Method.Name)
               .ShouldContain("DoA", "Prover.SpecWriter.Specs.When_listing_calls.BService.DoB");

    It should_know_three_were_called = () => sut.GetCalledMethods().Count().ShouldEqual(3);
    It should_not_have_empty_enumerable_of_methods = () => sut.GetCalledMethods().ShouldNotBeEmpty();

    It should_not_have_noted_DoC =
      () => sut.GetCalledMethods().Select(x => x.Method.Name)
               .ShouldNotContain("DoC");


    public interface AService {
      void DoA(int primitive, ADTO dto);
    }

    public interface BService {
      void DoB(string anotherPrimitive, Money money);
    }

    public interface CService {
      int DoC();
    }

    [RecordInteractions]
    public class Service
      : AService, BService, CService {
      public void DoA(int primitive, ADTO dto) {
        Console.WriteLine("DoA({0}, {1})", primitive, dto);
      }

      void BService.DoB(string anotherPrimitive, Money money) {
        Console.WriteLine("DoA({0}, {1})", anotherPrimitive, money);
      }

      int CService.DoC() {
        return 10;
      }
    }
  }

  [Subject(typeof (RecordingInterceptorImpl))]
  public class When_replaying_a_call {
    Establish context = () => {
      container = WindsorContainerFactory.GetTracingContainer(
        Component.For<AService>().ImplementedBy<Service>().Forward<TestData>()
        );
      
      actor = container.Resolve<AService>();
      
      ;
      RecordingInterceptorFinder.InterceptorsFor(actor, out data, out control)
        .ShouldBeTrue();
    };

    Cleanup after = () => container.Dispose();

    static IWindsorContainer container;
    static AService actor;
    static RecordingControlPanel control;
    static RecordDataKeeper data;

    Because calling_method_once_then_replaying =
      () => {
        control.StartTrace();
        actor.DoA(56, new ADTO(2, 4, 6, "hi"));
        control.EndTrace();
        data.Replay();
      };

    It should_have_correct_end_state = () => actor.DoAsserts();

    public interface AService {
      void DoA(int primitive, ADTO dto);
      void DoAsserts();
    }

    [RecordInteractions]
    public class Service
      : AService {
      int LastPrimitive { get; set; }
      ADTO LastDto { get; set; }
      int Calls { get; set; }

      public void DoA(int primitive, ADTO dto) {
        Console.WriteLine("DoA({0}, {1}) called", primitive, dto);
        LastPrimitive = primitive;
        LastDto = dto;
        Calls++;
      }

      public void DoAsserts() {
        Calls.ShouldEqual(2);
        LastPrimitive.ShouldEqual(56);
        LastDto.Abc.ShouldEqual("hi");
      }
    }

    public interface TestData {
      int LastPrimitive { get; }
      ADTO LastDto { get; }
      int Calls { get; }
    }
  }
}