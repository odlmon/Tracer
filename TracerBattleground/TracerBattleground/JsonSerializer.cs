using Tracer;
using Newtonsoft.Json;

namespace TracerBattleground
{
    public class JsonSerializer: ISerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            return JsonConvert.SerializeObject(traceResult, Formatting.Indented);
        }
    }
}