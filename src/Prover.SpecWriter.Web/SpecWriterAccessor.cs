using System;

namespace Prover.SpecWriter.Web {
  public interface SpecWriterAccessor {
    RecordDataKeeper DataKeeper { get; }
    RecordingControlPanel Control { get; }

    T WithWriter<T>(Func<ContentWriter, T> callback);
  }
}