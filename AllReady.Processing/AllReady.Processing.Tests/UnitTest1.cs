using Moq;
using Xunit;
using Shouldly;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Diagnostics;

namespace AllReady.Processing.Tests
{
    public class UnitTest1
    {
        // comes from Environment Variable
        const string EmailSentFrom = "from@tests.com";

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var loggerMock = new MockTrace();
            var expected = new Mail { From = new Email(EmailSentFrom, "AllReady"), Subject = "test" };
            var personalization = new Personalization();
            personalization.AddTo(new Email("test@gmail.com"));
            expected.AddPersonalization(personalization);

            var incomingMessage = new QueuedEmailMessage { Subject = "test", Recipient = "test@gmail.com", Message = "simple text message" };
            // Act
            ProcessEmailQueueMessage.Run(@"{""Message"":""test"", ""Recepient"":""test@gmail.com"", ""Subject"":""test"" }", out Mail message, loggerMock);
            // Assert
            message.ShouldBe(expected);
        }

        // TODO tests
        // send a message that will not deserialize correctly
        // have a missing from

        [Theory]
        [InlineData("test")]
        public void TestMethod2(string input)
        {
            // Arrange
            var mock = new Mock<IMockable>();
            mock.Setup(i => i.Foo()).Returns("fooReturn");

            // Assert
            input.ShouldBe("test");
            input.ShouldNotBe("not test");

            Assert.Equal("test", input);
            mock.Object.Foo().ShouldBe("fooReturn");

        }

        [Fact]
        public void TestCiblingProject()
        {
            // Arrange
            var t = new QueuedEmailMessage { Subject = "Test" };

            // Assert
            t.Subject.ShouldBe("Test");
        }
        public interface IMockable
        {
            string Foo();
        }

        public class MockTrace : TraceWriter
        {
            public List<string> Infos = new List<string>();

            public MockTrace(): base(TraceLevel.Verbose)
            {
            }

            public override void Trace(TraceEvent traceEvent)
            {
                throw new System.NotImplementedException();
            }
        }

    }
}
