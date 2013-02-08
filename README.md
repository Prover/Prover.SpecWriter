# Prover SpecWriter

Prover SpecWriter is a project that makes it easy to record interactions between
and targeted at objects living in a CLR process; and then serialize these interactions
to a specification or unit-test.

### Suggested Areas of Use

The component has proven useful when dealing with legacy systems without unit tests,
as 'baseline' interaction tests can quickly be set up and run as a part of a test
suite testing for regressions.

Another possible use-case is recording of game-play or GUI interactions. It can be hard
to produce realistic test-data when interactions are stateful - SpecWriter gives you
the ability to create this state from the ground up, written as a test-fixture.

## API

The API should be almost fully documented with C# XML docs in the code.

In its initial release, it is based around the Castle Windsor Dynamic Proxy,
and as such, you configure SpecWriterFacility:

```
container.AddFacility<SpecWriterFacility>();
```

You can also configure further formatters, loggers, variable formatters and other
nice things with the configuration API:

```
    static void AddSpecWriterFacility(WindsorContainer container) {
      container.AddFacility<SpecWriterFacility>(
        f => {
          f.UseLogger(new SpecWriterNLogger());
          // ... etc
        });
    }
	
	// snippet from SpecWriterFacility, configurable options:
	
	public void UseLogger(Logger aLogger) {
      logger = aLogger;
    }

    public void UseVariableFormatter<T>()
      where T : VariableFormatter {
      variableFormatter = typeof (T);
    }

    public void UseFormatter<T>(string key) 
      where T : Formatter {
      formatters.Add(key, typeof(T));
    }

    public void UseContentWriter<T>()
      where T : ContentWriter {
      contentWriter = typeof (T);
    }

    public void UseRecordKeeper<T>() 
      where T : RecordDataKeeper {
      recordDataKeeper = typeof (T);
    }

    public void UseSerializer<T>()
      where T : Serializer {
      serializer = typeof (T);
    }

    public void UsePrintContextFactory<T>()
      where T : PrintContextFactory {
      printContextFac = typeof (T);
    }
```

The methods are documented, so use Intellisense to explore what options you have available.

The facility will place interceptors around components marked with `[RecordInteractions]`:

## SpecWriters

You can provide a SpecWriter implementation. Here's a unit test with MSpecWriter that
also showcases the two major controlling interfaces for the functionality, `RecordingControlPanel`
and `RecordDataKeeper`:

```csharp
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

```

## Controlling the Component

To control the component you can use  `RecordingControlPanel` and `RecordDataKeeper`,
as you saw above. These are registered in the container, and you can optionally create
replace them, or create and resolve from a child container to override them. The keys
of the components are visible in the facility source code (on the type `SpecWriterFacility`).

### A simple Web GUI

We have included a simple GUI that covered our use-cases.

![A simple SpecWriter GUI](https://raw.github.com/Prover/Prover.SpecWriter/master/doc/ScreenShot-Recording.png "This is what it looks like when it's running.")
