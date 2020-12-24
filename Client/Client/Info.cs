using System;
using System.Collections.Generic;
using System.IO;
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

        public static (string[], int) FileDirectoryList(string path) {
            if(path == null) {
                path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles();
            DirectoryInfo[] directories = di.GetDirectories();
            int elements = files.Length + directories.Length + 1;
            int dirlen = directories.Length;
            string[] retval = new string[elements];

            for (int i = 0; i < dirlen; i++) {
                retval[i] = directories[i].Name;
            }
            for (int i = dirlen; i < elements-1; i++) {
                retval[i] = files[i - dirlen].Name;
            }
            retval[elements-1] = path;
            return (retval, dirlen);
        }
    }
}
