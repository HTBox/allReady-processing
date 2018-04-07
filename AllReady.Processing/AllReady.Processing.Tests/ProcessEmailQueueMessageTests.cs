using Moq;
using Xunit;
using Shouldly;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace AllReady.Processing.Tests
{
    public class ProcessEmailQueueMessageTests
    {
        // comes from Environment Variable
        const string EmailSentFrom = "from@tests.com";
        const string FromEnvironmentVariable = "Authentication:SendGrid:FromEmail";

        [Fact]
        public void ShouldOutputMailWithPlainTextOnly()
        {
            // Arrange
            var loggerMock = new MockTrace();
            var messageInQueue = MessageInQueue("Subject test", "test@testing.com", "Message text plain");
            Environment.SetEnvironmentVariable(FromEnvironmentVariable, EmailSentFrom);
            // Act
            ProcessEmailQueueMessage.Run(messageInQueue, out Mail message, loggerMock);
            // Assert
            message.From.Name.ShouldBe("AllReady");
            message.From.Address.ShouldBe(EmailSentFrom);
            message.Subject.ShouldBe("Subject test");
            message.Contents[0].Type.ShouldBe("text/plain");
            message.Contents[0].Value.ShouldBe("Message text plain");
            message.Personalization[0].Tos[0].Address.ShouldBe("test@testing.com");
        }

        [Fact]
        public void ShouldOutputMailWithHtmlTextOnly()
        {
            // Arrange
            var loggerMock = new MockTrace();
            var messageInQueue = MessageInQueue("Subject test", "test@testing.com", null, "<span>text</span>");
            //Environment.SetEnvironmentVariable(FromEnvironmentVariable, EmailSentFrom);
            // Act
            ProcessEmailQueueMessage.Run(messageInQueue, out Mail message, loggerMock);
            // Assert
            message.From.Name.ShouldBe("AllReady");
            message.From.Address.ShouldBe(EmailSentFrom);
            message.Subject.ShouldBe("Subject test");
            message.Contents[0].Type.ShouldBe("text/html");
            message.Contents[0].Value.ShouldBe("<span>text</span>");
            message.Personalization[0].Tos[0].Address.ShouldBe("test@testing.com");
        }

        [Fact]
        public void ShouldTraceTheSubjectAndRecepient()
        {
            // Arrange
            var loggerMock = new MockTrace();
            var messageInQueue = MessageInQueue("Subject test", "test@testing.com", null, "<span>text</span>");
            Environment.SetEnvironmentVariable(FromEnvironmentVariable, EmailSentFrom);
            // Act
            ProcessEmailQueueMessage.Run(messageInQueue, out Mail message, loggerMock);
            // Assert
            loggerMock.Events.Count.ShouldBe(1);
            loggerMock.Events[0].Level.ShouldBe(TraceLevel.Info);
            loggerMock.Events[0].Message.ShouldBe("Sending email with subject `Subject test` to `test@testing.com`");
        }
        
        [Fact]
        public void ShouldThrowOnDeserialization()
        {
            // Arrange
            var loggerMock = new MockTrace();
            Environment.SetEnvironmentVariable(FromEnvironmentVariable, EmailSentFrom);
            // Act
            // Assert
            Should.Throw<Exception>(() => ProcessEmailQueueMessage.Run("invalid message", out Mail message, loggerMock));
        }

        [Fact]
        public void ShouldThrowForMissing_From()
        {
            // Arrange
            var loggerMock = new MockTrace();
            Environment.SetEnvironmentVariable(FromEnvironmentVariable, null);
            var messageInQueue = MessageInQueue("Subject test", "test@testing.com", null, "<span>text</span>");

            // Act
            // Assert
            Should.Throw<InvalidOperationException>(() => ProcessEmailQueueMessage.Run(messageInQueue, out Mail message, loggerMock));
        }

        public class MockTrace : TraceWriter
        {
            public List<TraceEvent> Events = new List<TraceEvent>();

            public MockTrace() : base(TraceLevel.Verbose)
            {
            }

            public override void Trace(TraceEvent traceEvent)
            {
                this.Events.Add(traceEvent);
            }
        }

        private string MessageInQueue(string subject, string recipient, string message = null, string htmlMessage = null)
        {
            return $"{{" +
                $"\"Recipient\":\"{recipient}\"," +
                $"\"Subject\":\"{subject}\"," +
                (message != null ? "\"Message\":\"" + message + "\"" : "") +
                (htmlMessage!= null ? "\"HtmlMessage\":\"" + htmlMessage + "\"" : "") +
                $"}}";
        }

    }
}


