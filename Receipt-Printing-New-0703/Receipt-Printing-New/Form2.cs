using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using web = System.Windows.Forms;
using drawing = System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static DevExpress.Xpo.Helpers.PerformanceCounters;

namespace Receipt_Printing_New
{
    public partial class eodreport : Form
    {
        public delegate void SetParameterValueDelegate(string value);
        public SetParameterValueDelegate setvalue;
        private string resid1 = "";
        public byte[] picbytes;
        public string RootPath = "";  //ConfigurationSettings.AppSettings["RootPath"].ToString(); //System.IO.Path.GetFullPath("PrinterReceipt");
        public string PrintSettingPath = "";
        public string logpath = "";
        public string printsettingfile = "";
        public string SITEURL = "";
        public string printername = "";
        public string PaperWidth = "";
        public string URLPrinting = "";
        public string restaurantid = "";
        public string fileimageprinting = "";
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

                int paperwidth = PaperWidth.Contains("80") ? 295 : 213;
                //int printHeight = (int)((295 * img.Height) / (img.Width));
                //int printWidth = 295;
                int printHeight = (int)((paperwidth * img.Height) / (img.Width));
                int printWidth = paperwidth;
                //213
                //int printHeight = (int)((213 * img.Height) / (img.Width));
                //int printWidth = 213;
                img = ScaleByWH(img, printWidth, printHeight);


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
               // WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error printing:");
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

              //  WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error PrinterStatus:");
            }

            return ret;
        }

        private void ReadSetting(string filename)
        {
            try
            {
               
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
                            if (line.Contains("PaperWidth|"))
                                PaperWidth = line.Split('|')[1].ToString().Trim();
                        }
                        r.Close();

                    }
                }
             
            }
            catch (Exception ex)
            {
            
            }

        }
        public eodreport()
        {
           
        }
        public  eodreport(string resid)
        {
            resid1 = resid;
            InitializeComponent();
            RootPath = System.IO.Path.GetFullPath(@"PrinterReceipt\");
            PrintSettingPath = System.IO.Path.GetFullPath(@"PrinterSetting\");
            logpath = System.IO.Path.GetFullPath(@"log\");
            bool exists = System.IO.Directory.Exists(PrintSettingPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(PrintSettingPath);
            printsettingfile = PrintSettingPath + "setting.txt";
            ReadSetting(printsettingfile);
        }

        private void eodreport_Load(object sender, EventArgs e)
        {
            URLPrinting = "https://www.greek-painters.com/vo/food/7-6-Dang-lang/cms/dashboards/quickaccess-eod-nologin.asp?r_id=" + resid1;
            webBrowser1.Navigate(URLPrinting);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
               setvalue("show");
               this.Dispose();
               this.Close();
        }

        private void eodreport_FormClosing(object sender, FormClosingEventArgs e)
        {
            setvalue("show");
            this.Dispose();
            this.Close();
        }

        private void Print_Click(object sender, EventArgs e)
        {
            // webBrowser1.ShowPrintDialog();
            printing(URLPrinting, resid1 + "-" + DateTime.Now.Ticks.ToString());
           // string responseText = getResponsePostRequest(URLPrinting);
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
                if (content.Contains("errorpage.asp")) { return "NOTFOUND"; }
            }
            catch (Exception ex)
            {
                //WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString() + " url " + url, "Error getResponsePostRequest:");
               // playNetworkError();
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
               // WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error makepicture:");
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
               // WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error PrintPage :");
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
                //WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error DocumentCompleted :");
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
               // WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error DocumentCompleted :");
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
               // WriteErrorLog(ex.Message.ToString() + " trace " + ex.StackTrace.ToString(), "Error DocumentCompleted :");
                return null;
            }


        }


    }

    public static class myPrinters1
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

    }
}
