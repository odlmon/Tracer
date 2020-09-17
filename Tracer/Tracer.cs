using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    class TracedItem
    {
        public Stopwatch stopwatch = new Stopwatch();
        public string Name { get; set; }

        public MethodInfo methodInfo;
    }

    public class TracerImpl: ITracer
    {
        private StackTrace stackTrace; 
        private ConcurrentDictionary<int, Stack<TracedItem>> tracedItems = 
            new ConcurrentDictionary<int, Stack<TracedItem>>();
        private TraceResult traceResult = new TraceResult();

        public void StartTrace()
        {
            //check thread
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            // if (!traceResult.threads.Any(item => item.Key == threadId.ToString())) 
            // { //create with current if not exist yet
            //     traceResult.threads.TryAdd(threadId.ToString(), new ThreadInfo
            //     {
            //         time = "0ms"
            //     });
            // }
            traceResult.threads.GetOrAdd(threadId.ToString(), new ThreadInfo
            {
                time = "0ms"
            });

            Stack<TracedItem> stack = tracedItems.GetOrAdd(threadId, new Stack<TracedItem>());
 
            TracedItem tracedItem = new TracedItem();
            stackTrace = new StackTrace();
            tracedItem.Name = stackTrace.GetFrame(1).GetMethod().Name;

            MethodInfo preparedItem = new MethodInfo()
            {
                name = tracedItem.Name,
                className = stackTrace.GetFrame(1).GetMethod().DeclaringType.Name
            };

            tracedItem.methodInfo = preparedItem;

            if (stack.Count != 0)
            {
                MethodInfo father = stack.Peek().methodInfo;
                father.methods.Add(preparedItem);
            } else
            {
                ThreadInfo threadInfo;
                if (!traceResult.threads.TryGetValue(
                    threadId.ToString(), out threadInfo))
                {
                    Console.WriteLine("went wrong");
                } else
                {
                    threadInfo.methods.Add(preparedItem);
                }
            }
            stack.Push(tracedItem);
            tracedItem.stopwatch.Restart();
        }

        public void StopTrace()
        {
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Stack<TracedItem> stack;
            tracedItems.TryGetValue(threadId, out stack);

            TracedItem tracedItem = stack.Pop();
            tracedItem.stopwatch.Stop();

            TimeSpan ts = tracedItem.stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}ms", ts.Milliseconds);
            tracedItem.methodInfo.time = elapsedTime;

            ThreadInfo threadInfo;
            if (!traceResult.threads.TryGetValue(
                threadId.ToString(), out threadInfo))
            {
                Console.WriteLine("went wrong");
            }
            else
            {
                string timeStr = threadInfo.time;
                int time = Int32.Parse(timeStr.Substring(0, timeStr.Length - 2));
                threadInfo.time = (time + ts.Milliseconds).ToString() + "ms";
            }

            Console.WriteLine("RunTime of {0}: {1}", tracedItem.Name, elapsedTime);
        }

        public TraceResult GetTraceResult()
        {
            return traceResult;
        }
    }
}
