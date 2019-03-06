using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HiEIS.Service
{
    class UtilService
    {
    }

    public class FileSupport
    {
        public MemoryStream Stream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }

    public class PdfSupport
    {
        public PdfSupport()
        {
            PdfDocuments = new List<PdfDocument>();
        }

        public List<PdfDocument> PdfDocuments { get; set; }
    }
}
