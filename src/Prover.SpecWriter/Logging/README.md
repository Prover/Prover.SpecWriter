# Logging

You should create a custom logger, like such:

```csharp
using NLog;
using Logger = Prover.SpecWriter.Logger;
public class SpecWriterNLogger
	: Logger {
	static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();
	public void Debug(string message) {
		_logger.Log(typeof(SpecWriterNLogger), new LogEventInfo(LogLevel.Debug, "Prover.SpecWriter", message));
	}
}
```