using System.Threading;
using Tracer;

namespace TracerBattleground
{

    class A
    {
        private ITracer tracer;

        internal A(ITracer tracer)
        {
            this.tracer = tracer;
        }
        public void Test()
        {
            tracer.StartTrace();
            for (int i = 0; i < 40000000; i++)
            {
                int b = i + 2;
            }
            new B(tracer).Inner();
            tracer.StopTrace();
        }
        public void Another()
        {
            tracer.StartTrace();
            for (int i = 0; i < 100000000; i++) { }
            tracer.StopTrace();
        }
    }

    class B
    {
        private ITracer tracer;

        internal B(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void Inner()
        {
            tracer.StartTrace();
            for (int i = 0; i < 1000000; i++)
            {
                int a = i + 1;
            }
            tracer.StopTrace();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ITracer tracer = new TracerImpl();
            new A(tracer).Test();
            new A(tracer).Another();

            Thread thread = new Thread(() => new A(tracer).Another() );
            thread.Start();
            thread.Join();

            TraceResult traceResult = tracer.GetTraceResult();
            
            var consoleWriter = new ConsoleWriter();
            var fileWriter = new FileWriter();
            
            var jsonSerializer = new JsonSerializer();
            var xmlSerializer = new XmlSerializer();

            consoleWriter.Write(jsonSerializer, traceResult);
            consoleWriter.Write(xmlSerializer, traceResult);
            
            fileWriter.Write(jsonSerializer, traceResult);
            fileWriter.Write(xmlSerializer, traceResult);
        }
    }
}
