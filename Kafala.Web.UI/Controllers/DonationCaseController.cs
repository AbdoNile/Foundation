﻿
using System;
using System.Web.Mvc;
using Foundation.Infrastructure.BL;
using Foundation.Infrastructure.Query;
using Foundation.Web;
using Foundation.Web.Paging;
using Kafala.BusinessManagers.DonationCase;
using Kafala.Query.DonationCase;
using Kafala.Web.ViewModels.DonationCase;

namespace Kafala.Web.UI.Controllers
{
    public class DonationCaseController : BaseController
    {
        private readonly IQueryContainer queryContainer;

        private readonly IBusinessManagerContainer businessManagerContainer;

        public DonationCaseController(IBusinessManagerContainer businessManagerContainer, IQueryContainer queryContainer)
        {
            this.businessManagerContainer = businessManagerContainer;
            this.queryContainer = queryContainer;
        }

        [RenderPagedView]
        public ActionResult Index(ListDonationCasesParameters parameters)
        {
            var container = this.queryContainer.Get<ListDonationCaseViewModelPopulator>();
            var model = container.Execute(parameters);
            return View("Index", model);
        }

        public ActionResult Create()
        {
            var container = this.queryContainer.Get<CreateDonationCaseViewModelPopulator>();
            var model = container.Execute(null);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(CreateDonationCaseViewModel model)
        {
            var manager = businessManagerContainer.Get<DonationCaseBusinessManager>();
            var donorId = manager.Add(model);
            return RedirectToAction("Details", new { id = donorId });
        }

      
        public ActionResult Details(Guid id)
        {
            var modelPopulator = this.queryContainer.Get<ViewDonationCaseViewModelPopulator>();
            var model = modelPopulator.Execute(id);
            return View("View", model);
        }

        public ActionResult Edit(Guid id)
        {
            var modelPopulator = this.queryContainer.Get<EditDonationCaseViewModelPopulator>();
            var model = modelPopulator.Execute(id);
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(EditDonationCaseViewModel model)
        {
            var manager = businessManagerContainer.Get<DonationCaseBusinessManager>();
            var donorId = manager.Update(model.Id, model);
            return RedirectToAction("Details", new { id = donorId });
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var manager = businessManagerContainer.Get<DonationCaseBusinessManager>(); 
            var result = manager.Delete(id);
            return RedirectToAction("Index");
        }
      
    }
}
