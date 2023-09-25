using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Receipt_Printing_New
{
    public partial class ucModule1 : UserControl
    {
        public static int modulecurrent = 1;
        public static string restaurantid = "0";
       
        
        public ucModule1(string restid )
        {
            restaurantid = restid;
            InitializeComponent();
            int index = 0;
            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
            string printernamedefault = "";
            foreach (var printer in printerQuery.Get())
            {
                var name = printer.GetPropertyValue("Name");
                var status = printer.GetPropertyValue("Status");
                var isDefault = (bool)printer.GetPropertyValue("Default");
                if (isDefault == true)
                    printernamedefault = name.ToString();
                var isNetworkPrinter = printer.GetPropertyValue("Network");
                cbprinter.Items.Insert(index, name);
                index += 1;
            }
            if (printernamedefault != "")
                cbprinter.Text = printernamedefault;
            index = 0;
            for (int i = 1; i < 10; i++)
            {
                cbcopy.Items.Insert(index, i);
                index++;
            }
            PrinterSetting form1 = new PrinterSetting(restaurantid);
            LoadFullReceipt(form1.GetFullReceipt());
            //printerSetting.LoadFullReceipt();
            setDefault();
        }
        public void LoadFullReceipt(DataTable dt)
        {
            
            int index = 0;
            ItemCategory category;
            foreach (DataRow row in dt.Rows)
            {

                category = new ItemCategory();
                category.stext = row["NAME"].ToString();
                category.svalue = row["id"].ToString();
                this.cklstboxfullreceipt.Items.Add(category);
            }
            cklstboxfullreceipt.DisplayMember = "stext";




        }
        public void setDefault()
        {
            this.lstboxsize.SelectedIndex = 1;            
            this.cbcopy.SelectedIndex = 0;
        }
        public void setFullReceiptDefault()
        {
            
            for (int i = 0; i < cklstboxfullreceipt.Items.Count; i++)
            {
                cklstboxfullreceipt.SetItemChecked(i, cbfullreceipt.Checked);

            }
        }
        public void setButtonName(int i)
        {
            this.button1.Name = "button" + i.ToString();
        }
        public void setMapping(int i)
        {
            groupsetting.Text = "Mapping " + i.ToString();

        }
        public string getMapping()
        {
            return groupsetting.Text; 
        }
        public void setHideDelete(int i)
        {
            if (i == 0)
                button1.Visible = false;
        }
        public void GetParameter()
        {
            foreach (object itemChecked in cklstboxparameter.CheckedItems)
            {
                DataRowView castedItem = itemChecked as DataRowView;
            }
        }
        public void setModuleCurrent(int i)
        {
            modulecurrent = i;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Do you want to to delete this mapping?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result== DialogResult.Yes)
            {
                Form1 form1 = new Form1();
                var bt = (Button)sender;
                var panelContainer = this.Parent as Panel;
                //panelContainer.VerticalScroll.Value = 0;
                panelContainer.Controls.Remove(bt.Parent.Parent);
                int m = 1;
                foreach (Control c in panelContainer.Controls)
                {
                    if (m == 1)
                        panelContainer.VerticalScroll.Value = 0;
                    c.Location = new Point(0, (m - 1) * form1.GetModuleHeight() + (m - 1) * 100);
                    var g = ((GroupBox)c.Controls.Find("groupsetting", true)[0]);
                    g.Text = "Mapping " + (m).ToString();
                    c.Update();
                    m++;
                }
                panelContainer.Refresh();
            }
          
            //PrinterSetting printerSetting = new PrinterSetting();
            //printerSetting.resetPointMapping();
            //printerSetting.Controls.Remove(this)
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cbfullreceipt_CheckedChanged(object sender, EventArgs e)
        {
            Form1 form1 = new Form1("true");
            var ck = (CheckBox)sender;
            var panelContainer = this.Parent as Panel;
            //panelContainer.VerticalScroll.Value = 0;
            var module = (UserControl)ck.Parent.Parent;
            var cklstboxfullreceipt = ((CheckedListBox)module.Controls.Find("cklstboxfullreceipt", true)[0]);
            if(ck.Checked==true)
            {
                for (int i = 0; i < cklstboxfullreceipt.Items.Count; i++)
                {
                    cklstboxfullreceipt.SetItemChecked(i, false);

                }
            }
            
        }
        private void cbfullreceipt_Click(object sender, EventArgs e)
        {
            //Form1 form1 = new Form1();
            //var ck = (CheckBox)sender;
            //var panelContainer = this.Parent as Panel;
            ////panelContainer.VerticalScroll.Value = 0;
            //var module = (UserControl)ck.Parent.Parent;
            //var cklstboxfullreceipt = ((CheckedListBox)module.Controls.Find("cklstboxfullreceipt", true)[0]);
            //for (int i = 0; i < cklstboxfullreceipt.Items.Count; i++)
            //{
            //    cklstboxfullreceipt.SetItemChecked(i, ck.Checked);

            //}
        }
        private void cklstboxfullreceipt_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1 form1 = new Form1("true");
            var ck = (CheckedListBox)sender;
            var panelContainer = this.Parent as Panel;
            //panelContainer.VerticalScroll.Value = 0;
            var module = (UserControl)ck.Parent.Parent;
            var cbfullreceipt = ((CheckBox)module.Controls.Find("cbfullreceipt", true)[0]);
           if(ck.CheckedItems.Count > 0)
            {
                cbfullreceipt.Checked = false;
                for(int i =0; i< ck.CheckedItems.Count;i++)
                {
                    int index = ck.CheckedIndices[i];
                    ck.SetItemChecked(index, true);
                }
            }
               
            //if (ck.CheckedItems.Count != ck.Items.Count)
            //    cbfullreceipt.Checked = false;
            //else
            //    cbfullreceipt.Checked = true;


        }

    }
    public class ItemCategory
    {
        public string stext { get; set; }
        public string svalue { get; set; }
    }
}
