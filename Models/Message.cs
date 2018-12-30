using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace callin.Models
{
    class Message
    {
        public string UserId { get; set; }
        public string Text { get; set; }

        public Message(string userid, string text)
        {
            this.UserId = userid;
            this.Text = text;
        }
    }
}
