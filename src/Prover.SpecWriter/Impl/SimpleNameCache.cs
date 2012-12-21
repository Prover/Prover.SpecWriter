using Prover.SpecWriter.Naming;
using Prover.SpecWriter.SpecLevel;

namespace Prover.SpecWriter.Impl {
  /// <summary>
  /// A class that simply encapsulates the pattern of getting a
  /// name from a dictionary cache, similar to how <see cref="SpecContextImpl"/>
  /// does it.
  /// </summary>
  public class SimpleNameCache {
    readonly Cache<string, NameEnumerator> cache =  new DictionaryCache<string, NameEnumerator>();
    public NameEnumerator Get(string name) {
      return cache.Get(name, () => new SimpleNameEnumerator(name));
    }
  }
}