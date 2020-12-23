using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Client {
    class Info {

        public static string GetOSName() {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
        public static string GetOSInfo() {
            return Environment.OSVersion.Platform.ToString();
        }
        // Estimates their location, though US and England will be same
        public static string GetLocalLanguage() {
            return System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        }
    }
}
