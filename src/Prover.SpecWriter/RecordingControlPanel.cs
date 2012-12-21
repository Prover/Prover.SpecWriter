using System;

namespace Prover.SpecWriter {
  /// <summary>
  /// This is the public control panel that lets you control the
  /// SpecWriter framework. You can call StartTrace to start tracing, and
  /// StopTrace to stop it. If you call Replay, then the same instance that was recorded
  /// will be animated again. Whether this supports your own invariants (e.g. the instance
  /// was transient and has been disposed), is up to yourself as a programmer.
  /// </summary>
  public interface RecordingControlPanel {

    /// <summary>
    /// Gets whether the control panel has started the recording.
    /// </summary>
    /// <returns></returns>
    bool IsStarted();

    /// <summary>
    /// Start tracing. This will trace all interactions to the object that has
    /// <see cref="RecordInteractionsAttribute"/> applied. If you have a multi-tennant or
    /// multi-user application and you only wish to trace parts of it, I recommend that you
    /// selectively apply the given attribute - e.g. by delegating from a new class to 
    /// a 'common' class that you are interested in tracing, and then creating a 
    /// component selector that gives the traced new class as a returned component
    /// for certain parameters (e.g. user id).
    /// </summary>
    void StartTrace();

    /// <summary>
    /// Stops the tracing of the components.
    /// </summary>
    void EndTrace();

    /// <summary>
    /// Run the passed action inside a trace, i.e. start the trace
    /// before the action is executed and then execute the action
    /// and then top the tracing.
    /// </summary>
    void Trace(Action action);
  }
}