using System;

namespace Prover.SpecWriter {

  /// <summary>
  /// Place this attribute on the *components* you want recorded;
  /// NOT the services. If you put it on a service then it won't work.
  /// A service if this:
  /// <code>Component.For{{TService}}().ImplementedBy{{TComponent}}()</code> and 
  /// 
  /// also this is true:
  /// <code>Component.For{{TService and TComponent}}()</code>, i.e. a service that
  /// is its own component is fine to record.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class RecordInteractionsAttribute
    : Attribute {
  }
}