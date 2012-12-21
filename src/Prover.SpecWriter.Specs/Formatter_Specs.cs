using System;
using System.Globalization;
using Machine.Specifications;
using Prover.SpecWriter.Formatters;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Prover.SpecWriter.Specs {
  [Subject(typeof(BoolFormatter))]
  public class When_formatting_boolean {
    Establish context = () => {
      subject = new BoolFormatter();
    };

    static BoolFormatter subject;

    It can_format_true = () => subject.CanFormat(true).ShouldBeTrue();
    It should_return_true = () => subject.Format(true).ShouldEqual("true");
    It should_return_false = () => subject.Format(false).ShouldEqual("false");
  }

  [Subject(typeof(DecimalFormatter))]
  public class When_formatting_decimal {
    Establish context = () => subject = new DecimalFormatter();

    static DecimalFormatter subject;

    It can_format_a_decimal = () => subject.CanFormat(3M).ShouldBeTrue();
    It should_return_valid_value = () => subject.Format(2M).ShouldEqual("2M");
    It should_return_zero = () => subject.Format(decimal.Zero).ShouldEqual("decimal.Zero");
    It should_return_min_value = () => subject.Format(decimal.MinValue).ShouldEqual("decimal.MinValue");
    It should_return_max_value = () => subject.Format(decimal.MaxValue).ShouldEqual("decimal.MaxValue");
    It should_return_minus_one_value = () => subject.Format(decimal.MinusOne).ShouldEqual("decimal.MinusOne");
  }

  [Subject(typeof(SingleFormatter))]
  public class When_formatting_singles {
    Establish context = () => subject = new SingleFormatter();

    static SingleFormatter subject;

    It should_return_a_value = () => subject.Format(4f).ShouldEqual("4f");
    It should_return_nan = () => subject.Format(float.NaN).ShouldEqual("float.NaN");
    It should_return_pos_inf = () => subject.Format(float.PositiveInfinity).ShouldEqual("float.PositiveInfinity");
    It should_return_neg_inf = () => subject.Format(float.NegativeInfinity).ShouldEqual("float.NegativeInfinity");
  }

  [Subject(typeof(DoubleFormatter))]
  public class When_formatting_double {
    Establish context = () => subject = new DoubleFormatter();
    
    static DoubleFormatter subject;

    It can_format_double = () => subject.CanFormat(34d).ShouldBeTrue();
    It should_return_a_value = () => subject.Format(45d).ShouldEqual("45d");
    It should_return_NaN = () => subject.Format(double.NaN).ShouldEqual("double.NaN");
    It should_return_pos_inf = () => subject.Format(double.PositiveInfinity).ShouldEqual("double.PositiveInfinity");
    It should_return_neg_inf = () => subject.Format(double.NegativeInfinity).ShouldEqual("double.NegativeInfinity");
  }

  [Subject(typeof(NullFormatter))]
  public class When_formatting_null {
    Establish context = () => subject = new NullFormatter();
    
    static NullFormatter subject;

    It can_format_null = () => subject.CanFormat(null).ShouldBeTrue();
    It should_return_null = () => subject.Format(null).ShouldEqual("null");
  }

  [Subject(typeof(PrimitiveFormatter))]
  public class When_formatting_other_primitives {
    Establish context = () => subject = new PrimitiveFormatter();
    
    static PrimitiveFormatter subject;
    static DateTime newYear = new DateTime(2013, 01, 01);

    It can_format_datetime = () => subject.CanFormat(newYear);
    
    It should_format_datetime_invariant_culture =
      () => subject.Format(newYear).ShouldEqual(newYear.ToString(CultureInfo.InvariantCulture));
  }

  [Subject(typeof(StringFormatter))]
  public class When_formatting_a_string {
    Establish context = () => subject = new StringFormatter();

    static Formatter subject;

    It can_format_string = () => subject.CanFormat("hi").ShouldBeTrue();
    It should_format_with_quotes = () => subject.Format("hi world").ShouldEqual(@"""hi world""");
  }
}