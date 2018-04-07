using Moq;
using Xunit;
using Shouldly;
//using Microsoft.Azure.WebJobs.Host;
//using SendGrid.Helpers.Mail;

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
            //var loggerMock = new Mock<TraceWriter>();
            //loggerMock.Setup(l => l.Info(It.IsAny<string>(), It.IsAny<string>()));
            //var expected = new Mail { From = new Email(EmailSentFrom, "AllReady"), Subject = "test" };
            //var personalization = new Personalization();
            //personalization.AddTo(new Email("test@gmail.com"));
            //expected.AddPersonalization(personalization);

            //    var incomingMessage = new QueuedEmailMessage { Subject = "test", Recipient = "test@gmail.com", Message = "simple text message" };
            //    // Act
            //    ProcessEmailQueueMessage.Run("testItem", out Mail message, loggerMock.Object);
            //    // Assert
            //    message.ShouldBe(expected);
        }

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

    }
}
