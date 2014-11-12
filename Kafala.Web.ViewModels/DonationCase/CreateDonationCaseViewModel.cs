﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Foundation.FormBuilder.CustomAttribute;
using Foundation.FormBuilder.DynamicForm;
using Kafala.BusinessManagers.DonationCase;
using Kafala.Entities.Enums;

namespace Kafala.Web.ViewModels.DonationCase
{
    public class CreateDonationCaseViewModel : IDonationCaseContract
    {
        [EditControl(ElementType = ElementType.Text)]
        [Required(ErrorMessage = "Please enter a name.")]
        public virtual string Name { get; set; }

        [EditControl(ElementType = ElementType.DateTime)]
        [Required(ErrorMessage = "Please enter a start date.")]
        public virtual DateTime? StartDate { get; set; }

        [EditControl(ElementType = ElementType.DateTime)]
        public virtual DateTime? EndDate { get; set; }

        [EditControl(ElementType = ElementType.Enum)]
        public virtual DonationCaseStatus DonationCaseStatus { get; set; }
    }
}
