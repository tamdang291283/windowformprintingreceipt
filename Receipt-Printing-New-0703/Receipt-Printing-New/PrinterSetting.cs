using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.Adapters;
using System.Configuration;

namespace Receipt_Printing_New
{
    public partial class PrinterSetting : Form
    {
        public static DataTable dt;
        public static SqlConnection conn;
        public static SqlDataAdapter adapter;
        public static string restaurantid = "0";
        public static DataSet ds;
        public PrinterSetting(string resid)
        {
            restaurantid = resid;
            InitializeComponent();
            LoadFullReceiptIntoTable();

        }
        public DataTable GetFullReceipt()
        {
            return dt;
        }
        public void LoadFullReceiptIntoTable()
        {
            string connectionstring = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            //conn = new SqlConnection("Data Source=46.17.94.187\\MSSQLSERVER2019;Initial Catalog=k9kondop_FO2008-2019;User ID=tam2008-2019; password=B!7ym84v;Pooling=true;Min Pool Size =5;Max Pool Size=150");
            conn = new SqlConnection(connectionstring);
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                ds = new DataSet();
            

                string SQL = @"
                            select distinct mc.id, mc.NAME, displayorder 
                            FROM  menucategories   mc  with(nolock) 
                                    INNER JOIN menuitems  mi  with(nolock) 
                                    ON mc.id = mi.idmenucategory
                            where mc.idbusinessdetail = {0} and  mi.idbusinessdetail = {0}
                                     and mi.hidedish <> 1
                            ORDER  BY mc.displayorder
                ";
                SQL = string.Format(SQL, restaurantid);

                adapter = new SqlDataAdapter(
                SQL, conn);
                adapter.Fill(ds);
                dt = new DataTable();
                dt = ds.Tables[0];
            }
            finally { 
                if(conn != null)
                {   
                    conn.Close();
                }
            }
            

            //this.cklstboxfullreceipt.DataSource = ds.Tables[0];

        }
        public static int moduleHeight = 197;
        private void PrinterSetting_Load(object sender, EventArgs e)
        {
            LoadPrinterSetting();
            //ucModule1 module = new ucModule1();

            //module.setMapping(1);
            //module.setHideDelete(0);
            //module.Dock = DockStyle.None;
            //panel1.Controls.Add(module);
            //moduleHeight = module.Height;
        }

        private void btAddNewMapping_Click(object sender, EventArgs e)
        {
            int numberModule = panel1.Controls.Count;
            ucModule1 module2 = new ucModule1(restaurantid);
            module2.setMapping(numberModule + 1);
            
            //module2.setModuleCurrent(numberModule + 1);
            //module2.setButtonName(numberModule + 1);
            module2.Dock = DockStyle.None;
            module2.Location = new Point(0, (numberModule * moduleHeight));
            panel1.Controls.Add(module2);
        }
        private void LoadPrinterSetting()
        {
            ucModule1 module;
            int numberModule = 0;
            if (File.Exists(System.IO.Path.GetFullPath("PrinterSettingLog.txt")))
            {
                using (StreamReader readtext = new StreamReader("PrinterSettingLog.txt"))
                {
                    while (true)
                    {
                        string readText = readtext.ReadLine();
                        if(string.IsNullOrEmpty(readText))
                        { return; }    
                        string[] data = readText.Split('|');
                        module = new ucModule1(restaurantid);
                        
                        string p = data[0];
                        if (!string.IsNullOrEmpty(p))
                        {
                            var cklstboxparameter = ((CheckedListBox)module.Controls.Find("cklstboxparameter", true)[0]);
                            for (int i = 0; i < cklstboxparameter.Items.Count; i++)
                            {
                                    string[] lst = p.Split(',');
                                    foreach (string s in lst)
                                    {
                                        if (cklstboxparameter.Items[i].ToString() == s)
                                        {
                                            cklstboxparameter.SetItemChecked(i, true);
                                        }
                                    }

                            }
                        }
                        string sreceipt = data[1];
                        if(!string.IsNullOrEmpty(sreceipt))
                        {
                            var cklstboxfullreceipt = ((CheckedListBox)module.Controls.Find("cklstboxfullreceipt", true)[0]);
                            for (int i = 0; i < cklstboxfullreceipt.Items.Count; i++)
                            {
                                    string[] lst = sreceipt.Split(',');
                                    foreach (string s in lst)
                                    {
                                        if (cklstboxfullreceipt.Items[i].ToString() == s)
                                        {
                                            cklstboxfullreceipt.SetItemChecked(i, true);
                                        }
                                    }

                            }
                        }
                        string sprinter = data[2];
                        if (!string.IsNullOrEmpty(sprinter))
                        {
                            var cbprinter = ((ComboBox)module.Controls.Find("cbprinter", true)[0]);
                            for(int i=0; i < cbprinter.Items.Count; i++)
                            {
                                if (cbprinter.Items[i].ToString() == sprinter)
                                {
                                    cbprinter.SelectedIndex = i;
                                }
                            }
                        }
                        string scopy = data[3];
                        if (!string.IsNullOrEmpty(scopy))
                        {
                            var cbcopy = ((ComboBox)module.Controls.Find("cbcopy", true)[0]);
                            cbcopy.SelectedIndex = int.Parse(scopy)-1;
                        }
                        string ssize = data[4];
                        if (!string.IsNullOrEmpty(ssize))
                        {
                            var lstboxsize = ((ListBox)module.Controls.Find("lstboxsize", true)[0]);
                            for(int i=0; i < lstboxsize.Items.Count; i++)
                            {
                                if (lstboxsize.Items[i].ToString() == ssize)
                                {
                                    lstboxsize.SelectedIndex = i;
                                }
                            }
                            //lstboxsize = ssize;
                        }

                        module.setMapping(numberModule + 1);
                        module.Dock = DockStyle.None;
                        module.Location = new Point(0, (numberModule * moduleHeight));
                        panel1.Controls.Add(module);
                        numberModule++;
                    }
                }
            }    
        }
        private void btsave_Click(object sender, EventArgs e)
        {
            

            List<MappingData> lstMappingData = new List<MappingData>();
            MappingData data;
            foreach (Control c in panel1.Controls)
            {
                data = new MappingData();
                List<string> param = new List<string>();
                List<string> receipt = new List<string>();
                var uc = ((UserControl)c);
                var cklstboxparameter = ((CheckedListBox)uc.Controls.Find("cklstboxparameter", true)[0]);
                foreach (object itemChecked in cklstboxparameter.CheckedItems)
                {
                    param.Add(itemChecked.ToString());
                }
                var cklstboxfullreceipt = ((CheckedListBox)uc.Controls.Find("cklstboxfullreceipt", true)[0]);
                foreach (object itemChecked in cklstboxfullreceipt.CheckedItems)
                {
                    receipt.Add(itemChecked.ToString());
                }
                var cbprinter = ((ComboBox)uc.Controls.Find("cbprinter", true)[0]);
                var cbcopy = ((ComboBox)uc.Controls.Find("cbcopy", true)[0]);
                var lstboxsize = ((ListBox)uc.Controls.Find("lstboxsize", true)[0]);
                data.printers = cbprinter.SelectedItem.ToString();
                data.copies = cbcopy.SelectedItem.ToString();
                data.sizes = lstboxsize.SelectedItem.ToString();
                data.parameters = string.Join(",", param);
                data.fullreceipts = string.Join(",", receipt);
                lstMappingData.Add(data);
                //c.Hide();
            }
            if (File.Exists(System.IO.Path.GetFullPath("PrinterSettingLog.txt")))
                File.Delete(System.IO.Path.GetFullPath("PrinterSettingLog.txt"));
            if (lstMappingData.Count > 0)
            {
                using (StreamWriter writetext = new StreamWriter("PrinterSettingLog.txt"))
                {
                    foreach (MappingData mapping in lstMappingData)
                    {
                        string line = mapping.parameters + "|" + mapping.fullreceipts + "|" + mapping.printers + "|" + mapping.copies + "|" + mapping.sizes;
                        writetext.WriteLine(line);
                    }
                }
            }
            MessageBox.Show("The setting is saved");
        }
    }
}
