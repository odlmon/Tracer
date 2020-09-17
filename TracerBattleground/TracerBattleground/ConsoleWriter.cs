using System;
using System.IO;
using Tracer;

namespace TracerBattleground
{
    public class ConsoleWriter: IWriter
    {
        public void Write(ISerializer serializer, TraceResult traceResult)
        {
            Console.WriteLine(serializer.Serialize(traceResult));
        }
    }
}