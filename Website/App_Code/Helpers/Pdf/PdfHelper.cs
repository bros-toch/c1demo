using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Helpers.Pdf
{
    public class PdfHelper
    {
        public static byte[] GetPdfFile(string uniqueFileId)
        {
            string htmlFilePath = string.Format(@"{0}\{1}\{2}.html", HttpContext.Current.Server.MapPath("~"), @"PdfTemp\", uniqueFileId.ToString());
            int counter = 0;
            while (!File.Exists(htmlFilePath))
            {
                counter++;
                // wait a maximum of 1 minute to see if the HTML file is ready
                if (counter < 60)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    throw new Exception(string.Format("Failed to generate HTML for {0}", uniqueFileId.ToString()));
                }
            }

            PdfOutput pdfOutput = new PdfOutput()
            {
                OutputFilePath = htmlFilePath.Replace(".html", ".pdf")
            };

            var absoultePath = System.Web.VirtualPathUtility.ToAbsolute("~/");

            PdfDocument pdfDocument = new PdfDocument()
            {
                Url = string.Format("{0}://{1}/{2}PdfTemp/{3}.html", /*HttpContext.Current.Request.Url.Scheme*/"http",
                    HttpContext.Current.Request.Url.Host,
                    absoultePath == "/" ? string.Empty : absoultePath,
                    uniqueFileId)
            };

            //Logging.PutInfo("pdf url: " + pdfDocument.Url);

            PdfConvert.ConvertHtmlToPdf(pdfDocument, pdfOutput);
            var file = File.ReadAllBytes(htmlFilePath.Replace(".html", ".pdf"));

            File.Delete(htmlFilePath);
            File.Delete(htmlFilePath.Replace(".html", ".pdf"));
            return file;
        }

        internal static void PutHtmlFile(string uniqueFileId, string html)
        {
            string outputHtmlFilePath = string.Format(@"{0}\{1}\{2}.html", HttpContext.Current.Server.MapPath("~"), @"PdfTemp\", uniqueFileId);
            var regex = new Regex("(\\<script(.+?)\\</script\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = regex.Replace(html, string.Empty);
            File.AppendAllText(outputHtmlFilePath, html);
        }

        /// <summary>
        /// Generates the PDF certificate.
        /// </summary>
        /// <param name="htmlCert">The HTML cert.</param>
        /// <returns>Byte Array - containing PDF file of Certificate.</returns>
        public static byte[] GeneratePdf(string htmlCert)
        {
            byte[] pdfFile = null;

            // save the file to disk
            string uniqueFileId = Guid.NewGuid().ToString();
            string outputDirectoryPath = HttpContext.Current.Server.MapPath("~/PdfTemp/");
            string outputFilePath = Path.Combine(outputDirectoryPath, string.Format("{0}.html", uniqueFileId));

            if (!Directory.Exists(outputDirectoryPath))
            {
                Directory.CreateDirectory(outputDirectoryPath);
            }

            // save the file to disk
            using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
            {
                using (StreamWriter ws = new StreamWriter(fs, Encoding.UTF8))
                {
                    ws.WriteLine(htmlCert);
                }
            }

            // wait for the file to be created
            int counter = 0;
            while (!File.Exists(outputFilePath))
            {
                counter++;
                // wait a maximum of 1 minute to see if the HTML file is ready
                if (counter < 60)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    throw new Exception(string.Format("Failed to generate HTML for {0}", uniqueFileId.ToString()));
                }
            }

            // setup transport objects
            PdfOutput pdfOutput = new PdfOutput
            {
                OutputFilePath = outputFilePath.Replace(".html", ".pdf")
            };

            var absoultePath = VirtualPathUtility.ToAbsolute("~/");

            PdfDocument pdfDocument = new PdfDocument
            {
                Url = string.Format("{0}://{1}/{2}PdfTemp/{3}.html", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, absoultePath, uniqueFileId)
            };

            // make call to convert
            PdfConvert.ConvertHtmlToPdf(pdfDocument, pdfOutput);
            pdfFile = File.ReadAllBytes(outputFilePath.Replace(".html", ".pdf"));

            // clean up both files
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            if (File.Exists(outputFilePath.Replace(".html", ".pdf")))
            {
                File.Delete(outputFilePath.Replace(".html", ".pdf"));
            }

            return pdfFile;
        }

    }
}
