using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tracer
{
    public class MethodInfo
    {
        public string name;
        public string className;
        public string time;
        public List<MethodInfo> methods = new List<MethodInfo>();
    }
    public class ThreadInfo
    {
        public string time;
        public List<MethodInfo> methods = new List<MethodInfo>();
    }
    public class TraceResult
    {
        public ConcurrentDictionary<string, ThreadInfo> threads = 
            new ConcurrentDictionary<string, ThreadInfo>();
    }
}
