using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Naming;
using Prover.SpecWriter.Formatters;
using Prover.SpecWriter.Impl;
using Prover.SpecWriter.Logging;
using Prover.SpecWriter.Naming;
using Prover.SpecWriter.Serializers;
using Prover.SpecWriter.SpecWriters;
using Prover.SpecWriter.StatementLevel;
using Prover.SpecWriter.VariableFormatters;

namespace Prover.SpecWriter {
  /// <summary>
  ///   Bootstrapper for the integration test tracing infrastructure.
  ///   If you add this facility before you add a component, then all components
  ///   with the attribute <see cref="RecordInteractionsAttribute"/> will have their
  ///   interactions recorded.
  /// </summary>
  public class SpecWriterFacility
    : AbstractFacility {
    readonly Dictionary<string, Type> formatters = new Dictionary<string, Type>();
    readonly SimpleNameCache nameCache = new SimpleNameCache();

    Type recordDataKeeper = typeof (Recorder);
    Type contentWriter = typeof (MSpecWriter);
    Type variableFormatter = typeof(BinaryVarFormatter);
    Type serializer = typeof(BinarySerializer);
    Type printContextFac = typeof (DefaultPrintContextFactory);

    Logger logger;

    /// <summary>This is the key of the interceptor that records interactions.</summary>
    public const string InterceptorKey = "specwriter-interceptor";

    public const string VariableFormatterKey = "specwriter-default-formatter";
    public const string SerializerKey = "specwriter-serializer";
    public const string ContentWriterKey = "specwriter-content-writer";
    public const string RecordingControlPanelKey = "specwriter-recording-control-panel";
    public const string RecordDataKeeperKey = "specwriter-record-data-keeper";
    public const string LoggerKey = "specwriter-logger";
    public const string PrintContextFactoryKey = "specwriter-print-context-factory";

    protected override void Init() {
      Kernel.Register(
        Component.For<RecordingInterceptorImpl>()
          .Named(InterceptorKey)
          .LifeStyle.Transient,

        Component.For<RecordDataKeeper>()
          .ImplementedBy(recordDataKeeper)
          .Named(RecordDataKeeperKey)
          .LifeStyle.Singleton,

        Component.For<RecordingControlPanel>()
          .ImplementedBy<RecordingControlPanelImpl>()
          .Named(RecordingControlPanelKey)
          .LifeStyle.Singleton,

        Component.For<Serializer>()
          .ImplementedBy(serializer)
          .Named(SerializerKey)
          .LifeStyle.Transient,
        
        Component.For<VariableFormatter>()
          .ImplementedBy(variableFormatter)
          .Named(VariableFormatterKey)
          .LifeStyle.Transient,

        Component.For<PrintContextFactory>()
          .ImplementedBy(printContextFac)
          .Named(PrintContextFactoryKey)
          .LifeStyle.Transient,

        Component.For<ContentWriter>()
          .ImplementedBy(contentWriter)
          .Named(ContentWriterKey)
          .LifeStyle.Transient,

        Component.For<Func<string, NameEnumerator>>()
          .UsingFactoryMethod<Func<string, NameEnumerator>>(
            () => variable => new SimpleNameEnumerator(variable), 
            true)
          .LifeStyle.Transient,

        Component.For<Logger>()
          .Named(LoggerKey)
          .Instance(logger ?? new ConsoleLogger()));

      // register chosen formatters
      Kernel.Register(
        formatters.Select(formatter =>
                          Component.For<Formatter>()
                                   .Forward(formatter.Value)
                                   .Named(formatter.Key) as IRegistration)
                  .ToArray());

      // register formatters form this assembly
      Kernel.Register(
        AllTypes.FromThisAssembly()
                .InSameNamespaceAs<SingleFormatter>()
                .WithServiceFirstInterface()
                .LifestyleTransient()
                .Configure(cr => cr.Named(ComputeComponentName(cr))));

      // register the IEnumerable that the spec writers or
      // print context factory asks for.
      Kernel.Register(
        Component.For<IEnumerable<Formatter>>()
                 .UsingFactoryMethod(k => k.ResolveAll<Formatter>(), true)
                 .OnDestroy(fs => {
                   foreach (var formatter in fs) Kernel.ReleaseComponent(formatter);
                 }));

      Kernel.ComponentRegistered += Kernel_ComponentRegistered;

      InspectExistingComponents(Kernel);
    }

    string ComputeComponentName(ComponentRegistration cr) {
      var abbr = new string(cr.Implementation.Name.Where(char.IsUpper).ToArray());
      return "specwriter-" + nameCache.Get(abbr).Next();
    }

    void InspectExistingComponents(IKernel k) {
      var components = k.GetSubSystem(SubSystemConstants.NamingKey) as INamingSubSystem;
      foreach (var handler in components.GetAllHandlers()) {
        Kernel_ComponentRegistered(null, handler);
      }
    }

    void Kernel_ComponentRegistered(string key, IHandler handler) {
      if (handler.ComponentModel.Implementation.HasAttribute<RecordInteractionsAttribute>()) {
        handler.ComponentModel.Interceptors.Add(new InterceptorReference(InterceptorKey));
      }
    }

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
  }
}