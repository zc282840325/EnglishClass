using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishClass.Models
{
    public class MessageModel
    {
        public MessageModel()
        {
        }

        public MessageModel(string name, DateTime time, string content, string state)
        {
            Name = name;
            Time = time;
            Content = content;
            State = state;
        }

        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public string State { get; set; }
    }
}