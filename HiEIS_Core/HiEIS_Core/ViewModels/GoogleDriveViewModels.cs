using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class GoogleDriveUploadFileVM
    {
        public GoogleDriveUploadFileVM()
        {
            mimeType = "application/pdf";
        }
        public string name { get; set; }
        public string title { get; set; }
        public string mimeType { get; set; }
        public List<string> parents { get; set; }
    }

    public class GoogleDriveUploadFileSuccessVM
    {
        public string Id { get; set; }
    }
}
