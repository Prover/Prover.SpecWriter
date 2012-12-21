using System;

namespace Prover.SpecWriter.Impl {
  public interface Cache<TKey, TValue> {
    /// <summary>
    /// Find an item from the cache.
    /// </summary>
    /// <param name="key">Non-null key to look for</param>
    /// <param name="missingKeyFactory">
    /// A factory method that inserts the
    /// value into the cache, if called.
    /// </param>
    /// <returns>
    /// The value from the cache, or from the factory
    /// method if the key yet didn't have a value.
    /// </returns>
    TValue Get(TKey key, Func<TValue> missingKeyFactory);
  }
}