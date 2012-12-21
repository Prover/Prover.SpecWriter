using Machine.Specifications;
using Prover.SpecWriter.Impl;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Prover.SpecWriter.Specs {
  [Subject(typeof(SimpleNameCache))]
  public class When_getting_same_enumerator {
    Establish context = () => subject = new SimpleNameCache();

    Because generating_initial_name = () => {
      var abcEnum1 = subject.Get("abc");
      var abcEnum2 = subject.Get("abc");
      abcEnum1.ShouldBeTheSameAs(abcEnum2);

      var abc1 = abcEnum1.Next();
      var abc2 = abcEnum2.Next();
      abc1.ShouldNotEqual(abc2);
    };

    static SimpleNameCache subject;

    It should_generate_input_initially =
      () => subject.Get("another").Next().ShouldEqual("another");

    It should_return_different_instances_for_different_vars =
      () => subject.Get("happy").ShouldNotBeTheSameAs(subject.Get("abc"));

    It should_generate_third_abc =
      () => subject.Get("abc").Next().ShouldEqual("abc2");
  }
}