using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HiEIS.Certificate
{
    public class Utils
    {
        const string folderNew = @"D:\Signed\{0}";
        const string fileNew = @"D:\Signed\{0}\signed-{1}";
        public static X509Certificate2 GetCertificate()
        {
            X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = userCaStore.Certificates;
                X509Certificate2Collection findResult = certificatesInStore.Find(X509FindType.FindBySubjectName, "VN", true);
                X509Certificate2 clientCertificate = null;
                if (findResult.Count > 0)
                {
                    clientCertificate = findResult[0];
                }
                else
                {
                    throw new Exception("Không nhận diện được chữ kí số, vui lòng kiểm tra lại");
                }
                return clientCertificate;
            }
            catch
            {
                throw;
            }
            finally
            {
                userCaStore.Close();
            }
        }
        public static ByteArrayContent SignWithThisCert(X509Certificate2 cert, string fileInputPath, string type)
        {
            string SourcePdfFileName = fileInputPath;
            var fileName = Path.GetFileName(SourcePdfFileName) +DateTime.Now.ToString("HHmmss")+".pdf";
            string dateNow = DateTime.Now.ToString("dd-MM-yyyy");
            if (!Directory.Exists(string.Format(folderNew, dateNow)))
            {
                Directory.CreateDirectory(string.Format(folderNew, dateNow));
            }
            string DestPdfFileName = String.Format(fileNew, dateNow, fileName);
            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };
            IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");
            PdfReader pdfReader = new PdfReader(SourcePdfFileName);
            //PdfReader pdfReader = new PdfReader(streamFile);
            FileStream signedPdf = new FileStream(DestPdfFileName, FileMode.Create);  //the output pdf file
            PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');
            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
            //signatureAppearance.SetVisibleSignature("Signature2");
            signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
            //signatureAppearance.Layer2Text = "Được ký bởi" + cert.GetName().ToString();
            switch (type)
            {
                case "Invoice":
                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(450, 150, 600, 250), pdfReader.NumberOfPages, null);
                    break;
                case "Sheet":
                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 0, 0, 0), pdfReader.NumberOfPages, null);
                    break;
            }
            //signatureAppearance.Layer2Font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN);
            BaseFont unicode =
                        BaseFont.CreateFont("c:/windows/fonts/times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            signatureAppearance.Layer2Font = new iTextSharp.text.Font(unicode);
            MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);

            return new ByteArrayContent(ReadFully(dateNow,fileName));
        }
        public static byte[] ReadFully(String dateNow,String fileName)
        {
            using (FileStream fileStream = File.Open(String.Format(fileNew, dateNow, fileName), FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }


    }
}
