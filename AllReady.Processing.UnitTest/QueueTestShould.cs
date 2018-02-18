using System.Diagnostics;
using Microsoft.Azure.WebJobs.Host;
using Moq;
using Xunit;

namespace AllReady.Processing.UnitTest
{
    public class QueueTestShould
    {
        [Fact]
        public void Log_that_the_queue_message_was_dequeued()
        {
            string item = "Test Item";
            var mockTraceWriter = new Mock<TraceWriter>(TraceLevel.Verbose);

            QueueTest.Run(item, mockTraceWriter.Object);


            mockTraceWriter.Verify(x => x.Trace(It.Is<TraceEvent>(e => e.Level == TraceLevel.Info && e.Message.Contains(item))), Times.Once);
        }
    }
}