using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Prover.SpecWriter.Naming;
using Prover.SpecWriter.Serializers;
using Prover.SpecWriter.SpecLevel;

namespace Prover.SpecWriter.Web {
  public class SpecWriterHttpApi
    : IHttpHandler {
    public void ProcessRequest(HttpContext context) {
      var accessor = context.ApplicationInstance as SpecWriterAccessor;
      if (accessor == null) {
        context.Response.Write("Please add Prover.SpecWriter.Web.SpecWriterAccessor to your application instance");
        context.Response.StatusCode = 500;
        return;
      }

      if (MatchesTarget(context, accessor))
        return;
      
      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Prover.SpecWriter.Web.index.htm"))
      using (var reader = new StreamReader(stream))
        context.Response.Write(reader.ReadToEnd());
    }

    bool MatchesTarget(HttpContext context, SpecWriterAccessor accessor) {
      var target = context.Request["target"] ?? "";
      switch (target) {
        case "":
          return false;
        case "status-submit":
          HandleStatus(context, accessor);
          return true;
        case "start-recording":
          HandleStartRecording(context, accessor);
          return true;
        case "stop-recording":
          HandleStopRecording(context, accessor);
          return true;
        case "clear-recording":
          HandleClearRecording(context, accessor);
          return true;
        case "render-spec":
          HandleRenderSpec(context, accessor);
          return true;
      }
      return false;
    }

    void HandleRenderSpec(HttpContext context, SpecWriterAccessor accessor) {
      var dk = accessor.DataKeeper;
      var called = dk.GetCalledMethods().ToList();
      
      if (called.Count == 0) {
        context.Response.Write(JsonConvert.SerializeObject(new {
          code    = "// Currently no code. Why not call some methods?",
          hasCode = false
        }));
        return;
      }

      var type = called.First().Target;

      var spec = accessor.WithWriter(cw => {
        var ctx = new SpecContextImpl(
          OrDefault(context, "Subject", "subject"),
          OrDefault(context, "BoostrapClassName", "Bootstrap"),
          OrDefault(context, "Namespace", "YourNamespace.Here"),
          OrDefault(context, "Title", "No special title").Replace(" ", "_"),
          called,
          type,
          s => new SimpleNameEnumerator(s),
          typeof(DataContractSerializer));
        return cw.WriteContext(ctx);
      });

      context.Response.Write(JsonConvert.SerializeObject(new {
        code = spec,
        hasCode = true
      }));
    }

    string OrDefault(HttpContext ctx, string key, string def) {
      var v = ctx.Request[key];
      return string.IsNullOrEmpty(v) ? def : v;
    }

    void HandleClearRecording(HttpContext context, SpecWriterAccessor accessor) {
      accessor.DataKeeper.Clear();
      var isStarted = accessor.Control.IsStarted();
      context.Response.Write(
        JsonConvert.SerializeObject(new {
          running = isStarted,
          status  = string.Format("Cleared recording. {0}", Status(isStarted)),
          count   = accessor.DataKeeper.CurrentCount()
        }));
    }

    void HandleStopRecording(HttpContext context, SpecWriterAccessor accessor) {
      accessor.Control.EndTrace();
      context.Response.Write(
        JsonConvert.SerializeObject(new {
          running = false,
          status = Status(false),
          count = accessor.DataKeeper.CurrentCount()
        }));
    }

    void HandleStartRecording(HttpContext context, SpecWriterAccessor accessor) {
      accessor.Control.StartTrace();
      context.Response.Write(
        JsonConvert.SerializeObject(new {
          running = true,
          status = Status(true),
          count = accessor.DataKeeper.CurrentCount()
        }));
    }

    void HandleStatus(HttpContext context, SpecWriterAccessor accessor) {
      var running = accessor.Control.IsStarted();
      context.Response.Write(
        JsonConvert.SerializeObject(new {
          running,
          status = Status(running),
          count  = accessor.DataKeeper.CurrentCount()
        }));
    }

    static string Status(bool running) {
      return running ? "Currently RUNNING" : "Currently NOT RUNNING";
    }

    public bool IsReusable { get { return true; } }
  }
}