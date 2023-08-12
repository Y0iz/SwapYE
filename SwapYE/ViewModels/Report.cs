using SwapYE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwapYE.ViewModels
{
    public class Report
    {
        public List<ReportItem> reitem { get; set; }
        public List<ReportComment> recomment { get; set; }
        public List<ReportChat> remsg { get; set; }
    }
}