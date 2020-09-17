using Tracer;

namespace TracerBattleground
{
    public interface ISerializer
    {
        string Serialize(TraceResult traceResult);
        string GetExtension();
    }
}