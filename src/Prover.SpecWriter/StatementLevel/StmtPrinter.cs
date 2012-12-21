using System;
using System.Linq;
using Prover.SpecWriter.Impl;

namespace Prover.SpecWriter.StatementLevel {
  public static class StmtPrinter {
    public static string Print(this PrintContext printContext) {
      printContext = WriteVars(printContext);
      printContext = WriteInvocation(printContext);
      return printContext.Builder.ToString();
    }

    static PrintContext WriteVars(PrintContext ctx) {
      var info = ctx.Info;
      var vars = new string[info.Arguments.Length];
      for (int index = 0; index < info.Arguments.Length; index++) {
        var arg = info.Arguments[index];
        if (arg == null) continue; // nulls are handled by NullFormatter
        if (arg is OwnFormatter) continue; // check if it can format itself
        if (ctx.CustomFormatters.Any(f => f.CanFormat(arg))) continue; // check with out formatters

        // get a new variable name (it might be taken already!)
        var methodParamName = ctx.ForBaseName(info.Method.GetParameters()[index].Name).Next();
        vars[index] = methodParamName;

        ctx.Builder.AppendLine(
          new string(' ', ctx.SpaceIndentation)
          + ctx.VariableFormatter.Format(arg, methodParamName));
      }
      return ctx.SetWrittenVars(vars);
    }

    static PrintContext WriteInvocation(PrintContext context) {
      var args = PrintArguments(context);
      var indent = new string(' ', context.SpaceIndentation);

      string dereferenceTarget = context.SubjectName;
      string methodName;
      string interfaceName;
      if (ExplicitInterfaceImpl(context, out methodName, out interfaceName))
        dereferenceTarget = string.Format("({0} as {1})", dereferenceTarget, interfaceName);

      context.Builder.AppendLine(
        indent + string.Format("{0}.{1}({2});",
                               dereferenceTarget,
                               methodName,
                               args));

      return context;
    }

    // when invoking explicitly implemented interfaces, chop off the prefix of that interface
    static bool ExplicitInterfaceImpl(PrintContext context, out string method, out string explicitInterface) {
      var m = context.Info.Method;
      var dotIndex = m.Name.LastIndexOf(".", StringComparison.InvariantCulture);

      method = dotIndex == -1
                 ? m.Name
                 : m.Name.Substring(dotIndex + 1, m.Name.Length - dotIndex - 1);

      explicitInterface = dotIndex == -1
                            ? ""
                            : m.Name.Substring(0, dotIndex);

      return dotIndex != -1;
    }

    static string PrintArguments(PrintContext ctx) {
      if (ctx.Info.Arguments.Length == 0)
        return "";

      return string.Join(
        ", ",
        ctx.Info.Arguments.Select(
          (x, i) => ctx.WrittenVars[i] != null 
            ? string.Format("{0}.Deserialize<{1}>({2})", 
                ctx.SerializerName, 
                x.GetType().FullName, 
                ctx.WrittenVars[i]) 
            : x.OwnFormatOr(y => ctx.AllFormatters.First(f => f.CanFormat(y)).Format(y))
          ).ToArray());
    }
  }
}