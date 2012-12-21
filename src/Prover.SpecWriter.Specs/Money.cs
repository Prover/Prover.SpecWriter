namespace Prover.SpecWriter.Specs {
  public class Money {
    public decimal Amount { get; set; }

    public override string ToString() {
      return string.Format("Money{{Amount: {0}}}", Amount);
    }
  }
}