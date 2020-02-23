using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XmlTranslaterWeb.Models
{
   
        public class UploadFileModel
        {
            public UploadFileModel()
            {
              
            }
            public HttpPostedFileBase File{ get; set; }
            public string lang { get; set; }
            // Rest of model details
        }
    
}