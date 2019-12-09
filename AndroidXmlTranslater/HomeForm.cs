using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AndroidXmlTranslater
{
    public partial class HomeForm : Form
    {
        public static int Rate = 0;
        public static int Count = 0;
        public HomeForm()
        {
            InitializeComponent();
        }
        private BindingList<XmlObject> xmlObjects = new BindingList<XmlObject>();
        private void LoadXml()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "XML Dosyası |*.xml";
            if (file.ShowDialog() == DialogResult.OK)
            {
                string path = file.FileName;
                GetStrings(path);

            }
        }
        private void GetStrings(String filePath)
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
                    listBox1.Items.Add(xml.old_value);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }
        private async void TranslateXml()
        {
            foreach (XmlObject item in xmlObjects)
            {
                String trText = await Translate(item.old_value);
                item.new_value = trText;
                Count += 1;
                Rate = ((Count * 100) / xmlObjects.Count);


            }

        }
        private async Task<String> Translate(String text)
        {
            using (var wb = new WebClient())
            {
                var reqData = new NameValueCollection();
                reqData["text"] = text; // text to translate
                reqData["lang"] = txtLang.Text; // target language
                reqData["key"] = "trnsl.1.1.20180719T155226Z.d2f3a41e023a4b8e.ddf417ba5c68359f7cde322cc0097c32eacec422";

                try
                {
                    Task<String> islem = Task.Run<String>(() =>
                     {
                         var response = wb.UploadValues("https://translate.yandex.net/api/v1.5/tr.json/translate", "POST", reqData);
                         string responseInString = Encoding.UTF8.GetString(response);

                         var rootObject = JsonConvert.DeserializeObject<Translation>(responseInString);
                         //Console.WriteLine($"Original text: {reqData["text"]}\n" +
                         //    $"Translated text: {rootObject.text[0]}\n" +
                         //    $"Lang: {rootObject.lang}");

                         return rootObject.text[0];

                     });
                    return islem.Result;




                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR!!! " + ex.Message);
                    Task<String> islem = Task.Run<String>(() =>
                    {
                        return "Error";
                    });
                    return islem.Result;
                }

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

        private void SaveXml()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML Files|*.xml";
            save.OverwritePrompt = true;
            save.CreatePrompt = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
                GenerateXml().Save(save.FileName);
                MessageBox.Show("Succesfully Saved", "Succes", MessageBoxButtons.OK);

            }
        }

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            LoadXml();
        }

        private void checkButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtLang.Text))
            {
                if (MessageBox.Show(
         "Copy Code From Table then Paste into textbox", "Visit Web Site", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk
     ) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://tech.yandex.com/translate/doc/dg/concepts/api-overview-docpage/#api-overview__languages");
                }
                return;
            }
          else  if (xmlObjects.Count > 0)
            {
                using (LoadingForm frm = new LoadingForm(TranslateXml))
                {

                    frm.ShowDialog(this);
                }
                gridControl1.DataSource = xmlObjects;
            }
            else
            {

                MessageBox.Show("First Load Xml", "Warning", MessageBoxButtons.OK);
            }


        }

        private void checkButton3_CheckedChanged(object sender, EventArgs e)
        {
            SaveXml();

        }
    }
}
