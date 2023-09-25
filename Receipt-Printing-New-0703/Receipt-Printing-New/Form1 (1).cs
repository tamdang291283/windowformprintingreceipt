using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using web = System.Windows.Forms;
using drawing = System.Drawing;
using System.Management;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Windows;
using System.Net;
using System.Threading;
using DevExpress.XtraPrinting.Native;
using System.Runtime.InteropServices;
using static Receipt_Printing_New.eodreport;
using System.Runtime.CompilerServices;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Data.SqlClient;
using Point = System.Drawing.Point;

namespace Receipt_Printing_New
{
    public partial class Form1 : Form
    {
        public byte[] picbytes;
        public string RootPath = "";  //ConfigurationSettings.AppSettings["RootPath"].ToString(); //System.IO.Path.GetFullPath("PrinterReceipt");
        public string PrintSettingPath = "";
        public string logpath = "";
        public string printsettingfile = "";
        public string SITEURL = "";

        System.Windows.Forms.Timer aTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer aTimerSound = new System.Windows.Forms.Timer();
        public bool printed = false;
        public string fileimageprinting = "";
        public bool flagtest = false;
        private NotifyIcon trayIcon;
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();

        public static DataTable dt;
        public static SqlConnection conn;
        public static SqlDataAdapter adapter;
        public static DataSet ds;
        public static int moduleHeight = 200;
        enum PrinterStatus
        {
            Other = 1,
            Unknown,
            Idle,
            Printing,
            Warmup,
            Stopped,
            printing,
            Offline
        }
        public Form1()
        {
            InitializeComponent();
            LoadFullReceiptIntoTable();
            LoadTabSetting();

            LoadPrinterSetting();
        }
        #region Tab_Setting
        public void LoadTabSetting()
        {
            try
            {

                //string text = "https://www.greek-painters.com/vo/food/7-6-dang-lang/printers/WinformsApp/OrderCome.asp?id_r=2";
                cbPaperWidth.Items.Add("58 mm");
                cbPaperWidth.Items.Add("80 mm");
                cbPaperWidth.SelectedIndex = 1;
                traynotification.BalloonTipText = "Printing Receipt";
                traynotification.BalloonTipTitle = "Printing Receipt";
                traynotification.Text = "Printing Receipt";
                EnableSound.Checked = true;
                txtInterval.Text = "5";
                RootPath = System.IO.Path.GetFullPath(@"PrinterReceipt\");
                PrintSettingPath = System.IO.Path.GetFullPath(@"PrinterSetting\");
                logpath = System.IO.Path.GetFullPath(@"log\");
                bool exists = System.IO.Directory.Exists(PrintSettingPath);
                if (!exists)
                    System.IO.Directory.CreateDirectory(PrintSettingPath);
                printsettingfile = PrintSettingPath + "setting.txt";

                // Detect delete file
                DeleteLogFile2week();
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
                    cbPrinterList.Items.Insert(index, name);
                    index += 1;
                }
                if (printernamedefault != "")
                    cbPrinterList.Text = printernamedefault;

                btnStop.Enabled = false;
                FixBrowserVersion();
                //txtAddressURL.Text = pageOrdercome;
                ReadSetting(printsettingfile);

                aTimer.Interval = 1000 * Int32.Parse(txtInterval.Text); // specify interval time as you want
                aTimer.Tick += new EventHandler(timer_Tick);

                //if (File.Exists(System.IO.Path.GetFullPath("PrinterLog.txt")))
                //    File.Delete(System.IO.Path.GetFullPath("PrinterLog.txt"));
                //if (AutoStart.Checked == true)
                //    trigerStart();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error Main:");
                web.MessageBox.Show("Error: " + ex.ToString());

            }
        }
        public void DeleteLogFile2week()
        {
            string[] filePaths = Directory.GetFiles(logpath, "*.txt",
                                         SearchOption.TopDirectoryOnly);
            foreach (string s in filePaths)
            {
                if (s.Contains("_PrinterLog.txt"))
                {
                    string sfilename = s.Replace("_PrinterLog.txt", "");
                    sfilename = sfilename.Split('\\')[sfilename.Split('\\').Length - 1];
                    sfilename = sfilename.Replace("-", "/");
                    DateTime ft = DateTime.Parse(sfilename);
                    int numberday = (int)(DateTime.Now.Date - ft.Date).TotalDays;
                    if (numberday >= 7)
                        File.Delete(s);

                }
                else if (s.Contains("_ErrorLog.txt"))
                {
                    string sfilename = s.Replace("_ErrorLog.txt", "");
                    sfilename = sfilename.Split('\\')[sfilename.Split('\\').Length - 1];
                    sfilename = sfilename.Replace("-", "/");
                    DateTime ft = DateTime.Parse(sfilename);
                    int numberday = (int)(DateTime.Now.Date - ft.Date).TotalDays;
                    if (numberday >= 7)
                        File.Delete(s);
                }
            }

        }
        public static int GetEmbVersion()
        {
            int ieVer = GetBrowserVersion();

            if (ieVer > 9)
                return ieVer * 1100 + 1;

            if (ieVer > 7)
                return ieVer * 1111;

            return 7000;
        } // End Function GetEmbVersion
        public static void FixBrowserVersion()
        {
            string appName = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            FixBrowserVersion(appName);
        }
        public static void FixBrowserVersion(string appName)
        {
            FixBrowserVersion(appName, GetEmbVersion());
        }
        public static void FixBrowserVersion(string appName, int ieVer)
        {
            FixBrowserVersion_Internal("HKEY_LOCAL_MACHINE", appName + ".exe", ieVer);
            FixBrowserVersion_Internal("HKEY_CURRENT_USER", appName + ".exe", ieVer);
            FixBrowserVersion_Internal("HKEY_LOCAL_MACHINE", appName + ".vshost.exe", ieVer);
            FixBrowserVersion_Internal("HKEY_CURRENT_USER", appName + ".vshost.exe", ieVer);
        } // End Sub FixBrowserVersion 

        private static void FixBrowserVersion_Internal(string root, string appName, int ieVer)
        {
            try
            {
                //For 64 bit Machine 
                if (Environment.Is64BitOperatingSystem)
                    Microsoft.Win32.Registry.SetValue(root + @"\Software\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, ieVer);
                else  //For 32 bit Machine 
                    Microsoft.Win32.Registry.SetValue(root + @"\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, ieVer);


            }
            catch (Exception ex)
            {
                // some config will hit access rights exceptions
                // this is why we try with both LOCAL_MACHINE and CURRENT_USER
                // WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error FixBrowserVersion_Internal:");
            }
        } // End Sub FixBrowserVersion_Internal 
        private static int GetBrowserVersion()
        {

            // string strKeyPath = @"HKLM\SOFTWARE\Microsoft\Internet Explorer";
            string strKeyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer";
            string[] ls = new string[] { "svcVersion", "svcUpdateVersion", "Version", "W2kVersion" };

            int maxVer = 0;
            try
            {
                for (int i = 0; i < ls.Length; ++i)
                {
                    object objVal = Microsoft.Win32.Registry.GetValue(strKeyPath, ls[i], "0");
                    string strVal = System.Convert.ToString(objVal);
                    if (strVal != null)
                    {
                        int iPos = strVal.IndexOf('.');
                        if (iPos > 0)
                            strVal = strVal.Substring(0, iPos);

                        int res = 0;
                        if (int.TryParse(strVal, out res))
                            maxVer = Math.Max(maxVer, res);
                    } // End if (strVal != null)

                } // Next i
            }
            catch (Exception ex)
            {
                //  WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error GetBrowserVersion:");

            }


            return maxVer;
        }

        private static Dictionary<string, drawing.Imaging.ImageCodecInfo> encoders = null;
        public static Dictionary<string, drawing.Imaging.ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demandd
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (encoders == null)
                {
                    encoders = new Dictionary<string, drawing.Imaging.ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (drawing.Imaging.ImageCodecInfo codec in drawing.Imaging.ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return encoders;
            }
        }
        private static drawing.Imaging.ImageCodecInfo getEncoderInfo(string mimeType)
        {
            //do a case insensitive look at the mime type
            mimeType = mimeType.ToLower();
            //the codec to return, default to null
            drawing.Imaging.ImageCodecInfo foundCodec = null;
            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(mimeType))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[mimeType];
            }
            return foundCodec;
        }

        public drawing.Image ScaleByWH(drawing.Image original, int destWidth, int destHeight)
        {

            drawing.Bitmap b = new drawing.Bitmap(destWidth, destHeight);
            drawing.Graphics g = drawing.Graphics.FromImage((drawing.Image)b);
            try
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = drawing.Drawing2D.PixelOffsetMode.Half;
                g.DrawImage(original, 0, 0, destWidth, destHeight);

                //SetContrast(b, 50);
            }
            finally
            {
                g.Dispose();
            }

            return (drawing.Image)b;
        }
        public static drawing.Bitmap MedianFilter(drawing.Bitmap Image, int Size)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
            int ApetureMin = -(Size / 2);
            int ApetureMax = (Size / 2);
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    List<int> RValues = new List<int>();
                    List<int> GValues = new List<int>();
                    List<int> BValues = new List<int>();
                    for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                    {
                        int TempX = x + x2;
                        if (TempX >= 0 && TempX < NewBitmap.Width)
                        {
                            for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                            {
                                int TempY = y + y2;
                                if (TempY >= 0 && TempY < NewBitmap.Height)
                                {
                                    drawing.Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                    RValues.Add(TempColor.R);
                                    GValues.Add(TempColor.G);
                                    BValues.Add(TempColor.B);
                                }
                            }
                        }
                    }
                    RValues.Sort();
                    GValues.Sort();
                    BValues.Sort();
                    drawing.Color MedianPixel = drawing.Color.FromArgb(RValues[RValues.Count / 2],
                        GValues[GValues.Count / 2],
                       BValues[BValues.Count / 2]);
                    NewBitmap.SetPixel(x, y, MedianPixel);
                }
            }
            return NewBitmap;
        }

        // this code relies on the LockedBitmap class
        // threshold should be a value between -100 and 100
        private static void SetContrast(drawing.Bitmap bmp, int threshold)
        {
            var lockedBitmap = new LockBitmap(bmp);
            lockedBitmap.LockBits();

            var contrast = Math.Pow((100.0 + threshold) / 100.0, 2);

            for (int y = 0; y < lockedBitmap.Height; y++)
            {
                for (int x = 0; x < lockedBitmap.Width; x++)
                {
                    var oldColor = lockedBitmap.GetPixel(x, y);
                    var red = ((((oldColor.R / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    var green = ((((oldColor.G / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    var blue = ((((oldColor.B / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    if (red > 255) red = 255;
                    if (red < 0) red = 0;
                    if (green > 255) green = 255;
                    if (green < 0) green = 0;
                    if (blue > 255) blue = 255;
                    if (blue < 0) blue = 0;

                    var newColor = drawing.Color.FromArgb(oldColor.A, (int)red, (int)green, (int)blue);
                    lockedBitmap.SetPixel(x, y, newColor);
                }
            }
            lockedBitmap.UnlockBits();
        }
        // Get ImageEncodeInfo of Image
        private drawing.Imaging.ImageCodecInfo GetEncoder(drawing.Imaging.ImageFormat format)
        {
            drawing.Imaging.ImageCodecInfo[] codecs = drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private void printing(string url, string imgfilename)
        {
            try
            {

                bool exists = System.IO.Directory.Exists(RootPath);
                if (!exists)
                    System.IO.Directory.CreateDirectory(RootPath);




                //picbytes = null;
                //makepicture(url);
                ////drawing.Bitmap bmSave = ByteToImage(picbytes);
                //drawing.Image img = ByteToImageImg(picbytes);



                #region GeneratePhotoFromHTML
                string html = getResponsePostRequest(url);


                drawing.Image img = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImage(html);

                #endregion

                //bmSave.Dispose();
                //Bitmap newImage = new Bitmap(newWidth, newHeight);
                int newHeight = img.Height;//(int)((295 * img.Height) / (img.Width));               
                int newWidth = img.Width;// 295;

                int paperwidth = cbPaperWidth.Text.Contains("80") ? 295 : 213;
                //int printHeight = (int)((295 * img.Height) / (img.Width));
                //int printWidth = 295;
                int printHeight = (int)((paperwidth * img.Height) / (img.Width));
                int printWidth = paperwidth;
                //213
                //int printHeight = (int)((213 * img.Height) / (img.Width));
                //int printWidth = 213;
                //img = ScaleByWH(img, newWidth, newHeight);


                //img.Save(RootPath + imgfilename + ".png", drawing.Imaging.ImageFormat.Png);
                //fileimageprinting = RootPath + imgfilename + ".png";

                img.Save(RootPath + imgfilename + ".png", drawing.Imaging.ImageFormat.Png);
                fileimageprinting = RootPath + imgfilename + ".png";

                img.Dispose();



                PrintDocument pd = new PrintDocument();
                PaperSize pkCustomSize1 = new PaperSize("First custom size", printWidth, printHeight);
                pd.DefaultPageSettings.Margins.Left = 0;
                pd.DefaultPageSettings.Margins.Top = 0;
                pd.DefaultPageSettings.PrinterResolution.Kind = PrinterResolutionKind.High;
                // pd.PrinterSettings.


                pd.DefaultPageSettings.PaperSize = pkCustomSize1;

                //  printDoc.DefaultPageSettings.PaperSize = pkCustomSize1
                pd.PrintPage += new PrintPageEventHandler(this.PrintPage);
                pd.Print();

            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error printing:");
            }
        }

        private PrinterStatus GetPrinterStat(string printerDevice)
        {
            PrinterStatus ret = 0;
            try
            {
                string path = "win32_printer.DeviceId='" + printerDevice + "'";
                using (ManagementObject printer = new ManagementObject(path))
                {
                    printer.Get();
                    PropertyDataCollection printerProperties = printer.Properties;
                    PrinterStatus st =
                    (PrinterStatus)Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
                    ret = st;
                }
            }
            catch (Exception ex)
            {

                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error PrinterStatus:");
            }

            return ret;
        }

        private void playSound(string path)
        {
            System.Media.SoundPlayer player =
                new System.Media.SoundPlayer();
            player.SoundLocation = path;
            player.Load();
            player.Play();
        }

        private void playPrinting()
        {
            player.Stop();
            player.SoundLocation = System.IO.Path.GetFullPath("beep.wav");
            player.Load();
            player.PlayLooping();
        }

        private void playServerError()
        {
            player.Stop();
            player.SoundLocation = System.IO.Path.GetFullPath("response-error.wav");
            player.Load();
            player.PlayLooping();
        }

        private void playNetworkError()
        {
            player.Stop();
            player.SoundLocation = System.IO.Path.GetFullPath("connection-error.wav");
            player.Load();
            player.PlayLooping();
        }
        private string getResponsePostRequest(string url)
        {
            string content = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (!response.StatusCode.ToString().ToLower().Contains("ok"))
                    return "NOTFOUND";
                content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (content.Contains("errorpage.asp")) { playServerError(); return "NOTFOUND"; }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString() + " url " + url, "Error getResponsePostRequest:");
                playNetworkError();
                return "ERROR";
            }

            return content;
        }
        private void makepicture(string url)
        {
            try
            {
                Thread thread = new Thread(delegate ()
                {
                    using (System.Windows.Forms.WebBrowser browser = new System.Windows.Forms.WebBrowser())
                    {
                        browser.ScrollBarsEnabled = false;
                        browser.AllowNavigation = true;
                        browser.BringToFront();
                        browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
                        browser.Navigate(url);
                        while (browser.ReadyState != WebBrowserReadyState.Complete)
                        {
                            System.Windows.Forms.Application.DoEvents();
                        }

                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error makepicture:");
            }

        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {

            try
            {
                System.Drawing.Image i = System.Drawing.Image.FromFile(fileimageprinting);

                e.Graphics.SmoothingMode = drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.PixelOffsetMode = drawing.Drawing2D.PixelOffsetMode.HighQuality;
                e.PageSettings.PrinterResolution.Kind = PrinterResolutionKind.High;
                e.Graphics.DrawImage(i, e.PageBounds);
                // e.Graphics.DrawImage(i, e.PageBounds);
                i.Dispose();
                if (File.Exists(fileimageprinting))
                {
                    File.Delete(fileimageprinting);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error PrintPage :");
            }

        }
        private void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                web.WebBrowser browser = sender as web.WebBrowser;
                int scrollWidth = 0;
                int scrollHeight = 0;

                scrollHeight = browser.Document.Body.ScrollRectangle.Height;
                scrollWidth = 512;
                browser.Size = new System.Drawing.Size(scrollWidth, scrollHeight);


                //Bitmap bm = new Bitmap(scrollWidth, scrollHeight);
                using (drawing.Bitmap bitmap = new drawing.Bitmap(scrollWidth, scrollHeight))
                {
                    browser.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, browser.Width, browser.Height));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        //bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = stream.ToArray();
                        picbytes = bytes;

                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error DocumentCompleted :");
            }

        }

        public drawing.Bitmap ByteToImage(byte[] blob)
        {
            try
            {
                MemoryStream mStream = new MemoryStream();
                byte[] pData = blob;
                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                drawing.Bitmap bm = new drawing.Bitmap(mStream, false);
                mStream.Dispose();
                return bm;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error DocumentCompleted :");
                return null;
            }


        }
        public drawing.Image ByteToImageImg(byte[] blob)
        {
            try
            {
                MemoryStream mStream = new MemoryStream();
                byte[] pData = blob;
                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                drawing.Image bm = drawing.Image.FromStream(mStream, true);  //new drawing.Bitmap(mStream, false);
                mStream.Dispose();
                return bm;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error DocumentCompleted :");
                return null;
            }


        }


        void timer_Tick(object sender, EventArgs e)
        {
            processPrinting();
        }
        void timer_TickSound(object sender, EventArgs e)
        {
            player.Stop();
            aTimerSound.Stop();
        }
        private void processPrinting()
        {
            try
            {
                if (printed == false)
                {
                    if (txtAddressURL.Text.ToString().Trim() != "")
                    {


                        printed = true;
                        WriteLog(txtAddressURL.Text.ToString().Trim(), "URL: ");

                        string responseText = getResponsePostRequest(txtAddressURL.Text.ToString().Trim());
                        WriteLog(responseText, "ResponseText: ");
                        if (responseText != "NOTFOUND")
                        {
                            string[] listresponseText = responseText.Split(new string[] { "[***]" }, StringSplitOptions.None);
                            string[] listparam = listresponseText[0].Split(new string[] { "[**]" }, StringSplitOptions.None);

                            string sURL = "";
                            string OrderID = "";
                            string ResID = "";
                            string[] listURL = new string[] { };
                            int countEN = 0;
                            int countCN = 0;
                            if (listparam.Length >= 3)
                            {
                                sURL = listparam[0];
                                OrderID = listparam[1];
                                ResID = listparam[2];
                                listURL = sURL.Split('|');

                            }

                            aTimerSound.Stop();
                            foreach (string url in listURL)
                            {
                                if (url != "")
                                {
                                    try
                                    {
                                        if (url.Contains("dishname"))
                                        {
                                            if (EnableSound.Checked == true)
                                                playPrinting();
                                            WriteLog(url, "printing: ");
                                            printing(url, ResID + "-" + OrderID + "-EN" + countEN);
                                            WriteLog("Success!", "printing: ");
                                            countEN += 1;
                                        }
                                        else
                                        {
                                            if (EnableSound.Checked == true)
                                                playPrinting();
                                            WriteLog(url, "printing: ");
                                            printing(url, ResID + "-" + OrderID + "-CN" + countCN);
                                            WriteLog("Success!", "printing: ");
                                            countCN += 1;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString() + " for URL " + url, "Error : ");
                                        player.Stop();
                                    }


                                }
                            }
                            // mark order is printed
                            if (OrderID != "")
                                responseText = getResponsePostRequest(SITEURL + "printers/WinformsApp/updateprinted.asp?id_r=" + ResID + "&o_id=" + OrderID);


                            // Proccess local
                            listparam = new string[] { };
                            if (listresponseText.Length > 1)
                                listparam = listresponseText[1].Split(new string[] { "[**]" }, StringSplitOptions.None);
                            sURL = "";
                            OrderID = "";
                            ResID = "";
                            listURL = new string[] { };
                            if (listparam.Length >= 3)
                            {
                                sURL = listparam[0];
                                OrderID = listparam[1];
                                ResID = listparam[2];
                                listURL = sURL.Split('|');
                            }

                            countEN = 0;
                            countCN = 0;

                            foreach (string url in listURL)
                            {
                                if (url != "")
                                {
                                    try
                                    {
                                        if (url.Contains("dishname"))
                                        {
                                            if (EnableSound.Checked == true)
                                                playPrinting();
                                            WriteLog(url, "printing: ");
                                            printing(url, ResID + "-" + OrderID + "-EN" + countEN);
                                            WriteLog("Success!", "printing: ");
                                            countEN += 1;
                                        }
                                        else
                                        {
                                            if (EnableSound.Checked == true)
                                                playPrinting();
                                            WriteLog(url, "printing: ");
                                            printing(url, ResID + "-" + OrderID + "-CN" + countCN);
                                            WriteLog("Success!", "printing: ");
                                            countCN += 1;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString() + " for URL " + url, "Error : ");
                                        player.Stop();
                                    }
                                }
                            }
                            if (OrderID != "")
                                responseText = getResponsePostRequest(SITEURL + "printers/WinformsApp/updateprinted.asp?id_r=" + ResID + "&o_id=" + OrderID + "&local=Y");
                            // End 
                            aTimerSound.Interval = 3000; // 20 mins
                            aTimerSound.Tick += new EventHandler(timer_TickSound);
                            aTimerSound.Start();

                        }
                        printed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error processPrinting :");
            }

        }

        private void trigerStart()
        {
            try
            {
                flagtest = false;

                myPrinters.SetDefaultPrinter(cbPrinterList.Text.ToString().Trim());
                if (txtAddressURL.Text.ToString().Trim() != "")
                {
                    cbPrinterList.Enabled = false;
                    txtInterval.Enabled = false;
                    AutoStart.Enabled = false;
                    EnableSound.Enabled = false;
                    txtAddressURL.Enabled = false;
                    cbPaperWidth.Enabled = false;

                    SITEURL = txtAddressURL.Text.ToString().Trim().Substring(0, txtAddressURL.Text.ToString().Trim().IndexOf("printers"));
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    // processPrinting();
                    aTimer.Interval = 1000 * Int32.Parse(txtInterval.Text);
                    aTimer.Start();
                    //aTimer.Enabled = true; 
                }
                else
                {
                    web.MessageBox.Show("Please input Address URL");
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error : ");
            }

        }



        private void WriteErrorLog(string logcontent, string error)
        {
            string filenamedate = DateTime.Now.ToShortDateString();
            filenamedate = filenamedate.Replace("/", "-");
            if (!File.Exists(logpath + filenamedate + "_ErrorLog.txt"))
            {
                var isfile = File.Create(logpath + filenamedate + "_ErrorLog.txt");
                isfile.Close();
                isfile.Dispose();
            }
            using (StreamWriter w = File.AppendText(logpath + "ErrorLog.txt"))
            {
                Log(error + logcontent, w);
                // Close the writer and underlying file.
                w.Close();
            }
        }

        private void WriteLog(string logcontent, string error)
        {
            string filenamedate = DateTime.Now.ToShortDateString();
            filenamedate = filenamedate.Replace("/", "-");
            if (!File.Exists(logpath + filenamedate + "_PrinterLog.txt"))
            {
                var isfile = File.Create(logpath + filenamedate + "_PrinterLog.txt");
                isfile.Close();
                isfile.Dispose();
            }

            using (StreamWriter w = File.AppendText(logpath + filenamedate + "_PrinterLog.txt"))
            {
                Log(error + logcontent, w);
                // Close the writer and underlying file.
                w.Close();
            }
        }
        private void WriteSettingFile(string FileName)
        {
            using (StreamWriter w = File.CreateText(FileName))
            {
                w.WriteLine("printername|" + cbPrinterList.Text.ToString());
                w.WriteLine("timeinterval|" + txtInterval.Text.ToString());
                w.WriteLine("enablesound|" + EnableSound.Checked.ToString().ToLower());
                w.WriteLine("urlordercome|" + txtAddressURL.Text.ToString());
                w.WriteLine("autostart|" + AutoStart.Checked.ToString().ToLower());
                w.WriteLine("PaperWidth|" + cbPaperWidth.Text.ToLower());
                w.Flush();
                w.Close();
            }
        }
        private void ReadSetting(string filename)
        {
            try
            {
                string printername = "", timeinterval = "5", enablesound = "true", urlordercome = "", autostart = "", PaperWidth = "";
                if (File.Exists(filename))
                {
                    using (StreamReader r = new StreamReader(filename))
                    {
                        string line;

                        while ((line = r.ReadLine()) != null)
                        {
                            if (line.Contains("printername|"))
                            {
                                printername = line.Split('|')[1].ToString().Trim();
                            }
                            if (line.Contains("timeinterval|"))
                            {
                                timeinterval = line.Split('|')[1].ToString().Trim();
                            }
                            if (line.Contains("enablesound|"))
                            {
                                enablesound = line.Split('|')[1].ToString().Trim();
                            }
                            if (line.Contains("urlordercome|"))
                            {
                                urlordercome = line.Split('|')[1].ToString().Trim();
                            }
                            if (line.Contains("autostart|"))
                                autostart = line.Split('|')[1].ToString().Trim();
                            if (line.Contains("PaperWidth|"))
                                PaperWidth = line.Split('|')[1].ToString().Trim();
                        }
                        r.Close();

                    }
                }

                if (printername != "")
                    cbPrinterList.Text = printername;
                txtAddressURL.Text = urlordercome;
                txtInterval.Text = timeinterval;
                if (enablesound == "true")
                    EnableSound.Checked = true;
                else
                    EnableSound.Checked = false;
                if (autostart == "true")
                {
                    AutoStart.Checked = true;
                }
                if (PaperWidth != "")
                    cbPaperWidth.Text = PaperWidth;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error : ");
            }

        }
        private void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
            // Update the underlying file.
            w.Flush();
        }
        private void DumpLog(StreamReader r)
        {
            // While not at the end of the file, read and write lines.
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
            r.Close();
        }

        private void EnableSound_Checked(object sender, EventArgs e)
        {

            if (EnableSound.Checked == false)
                player.Stop();
        }
        private void AutoStart_Checked(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;

                WriteSettingFile(printsettingfile);
                trigerStart();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error btnStart_Click : ");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cbPrinterList.Enabled = true;
            txtInterval.Enabled = true;
            AutoStart.Enabled = true;
            EnableSound.Enabled = true;
            txtAddressURL.Enabled = true;
            cbPaperWidth.Enabled = true;
            aTimer.Stop();
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                traynotification.Visible = true;
                traynotification.ShowBalloonTip(1000);
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                traynotification.Visible = false;
            }
        }

        private void traynotification_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            traynotification.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }


        private string data1 = "";
        private void ShowForm(string status)
        {
            if (status == "show")
                this.Show();
        }
        private string GetRestaurantid()
        {
            string restaurantid = "";
            string  urlordercome = "";
            if (File.Exists(filename))
            {
                using (StreamReader r = new StreamReader(filename))
                {
                    string line;

                    while ((line = r.ReadLine()) != null)
                    {
                       
                        if (line.Contains("urlordercome|"))
                        {
                            urlordercome = line.Split('|')[1].ToString().Trim();
                            break;
                        }
                      
                    }
                    r.Close();

                }
            }


            if (urlordercome.Trim() != "")
            {
                restaurantid = urlordercome.Substring(urlordercome.IndexOf("?") + 1, urlordercome.Length - urlordercome.IndexOf("?") - 1);
                restaurantid = restaurantid.Replace("id_r=", "");
            }
            if (restaurantid == "")
                restaurantid = "0";
            return restaurantid;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            WriteSettingFile(printsettingfile);
            string restaurantid = GetRestaurantid();
            //if (txtAddressURL.Text.ToString().Trim() != "")
            //{
            //    restaurantid = txtAddressURL.Text.ToString().Substring(txtAddressURL.Text.ToString().IndexOf("?") + 1, txtAddressURL.Text.ToString().Length - txtAddressURL.Text.ToString().IndexOf("?") - 1);
            //    restaurantid = restaurantid.Replace("id_r=", "");
            //}
            if (restaurantid == "")
            {
                MessageBox.Show("Please input URL have param id_r=");
            }
            else
            {
                eodreport ofr = new eodreport(restaurantid);
                //Subscribe frm for Callback
                this.Hide();
                ofr.setvalue = new eodreport.SetParameterValueDelegate(ShowForm);
                ofr.Show();
            }





        }

        #endregion

        #region Main_Setting
        public DataTable GetFullReceipt()
        {
            return dt;
        }
        public int GetModuleHeight()
        {
            return moduleHeight;
        }
        public void LoadFullReceiptIntoTable()
        {
            string connectionstring = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            conn = new SqlConnection(connectionstring);
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                ds = new DataSet();
                string restaurantid = GetRestaurantid();
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
                "SQL ", conn);
                adapter.Fill(ds);
                dt = new DataTable();
                dt = ds.Tables[0];
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }


            //this.cklstboxfullreceipt.DataSource = ds.Tables[0];

        }
        private void LoadPrinterSetting()
        {
            ucModule1 module;
            int numberModule = 0;
            int rowNumber = 0;
            if (File.Exists(System.IO.Path.GetFullPath("PrinterSettingLog.txt")))
            {
                using (StreamReader readtext = new StreamReader("PrinterSettingLog.txt"))
                {

                    while (true)
                    {

                        string readText = readtext.ReadLine();
                        if (rowNumber <= 0)
                        { rowNumber++; continue; }
                        if (string.IsNullOrEmpty(readText))
                        { return; }
                        string[] data = readText.Split('|');
                        module = new ucModule1();

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
                        string isfullreceipt = data[1];
                        var cbfullreceipt = ((CheckBox)module.Controls.Find("cbfullreceipt", true)[0]);
                        cbfullreceipt.Checked = Convert.ToBoolean(isfullreceipt);
                        string sreceipt = data[2];
                        if (!string.IsNullOrEmpty(sreceipt))
                        {
                            var cklstboxfullreceipt = ((CheckedListBox)module.Controls.Find("cklstboxfullreceipt", true)[0]);
                            for (int i = 0; i < cklstboxfullreceipt.Items.Count; i++)
                            {
                                ItemCategory itemCategory = (ItemCategory)cklstboxfullreceipt.Items[i];
                                string[] lst = sreceipt.Split(',');
                                foreach (string s in lst)
                                {
                                    if(itemCategory.svalue == s)                                    
                                    {
                                        cklstboxfullreceipt.SetItemChecked(i, true);
                                    }
                                }

                            }
                        }
                        string sprinter = data[3];
                        if (!string.IsNullOrEmpty(sprinter))
                        {
                            var cbprinter = ((ComboBox)module.Controls.Find("cbprinter", true)[0]);
                            for (int i = 0; i < cbprinter.Items.Count; i++)
                            {
                                if (cbprinter.Items[i].ToString() == sprinter)
                                {
                                    cbprinter.SelectedIndex = i;
                                }
                            }
                        }
                        string scopy = data[4];
                        if (!string.IsNullOrEmpty(scopy))
                        {
                            var cbcopy = ((ComboBox)module.Controls.Find("cbcopy", true)[0]);
                            cbcopy.SelectedIndex = int.Parse(scopy) - 1;
                        }
                        string ssize = data[5];
                        if (!string.IsNullOrEmpty(ssize))
                        {
                            var lstboxsize = ((ListBox)module.Controls.Find("lstboxsize", true)[0]);
                            for (int i = 0; i < lstboxsize.Items.Count; i++)
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
        #endregion
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btAddNewMapping_Click(object sender, EventArgs e)
        {
            int numberModule = panel1.Controls.Count;
            panel1.VerticalScroll.Value = 0;
            ucModule1 module = new ucModule1();
            module.setMapping(numberModule + 1);
            //module.setFullReceiptDefault();
            module.Dock = DockStyle.None;
            module.Location = new Point(0, numberModule * moduleHeight);

            panel1.Controls.Add(module);
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
                var cbfullreceipt = ((CheckBox)uc.Controls.Find("cbfullreceipt", true)[0]);

                var cklstboxfullreceipt = ((CheckedListBox)uc.Controls.Find("cklstboxfullreceipt", true)[0]);
                foreach (object itemChecked in cklstboxfullreceipt.CheckedItems)
                {
                    ItemCategory itemCategory = (ItemCategory)itemChecked;
                    receipt.Add(itemCategory.svalue);
                }
                var cbprinter = ((ComboBox)uc.Controls.Find("cbprinter", true)[0]);
                var cbcopy = ((ComboBox)uc.Controls.Find("cbcopy", true)[0]);
                var lstboxsize = ((ListBox)uc.Controls.Find("lstboxsize", true)[0]);
                data.printers = cbprinter.SelectedItem.ToString();
                data.copies = cbcopy.SelectedItem.ToString();
                data.sizes = lstboxsize.SelectedItem.ToString();
                data.isfullreceipt = cbfullreceipt.Checked;
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
                    string columns = "Parameters|Full Receipt|Categories|Printer name|Copies|Paper width";
                    writetext.WriteLine(columns);
                    foreach (MappingData mapping in lstMappingData)
                    {
                        string line = mapping.parameters + "|" + mapping.isfullreceipt + "|" + mapping.fullreceipts + "|" + mapping.printers + "|" + mapping.copies + "|" + mapping.sizes;
                        writetext.WriteLine(line);
                    }
                }
            }
            MessageBox.Show("The setting is saved");
        }
    }

    public static class myPrinters
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

    }
}
