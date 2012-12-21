using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Prover.SpecWriter.Specs.Framework {

  public static class WindsorContainerFactory {
    public static IWindsorContainer GetTracingContainer(params IRegistration[] registrations) {
      var container = new WindsorContainer();
      container.AddFacility<SpecWriterFacility>();
      if (registrations != null) container.Register(registrations);
      return container;
    }

    public static IWindsorContainer GetTracingContainer<TService, TComponent>(
      Action<SpecWriterFacility> configure, 
      out RecordDataKeeper dataKeeper, 
      out RecordingControlPanel control,
      out TService actor)
      where TService : class
      where TComponent : TService {
      var container = new WindsorContainer();
      container.AddFacility(configure);
      container.Register(Component.For<TService>().ImplementedBy<TComponent>());
      actor = container.Resolve<TService>();
      RecordingInterceptorFinder.InterceptorsFor(actor, out dataKeeper, out control).ShouldBeTrue();
      return container;
    }

    public static void Dispose(this IWindsorContainer container, object actor, RecordingControlPanel control, RecordDataKeeper dataKeeper) {
      container.Release(actor);
      container.Release(dataKeeper);
      container.Release(control);
      container.Dispose();
    }
  }
}