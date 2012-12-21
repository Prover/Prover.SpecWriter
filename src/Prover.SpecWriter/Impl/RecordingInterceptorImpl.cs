using System;
using System.Collections.Generic;
using System.Threading;
using Castle.DynamicProxy;

namespace Prover.SpecWriter.Impl {
  /// <summary>
  /// This is a thread-safe recording interceptor. This means that the methods
  /// <see cref="RecordingControlPanel.StartTrace"/>,
  /// <see cref="RecordingControlPanel.EndTrace"/> 
  /// and <see cref="RecordingControlPanel.Replay"/>
  /// are safe to call from re-entrant or concurrent code. Beware
  /// that most likely, the code you are interecepting is NOT thread-safe.
  /// </summary>
  public class RecordingInterceptorImpl
    : IInterceptor {
    readonly RecordDataKeeper recorder;
    readonly RecordingControlPanel recordingControlPanel;

    public RecordingInterceptorImpl(
      RecordDataKeeper recorder,
      RecordingControlPanel recordingControlPanel) {
      if (recorder == null) throw new ArgumentNullException("recorder");
      if (recordingControlPanel == null) throw new ArgumentNullException("recordingControlPanel");
      this.recorder = recorder;
      this.recordingControlPanel = recordingControlPanel;
    }

    void IInterceptor.Intercept(IInvocation invocation) {
      if (recordingControlPanel.IsStarted())
        recorder.AddCall(new CalledMethodInfo(invocation));
      invocation.Proceed();
    }

    public RecordDataKeeper Recorder {
      get { return recorder; }
    }

    public RecordingControlPanel RecordingControlPanel {
      get { return recordingControlPanel; }
    }
  }
}