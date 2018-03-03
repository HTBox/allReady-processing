using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AllReady.Processing
{
    public static class QueueTest
    {
        // This function can be used locally to test interaction with your 
        // local storage emulator.

        [FunctionName("QueueTest")]
        public static void Run([QueueTrigger("queue-test")]string item, TraceWriter log)
        {            
            log.Info($"A message was dequeued from the queue-test queue: {item}");
        }
    }
}
