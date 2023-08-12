using SwapYE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwapYE.ViewModels
{
    public class UserItem
    {
        public IEnumerable<Item> items { get; set; }
        public  User user { get; set; }
    }
}