using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AllReady.Processing
{
    public static class QueueTest
    {
        // This function can be used locally to test interaction with your 
        // local storage emulator.

        [FunctionName("QueueTest")]
        public static void Run([QueueTrigger("queue-test")]string item, ILogger log)
        {            
            log.LogInformation($"A message was dequeued from the queue-test queue: {item}");
        }
    }
}
