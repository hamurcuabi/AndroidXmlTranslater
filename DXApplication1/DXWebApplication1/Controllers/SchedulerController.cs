using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DXWebApplication1.Model;

namespace DXWebApplication1.Controllers {
    public class SchedulerController : BaseController {
        public ActionResult Index() {
            return View(SchedulerHelper.GetSchedulerDataSource(GetSelectedResourcesIds()));
        }
        public ActionResult SchedulerPartial() {
            return PartialView("SchedulerPartial", SchedulerHelper.GetSchedulerDataSource(GetSelectedResourcesIds()));
        }
        [ValidateAntiForgeryToken]
        public ActionResult SchedulerPartialEditAppointment(SchedulerAppointment appointment) {
            SafeExecute(() => SchedulerHelper.UpdateSchedulerDataSource());
            return SchedulerPartial();
        }

        private List<long> GetSelectedResourcesIds() {
            var selectedResourcesIds = SafeExecute(() => GetSelectedResourcesFromRequest());
            if(selectedResourcesIds != null)
                return selectedResourcesIds as List<long>;
            return null;
        }

        private List<long> GetSelectedResourcesFromRequest() {
            if(Request.Params.AllKeys.Contains("SelectedResources")) {
                string selectedResourcesRequest = Request.Params["SelectedResources"];
                return selectedResourcesRequest.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt64(id)).ToList();
            }
            return null;
        }
    }
}