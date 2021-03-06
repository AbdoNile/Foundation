﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Web.Paging;
using Kafala.Web.ViewModels.Reports.Partials;

namespace Kafala.Web.ViewModels.Reports
{
    public class PaymentStatusReportViewModel 
    {
        public FilterPaymentStatus FilterPaymentStatus { get; set; }
        public decimal ExpectedAmount { get; set; }

        public int CollectedAmount { get; set; }

        public int OverDueAmount { get; set; }
        
        public IEnumerable<OverDuePaymentViewModel> OutStandingPayments { get; set; }

    }
}
