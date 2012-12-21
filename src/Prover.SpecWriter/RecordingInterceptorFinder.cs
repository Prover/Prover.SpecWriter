using System.Linq;
using Castle.DynamicProxy;
using Prover.SpecWriter.Impl;

namespace Prover.SpecWriter {
  /// <summary>
  /// Use this static class to find the interceptors (if any) attached to the
  /// instance.
  /// </summary>
  public static class RecordingInterceptorFinder {
    /// <summary>
    /// Get the interceptor for the instance.
    /// </summary>
    /// <typeparam name="T">Type of service or component (doesn't matter much)</typeparam>
    /// <param name="service"></param>
    /// <param name="recorder"></param>
    /// <param name="control"></param>
    /// <returns>true if the recording interceptor was attached, false otherwise</returns>
    public static bool InterceptorsFor<T>(
      T service,
      out RecordDataKeeper recorder,
      out RecordingControlPanel control) {
      var accessor = service as IProxyTargetAccessor;
      if (accessor == null) {
        recorder = null;
        control = null;
        return false;
      }

      var interceptor = accessor.GetInterceptors().OfType<RecordingInterceptorImpl>().FirstOrDefault();
      if (interceptor == null) {
        recorder = null;
        control = null;
        return false;
      }

      recorder = interceptor.Recorder;
      control = interceptor.RecordingControlPanel;
      return true;
    }
  }
}