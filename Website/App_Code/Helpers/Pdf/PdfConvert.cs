using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;

namespace Helpers.Pdf
{
    public class PdfConvertException : Exception
    {
        public PdfConvertException(String msg) : base(msg) { }
    }

    public class PdfConvertTimeoutException : PdfConvertException
    {
        public PdfConvertTimeoutException() : base("HTML to PDF conversion process has not finished in the given period.") { }
    }

    public class Document
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PdfOutput
    {
        public String OutputFilePath { get; set; }
        public Stream OutputStream { get; set; }
        public Action<PdfDocument, byte[]> OutputCallback { get; set; }
    }

    public class PdfDocument
    {
        public String Url { get; set; }
        public String HeaderUrl { get; set; }
        public String FooterUrl { get; set; }
        public object State { get; set; }
    }

    public class PdfConvertEnvironment
    {
        public String TempFolderPath { get; set; }
        public String WkHtmlToPdfPath { get; set; }
        public int Timeout { get; set; }
        public bool Debug { get; set; }
    }

    public class PdfConvert
    {
        static PdfConvertEnvironment _e;

        public static PdfConvertEnvironment Environment
        {
            get
            {
                if (_e == null)
                    _e = new PdfConvertEnvironment
                    {
                        TempFolderPath = Path.GetTempPath(),
                        WkHtmlToPdfPath = Path.Combine(HttpContext.Current.Server.MapPath(@"~\App_Data"), @"wkhtmltopdf\wkhtmltopdf.exe"),
                        Timeout = 60000
                    };
                return _e;
            }
        }
        
        public static void ConvertHtmlToPdf(PdfDocument document, PdfOutput output)
        {
            ConvertHtmlToPdf(document, null, output);
        }

        public static void ConvertHtmlToPdf(PdfDocument document, PdfConvertEnvironment environment, PdfOutput woutput)
        {
            if (environment == null)
                environment = Environment;

            String outputPdfFilePath;
            bool delete;
            if (woutput.OutputFilePath != null)
            {
                outputPdfFilePath = woutput.OutputFilePath;
                delete = false;
            }
            else
            {
                outputPdfFilePath = Path.Combine(environment.TempFolderPath, String.Format("{0}.pdf", Guid.NewGuid()));
                delete = true;
            }

            if (!File.Exists(environment.WkHtmlToPdfPath))
                throw new PdfConvertException(String.Format("File '{0}' not found. Check if wkhtmltopdf application is installed.", environment.WkHtmlToPdfPath));

            ProcessStartInfo si;

            StringBuilder paramsBuilder = new StringBuilder();
            paramsBuilder.Append("--page-size A4 ");
            paramsBuilder.Append("--load-error-handling ignore ");
            //paramsBuilder.Append("--redirect-delay 0 "); not available in latest version

            paramsBuilder.Append("--margin-top 10 ");
            paramsBuilder.Append("--margin-left 5 ");
            paramsBuilder.Append("--margin-right 5 ");
            paramsBuilder.Append("--margin-bottom 10 ");

            if (!string.IsNullOrEmpty(document.HeaderUrl))
            {
                paramsBuilder.AppendFormat("--header-html {0} ", document.HeaderUrl);
                paramsBuilder.Append("--margin-top 0 ");
                paramsBuilder.Append("--header-spacing 0 ");
            }

            if (!string.IsNullOrEmpty(document.FooterUrl))
            {
                paramsBuilder.AppendFormat("--footer-html {0} ", document.FooterUrl);
                paramsBuilder.Append("--margin-bottom 0 ");
                paramsBuilder.Append("--footer-spacing 0 ");
            }

            //remove the border from the pages
            // paramsBuilder.Append("-L 0 -R 0 -T 0 -B 0 ");

            paramsBuilder.AppendFormat("\"{0}\" \"{1}\"", document.Url, outputPdfFilePath);


            si = new ProcessStartInfo();
            si.CreateNoWindow = true;
            si.FileName = environment.WkHtmlToPdfPath;
            si.Arguments = paramsBuilder.ToString();
            si.UseShellExecute = false;
            si.RedirectStandardError = true;
            si.RedirectStandardInput = true;
            si.RedirectStandardOutput = true;

            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = si;
                    process.Start();

                    if (!process.WaitForExit(environment.Timeout))
                        throw new PdfConvertTimeoutException();

                    if (!File.Exists(outputPdfFilePath))
                    {
                        if (process.ExitCode != 0)
                        {
                            var error = si.RedirectStandardError ? process.StandardError.ReadToEnd() : String.Format("Process exited with code {0}.", process.ExitCode);
                            throw new PdfConvertException(String.Format("Html to PDF conversion of '{0}' failed. Wkhtmltopdf output: \r\n{1}", document.Url, error));
                        }

                        throw new PdfConvertException(String.Format("Html to PDF conversion of '{0}' failed. Reason: Output file '{1}' not found.", document.Url, outputPdfFilePath));
                    }

                    if (woutput.OutputStream != null)
                    {
                        using (Stream fs = new FileStream(outputPdfFilePath, FileMode.Open))
                        {
                            byte[] buffer = new byte[32 * 1024];
                            int read;

                            while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                                woutput.OutputStream.Write(buffer, 0, read);
                        }
                    }

                    if (woutput.OutputCallback != null)
                    {
                        woutput.OutputCallback(document, File.ReadAllBytes(outputPdfFilePath));
                    }
                }
            }
            finally
            {
                if (delete && File.Exists(outputPdfFilePath))
                    File.Delete(outputPdfFilePath);
            }
        }

        /// <summary>
        /// Converts the HTML string to PDF file.
        /// </summary>
        /// <param name="htmlToConvert">The HTML to convert.</param>
        /// <returns>Byte Array - contains the PDF file from the HTML passed in.</returns>
        /// <remarks>Requires images to be embedded.</remarks>
        public static byte[] ConvertHtmlStringToPdfFile(string htmlToConvert)
        {
            byte[] pdfFile = null;

            // set temp build location
            PdfConvertEnvironment environment = Environment;
            string outputPdfFilePath = Path.Combine(environment.TempFolderPath, String.Format("{0}.pdf", Guid.NewGuid()));

            // check for the converter being installed in the site
            if (!File.Exists(environment.WkHtmlToPdfPath))
            {
                throw new PdfConvertException(String.Format("File '{0}' not found. Check if wkhtmltopdf application is installed.", environment.WkHtmlToPdfPath));
            }

            // setup the converter
            var processStartInfo = new ProcessStartInfo
                                       {
                                           CreateNoWindow = !environment.Debug,
                                           FileName = environment.WkHtmlToPdfPath,
                                           Arguments = "-q -n - " + outputPdfFilePath, // this will right out the file to disk
                                           UseShellExecute = false,
                                           RedirectStandardError = !environment.Debug,
                                           RedirectStandardInput = true,
                                           RedirectStandardOutput = true,
                                       };

            //p.StartInfo.Arguments = switches + " " + Url + " " + filename;

            //p.StartInfo.UseShellExecute = false; // needs to be false in order to redirect output
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.RedirectStandardInput = true; // redirect all 3, as it should be all 3 or none
            //p.StartInfo.WorkingDirectory = StripFilenameFromFullPath(p.StartInfo.FileName);

            //p.Start();

            // read the output here...
            //string output = p.StandardOutput.ReadToEnd(); 

            // file handling containers
            StreamWriter streamWriter;
            Stream stream = new MemoryStream();

            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();

                    if (!process.WaitForExit(environment.Timeout))
                    {
                        throw new PdfConvertTimeoutException();
                    }

                    if (!File.Exists(outputPdfFilePath))
                    {
                        if (process.ExitCode != 0)
                        {
                            var error = processStartInfo.RedirectStandardError ? process.StandardError.ReadToEnd() : String.Format("Process exited with code {0}.", process.ExitCode);
                            throw new PdfConvertException(String.Format("Html to PDF conversion of HTML static text failed. Wkhtmltopdf output: \r\n{0}", error));
                        }

                        throw new PdfConvertException(String.Format("Html to PDF conversion of HTML static text failed. Reason: Output file '{0}' not found.", outputPdfFilePath));
                    }

                    streamWriter = process.StandardInput;
                    streamWriter.AutoFlush = true;

                    streamWriter.Write(htmlToConvert);
                    streamWriter.Close();

                    // build a stream of the PDF file reading from disk 
                    using (Stream fs = new FileStream(outputPdfFilePath, FileMode.Open))
                    {
                        int read;
                        byte[] buffer = new byte[32 * 1024];

                        while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream.Write(buffer, 0, read);
                        }
                    }

                    // put the stream into an array to user download
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.Position = 0;
                        int read;
                        byte[] bufferOut = new byte[16 * 1024];

                        while ((read = stream.Read(bufferOut, 0, bufferOut.Length)) > 0)
                        {
                            ms.Write(bufferOut, 0, read);
                        }
                        pdfFile = ms.ToArray();
                    }
                }
            }
            finally
            {
                // clean up temp file that was generated
                if (File.Exists(outputPdfFilePath))
                {
                    File.Delete(outputPdfFilePath);
                }
            }

            return pdfFile;
        }

    }

    class OSUtil
    {
        public static string GetProgramFilesx86Path()
        {
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }
            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
    }

    //public static class HttpResponseExtensions
    //{
    //    public static void SendFileForDownload(this HttpResponse response, String filename, byte[] content)
    //    {
    //        SetFileDownloadHeaders(response, filename);
    //        response.OutputStream.Write(content, 0, content.Length);
    //        response.Flush();
    //    }

    //    public static void SendFileForDownload(this HttpResponse response, String filename)
    //    {
    //        SetFileDownloadHeaders(response, filename);
    //        response.TransmitFile(filename);
    //        response.Flush();
    //    }

    //    public static void SetFileDownloadHeaders(this HttpResponse response, String filename)
    //    {
    //        FileInfo fi = new FileInfo(filename);
    //        response.ContentType = "application/force-download";
    //        response.AddHeader("Content-Disposition", "attachment; filename=\"" + fi.Name + "\"");
    //    }
    //}
}
