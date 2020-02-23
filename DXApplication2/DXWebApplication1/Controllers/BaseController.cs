using System;
using System.Web.Mvc;

namespace DXWebApplication1.Controllers {
    public class BaseController : Controller {
        
        protected void SafeExecute(Action method) {
            try {
                method();
            } catch(Exception e) {
                SetErrorText(e.Message);
            }
        }
        protected object SafeExecute(Func<object> method) {
            try {
                return method();
            } catch(Exception e) {
                SetErrorText(e.Message);
            }
            return null;
        }
        protected void SetErrorText(string message) {
            ViewBag.GeneralError = message;
        }
    }
}