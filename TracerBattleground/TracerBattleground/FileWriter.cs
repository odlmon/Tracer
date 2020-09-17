using System.IO;
using Tracer;

namespace TracerBattleground
{
    public class FileWriter: IWriter
    {
        public void Write(ISerializer serializer, TraceResult traceResult)
        {
            using (StreamWriter sw = new StreamWriter("traceResult" + serializer.GetExtension()))
            {
                sw.WriteLine(serializer.Serialize(traceResult));
            }
        }
    }
}