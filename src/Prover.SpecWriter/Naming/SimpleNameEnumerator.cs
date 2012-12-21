namespace Prover.SpecWriter.Naming {
  public class SimpleNameEnumerator
    : NameEnumerator {
    readonly string nameBase;

    public SimpleNameEnumerator(string nameBase) {
      this.nameBase = nameBase;
    }

    int curr = -1;

    public string Next() {
      curr++;
      return curr == 0 ? nameBase : nameBase + curr.ToString();
    }
  }
}