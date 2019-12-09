using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndroidXmlTranslater
{
    public partial class LoadingForm : Form
    {
        public Action Worker { get; set; }
        public LoadingForm(Action worker)
        {
            InitializeComponent();
            if (worker == null) throw new ArgumentNullException();
            Worker = worker;
            timer1.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Task.Factory.StartNew(Worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = HomeForm.Rate;
            lblMsg.Text = "%" + progressBar1.Value;
        }
    }
}
