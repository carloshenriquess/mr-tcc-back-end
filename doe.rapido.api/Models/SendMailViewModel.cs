using System;
namespace doe.rapido.api.Models
{
    public class SendMailViewModel
    {
        public string[] Emails { get; set; } = new string[0];
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; }
    }
}
