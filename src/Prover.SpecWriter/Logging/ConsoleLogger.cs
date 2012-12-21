using System;

namespace Prover.SpecWriter.Logging {
  public class ConsoleLogger 
    : Logger {
    public void Debug(string message) {
      Console.WriteLine(message);
    }
  }
}