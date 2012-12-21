using System;

namespace Prover.SpecWriter.Impl {
  public class RecordingControlPanelImpl
    : RecordingControlPanel {
    readonly Logger logger;

    volatile bool started;

    public RecordingControlPanelImpl(Logger logger) {
      if (logger == null) throw new ArgumentNullException("logger");
      this.logger = logger;
    }

    public bool IsStarted() {
      return started;
    }

    void RecordingControlPanel.StartTrace() {
      StartTrace();
    }

    void RecordingControlPanel.EndTrace() {
      EndTrace();
    }

    void RecordingControlPanel.Trace(Action action) {
      if (action == null) throw new ArgumentNullException("action");
      Trace(action);
    }

    void StartTrace() {
      logger.Debug("StartTrace");
      started = true;
    }

    void EndTrace() {
      started = false;
      logger.Debug("EndTrace");
    }

    void Trace(Action action) {
      try {
        StartTrace();
        action();
      }
      finally {
        EndTrace();
      }
    }
  }
}