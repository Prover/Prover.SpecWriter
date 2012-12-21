using System.Collections.Generic;

namespace Prover.SpecWriter {
  /// <summary>This is the object that keeps track of what methods have been called.</summary>
  public interface RecordDataKeeper {
    /// <summary>
    /// Gets all information about what methods were called during the trace.
    /// </summary>
    IEnumerable<CalledMethodInfo> GetCalledMethods();

    /// <summary>Adds a call to the list of calls made.</summary>
    void AddCall(CalledMethodInfo info);

    /// <summary>Replay all saved methods.</summary>
    void Replay();

    /// <summary>Clear all recorded methods.</summary>
    void Clear();

    /// <summary>Gets the current trace count</summary>
    long CurrentCount();
  }
}