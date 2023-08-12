using SwapYE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwapYE.ViewModels
{
    public class ItemCommNotif
    {
        public Item item { get; set; }
        public Comment Comment { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public ReportComment reportComment { get; set; }
    }
}