using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiEIS.Certificate
{
    public partial class Main : Form
    {
        private static string _uri = "http://dongtv.hisoft.vn/";

        private List<DataModel> data = null;
        public Main()
        {
            InitializeComponent();
        }

        private async Task GetDataFromServer()
        {
            using (var httpClient = new HttpClient())
            {
                var apiUri = new Uri(_uri + "api/CurrentSign/ApproveInvoices");

                var response = await httpClient
                    .GetAsync(apiUri + "?code=" + txtCode.Text.Trim())
                    .ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var resContent = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<DataModel>>(resContent);
            }
        }

        private void Signing()
        {
            var cert = Utils.GetCertificate();
            if (cert == null)
            {
                throw new Exception("Không nhận diện được chữ kí số, vui lòng kiểm tra lại");
            }
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                for (var i = 0; i < data.Count(); i++)
                {
                    var path = data[i].Path;
                    var pdfByte = Utils.SignWithThisCert(cert, path, "a");

                    client.BaseAddress = new Uri(_uri);
                    var outputPDF = pdfByte;
                    outputPDF.Headers.ContentDisposition = new ContentDispositionHeaderValue("FileContents")
                    {
                        FileName = data[i].Id
                    };
                    content.Add(outputPDF);
                }
                Boolean result = false;
                result = client.PostAsync("/api/Invoice/Sign", content).Result.IsSuccessStatusCode;
                if (!result)
                {
                    throw new Exception("Có lỗi xảy ra, vui lòng thử lại !!!");
                }
            }
        }
        
        private async void btnSign_Click(object sender, EventArgs e)
        {
            try
            {
                await GetDataFromServer();
                Signing();
                MessageBox.Show("Đã kí thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
