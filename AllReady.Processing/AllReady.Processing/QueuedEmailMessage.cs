namespace AllReady.Processing
{

    public class QueuedEmailMessage
    {

        public string Recipient { get; set; }

        public string Message { get; set; }

        public string HtmlMessage { get; set; }

        public string Subject { get; set; }
    }
}
