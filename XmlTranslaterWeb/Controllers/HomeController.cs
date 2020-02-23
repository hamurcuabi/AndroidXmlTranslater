using AndroidXmlTranslater;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using XmlTranslaterWeb.Models;

namespace XmlTranslaterWeb.Controllers
{
    
    public class HomeController : Controller
    {
        private CountryMockData mockData;
        private string newfilePath;
        private static int Rate = 0;
        private static int Count = 0;
        private static readonly HttpClient client = new HttpClient();


        private BindingList<XmlObject> xmlObjects = new BindingList<XmlObject>();
        public ActionResult Index()
        {
            ViewBag.Title = "Android Xml Translater";
             mockData = new CountryMockData();
            ViewBag.Countries = mockData.GetCountryList();
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase uploadFile, Country country)
        {
           
            try
            {
                var supportedTypes = new[] { "xml"};
                var fileExt = System.IO.Path.GetExtension(uploadFile.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    ViewBag.Msj = "File Extension is invalid - Only Upload XML File";
                    return View("Error");
                }
                else if (uploadFile.ContentLength > 1024*1024)
                {
                    ViewBag.Msj = "Max File Size 1 MB. Your file "+uploadFile.ContentLength+" KB";
                    return View("Error");
                }
                else
                {
                    string filePath = Path.Combine(Server.MapPath("~/FileUpload/"), Path.GetFileName(uploadFile.FileName));
                    newfilePath = Path.Combine(Server.MapPath("~/FileUpload/"), Path.GetFileName("Yeni.xml"));

                    uploadFile.SaveAs(filePath);
                    GetStrings(filePath);
                    await TranslateXmlAsync(country.code);
                    GenerateXml().Save(newfilePath);
                    ViewBag.Msj = "File Uploaded";
                    @ViewBag.Link = newfilePath;
                    return View(xmlObjects);


                }
            }
            catch (Exception ex)
            {
                ViewBag.Msj = "Upload Container Should Not Be Empty or Contact Admin."+ex.Message;
                return View("Error");
            }



        }

        public void GetStrings(String filePath)
        {

            xmlObjects = new BindingList<XmlObject>();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filePath);

                XmlNodeList items = xDoc.GetElementsByTagName("string");
                foreach (XmlNode xItem in items)
                {
                    XmlObject xml = new XmlObject();
                    xml.attr = xItem.Attributes["name"].Value;
                    xml.old_value = xItem.InnerText;
                    xmlObjects.Add(xml);
                    //  listBox1.Items.Add(xml.old_value);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }
        private async Task TranslateXmlAsync(String lang)
        {
            foreach (XmlObject item in xmlObjects)
            {
                String trText = await TranslateAsync(item.old_value, lang);
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Translation>(trText);
                item.new_value = response.text[0];
                Count += 1;
                Rate = ((Count * 100) / xmlObjects.Count);


            }


        }
        public XmlDocument GenerateXml()
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("resources");
            xmlDoc.AppendChild(rootNode);

            XmlNode userNode = xmlDoc.CreateElement("string");
            XmlAttribute attribute = xmlDoc.CreateAttribute("name");
            foreach (var item in xmlObjects)
            {
                userNode = xmlDoc.CreateElement("string");
                attribute = xmlDoc.CreateAttribute("name");
                attribute.Value = item.attr;
                userNode.Attributes.Append(attribute);
                userNode.InnerText = item.new_value;
                rootNode.AppendChild(userNode);
            }




            return xmlDoc;
            // doc.Save(Console.Out);
        }
        private void SaveXml(String filename)
        {
            GenerateXml().Save(filename);
        }

        private async Task<string> TranslateAsync(String text, String lang)
        {

            var values = new Dictionary<string, string>
                {
                { "text", text },
                { "lang", lang },
                { "key", "trnsl.1.1.20180719T155226Z.d2f3a41e023a4b8e.ddf417ba5c68359f7cde322cc0097c32eacec422" }
                };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://translate.yandex.net/api/v1.5/tr.json/translate", content);

            return await response.Content.ReadAsStringAsync();



        }
        public ActionResult Download(string filePath)
        {

            byte[] fileBytes = GetFile(filePath);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "string.xml");
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

    }
}