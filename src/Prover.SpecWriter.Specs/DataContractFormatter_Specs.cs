using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Castle.Windsor;
using Machine.Specifications;
using Prover.SpecWriter.Specs.Framework;
using Prover.SpecWriter.VariableFormatters;
using Prover.SpecWriter.StatementLevel;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Prover.SpecWriter.Specs {
  [Subject(typeof(DataContractFormatter))]
  public class When_formatting_with_data_contract_formatter {

    static IWindsorContainer container;

    Establish context =
      () => container = WindsorContainerFactory.GetTracingContainer<S, A>(
        f => f.UseVariableFormatter<DataContractFormatter>(),
        out data,
        out control,
        out actor);

    Cleanup afterwards = () => container.Dispose(actor, control, data);

    Because calling_DoOp =
      () => {
        control.Trace(() => actor.DoOp(new ADTO(6, 7, 8, funkyString)));
        print = data.CreatePrintContext(container).Print();
      };

    const string funkyString = "this まやら åäö ^*á &very; <strange> str ½!";

    static string print;
    static S actor;
    static RecordingControlPanel control;
    static RecordDataKeeper data;

    It should_output_some_xml = () => {
      print.ShouldContain(@"<ADTO ");
      print.ShouldContain("</ADTO>");
    };

    It should_contain_xml_encoded_string =
      () => data.CreatePrintContext(container)
                .Print()
                .ShouldContain("<abc>this まやら åäö ^*á &amp;very; &lt;strange&gt; str ½!</abc>");

    It should_be_possible_to_deserialize_again =
      () => {
        var start = print.IndexOf("<ADTO", StringComparison.Ordinal);
        const string endVal = "</ADTO>";
        var end = print.IndexOf(endVal, StringComparison.Ordinal);
        var xml = print.Substring(start, end + endVal.Length - start);

        Console.WriteLine("Trying to write XML: {0}", xml);

        var deserializer = new DataContractSerializer(typeof (ADTO));
        using (var stringReader = new StringReader(xml))
        using (var xmlReader = new XmlTextReader(stringReader)) {
          var adto = (ADTO) deserializer.ReadObject(xmlReader, true);
          adto.A.ShouldEqual(6);
          adto.B.ShouldEqual(7);
          adto.C.ShouldEqual(8);
          adto.Abc.ShouldEqual(funkyString);
        }
      };

    // ReSharper disable MemberCanBePrivate.Global
    public interface S
    {
      void DoOp(ADTO paramName);
    }
    // ReSharper restore MemberCanBePrivate.Global

    [RecordInteractions]
    class A : S {
      public void DoOp(ADTO paramName)
      {
      }
    }
  }
}