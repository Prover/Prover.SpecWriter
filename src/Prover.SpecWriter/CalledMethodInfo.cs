using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Prover.SpecWriter {
  /// <summary>
  /// Immutable class that keeps track of calls. Beware; saves
  /// a reference to the actual target.
  /// </summary>
  [Serializable]
  public struct CalledMethodInfo {
    readonly object[] arguments;
    readonly Type[] genericArguments;
    readonly MethodInfo method;
    readonly Type target;
    readonly object targetInstance;

    public CalledMethodInfo(IInvocation invocation) {
      arguments = (object[]) invocation.Arguments.Clone();
      if (invocation.GenericArguments != null)
        genericArguments = (Type[]) invocation.GenericArguments.Clone();
      else
        genericArguments = null;
      method = invocation.MethodInvocationTarget;
      target = invocation.TargetType;
      targetInstance = invocation.InvocationTarget;
    }

    /// <summary>
    /// Gets the underlying target (of the proxy that intercepted and
    /// recorded this call)
    /// </summary>
    public Type Target {
      get { return target; }
    }

    /// <summary>
    /// Gets the invocation target instance.
    /// </summary>
    public object TargetInstance {
      get { return targetInstance; }
    }

    /// <summary>
    ///   Gets the actual argument values that the method was called with
    /// </summary>
    public object[] Arguments {
      get { return arguments; }
    }

    /// <summary>
    ///   Null if no generic arguments
    /// </summary>
    public Type[] GenericArguments {
      get { return genericArguments; }
    }

    /// <summary>
    /// Gets the method info that corresponds to the method that was
    /// called that created this call info.
    /// </summary>
    public MethodInfo Method {
      get { return method; }
    }
  }
}