using Newtonsoft.Json;
using System;
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
            for (int i = 0; i < 1000000000; i++) { }
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

            Console.WriteLine(new JsonSerializer().Serialize(tracer.GetTraceResult()));
            Console.WriteLine(new XmlSerializer().Serialize(tracer.GetTraceResult()));
        }
    }
}
