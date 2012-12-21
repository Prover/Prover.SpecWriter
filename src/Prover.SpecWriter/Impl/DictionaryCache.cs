using System;
using System.Collections.Generic;

namespace Prover.SpecWriter.Impl {
  /// <summary>
  /// Dictionary-backed cache.
  /// </summary>
  public class DictionaryCache<TKey, TValue>
    : Cache<TKey, TValue>, Lookup<TKey, TValue> {
    readonly Dictionary<TKey, TValue> backing = new Dictionary<TKey, TValue>();

    TValue Cache<TKey, TValue>.Get(TKey key, Func<TValue> missingKeyFactory) {
      if (ReferenceEquals(key, null)) throw new ArgumentNullException("key");

      TValue found;
      if (backing.TryGetValue(key, out found))
        return found;

      var created = missingKeyFactory();
      backing[key] = created;
      return created;
    }

    TValue Lookup<TKey, TValue>.Get(TKey key) {
      return backing[key];
    }
  }
}