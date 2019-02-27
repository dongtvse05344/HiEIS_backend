using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.Utils
{
    public class StringUtils
    {
        public static string Replace(string s)
        {
            return s.Replace("&Agrave;", "À").Replace("&Egrave;", "È")
                .Replace("&Igrave;", "Ì").Replace("&Ograve;", "Ò")
                .Replace("&Ugrave;", "Ù").Replace("&agrave;", "à")
                .Replace("&egrave;", "è").Replace("&igrave;", "ì")
                .Replace("&ograve;", "ò").Replace("&ugrave;", "ù")
                .Replace("&Aacute;", "Á").Replace("&Eacute;", "É")
                .Replace("&Iacute;", "Í").Replace("&Oacute;", "Ó")
                .Replace("&Uacute;", "Ú").Replace("&aacute;", "á")
                .Replace("&eacute;", "é").Replace("&iacute;", "í")
                .Replace("&oacute;", "ó").Replace("&uacute;", "ú")
                .Replace("&Acirc;", "Â").Replace("&Ecirc;", "Ê")
                .Replace("&Ocirc;", "Ô").Replace("&acirc;", "â")
                .Replace("&ecirc;", "ê").Replace("&ocirc;", "ô");
        }
    }
}
