﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.FormBuilder.CustomAttribute;
using Kafala.BusinessManagers.Payment;

namespace Kafala.Web.ViewModels.PaymentPeriod
{
    public class EditPaymentPeriodViewModel 
    {
        [EditControl(ElementType = ElementType.Hidden)]
        public virtual Guid Id { get; set; }
        
        [EditControl(ElementType = ElementType.Text)]
        public virtual string Name { get; set; }

        [EditControl(ElementType = ElementType.WholeNumber)]
        public virtual int Year { get; set; }

        [EditControl(ElementType = ElementType.WholeNumber)]
        public virtual int Month { get; set; }
    }
}
