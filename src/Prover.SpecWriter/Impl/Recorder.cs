using System;
using System.Collections.Generic;
using System.Threading;

namespace Prover.SpecWriter.Impl {
  public class Recorder
    : RecordDataKeeper
  {
    readonly Logger logger;

    readonly ReaderWriterLockSlim sem
      = new ReaderWriterLockSlim();

    readonly LinkedList<CalledMethodInfo> calledMethods
      = new LinkedList<CalledMethodInfo>();

    public Recorder(Logger logger) {
      this.logger = logger;
    }

    IEnumerable<CalledMethodInfo> RecordDataKeeper.GetCalledMethods() {
      return Write( () => new LinkedList<CalledMethodInfo>(calledMethods) );
    }

    void RecordDataKeeper.AddCall(CalledMethodInfo info) {
      Write(() => calledMethods.AddLast(info));
    }

    void RecordDataKeeper.Replay() {
      Replay();
    }

    void Replay() {
      logger.Debug(string.Format("Replaying {0} methods", calledMethods.Count));
      Write(() => {
        foreach (var info in calledMethods)
          info.Method.Invoke(info.TargetInstance, info.Arguments);
      });
    }

    void RecordDataKeeper.Clear() {
      Write( () => calledMethods.Clear() );
    }

    long RecordDataKeeper.CurrentCount() {
      return Read(() => calledMethods.Count);
    }

    T Read<T>(Func<T> func) {
      sem.EnterReadLock();
      try { return func(); }
      finally { sem.ExitReadLock(); }
    }

    void Write(Action action) {
      sem.EnterWriteLock();
      try { action(); }
      finally { sem.ExitWriteLock(); }
    }

    T Write<T>(Func<T> func) {
      sem.EnterWriteLock();
      try { return func(); }
      finally { sem.ExitWriteLock(); }
    }
  }
}