using SwapYE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwapYE.ViewModels
{
    public class chat
    {
        public List<Message> msg { get; set; }
        public List<Chat> chats { get; set; }
    }
}