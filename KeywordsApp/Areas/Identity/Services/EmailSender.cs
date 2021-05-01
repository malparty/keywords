namespace KeywordsApp.Areas.Identity.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string SendFromEmailAddress { get; set; }
    }
}