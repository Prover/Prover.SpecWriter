namespace Prover.SpecWriter.Impl {
  public interface Lookup<TKey, TValue> {
    TValue Get(TKey key);
  }
}