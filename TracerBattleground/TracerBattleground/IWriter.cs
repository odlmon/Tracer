using System.IO;
using Tracer;

namespace TracerBattleground
{
    public interface IWriter
    {
        void Write(ISerializer serializer, TraceResult traceResult);
    }
}