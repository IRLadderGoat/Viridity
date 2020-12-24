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
        public static string GetOSVersion () {
            switch (Environment.OSVersion.Platform) {
                case PlatformID.Win32S:
                    return "Win 3.1";
                case PlatformID.Win32Windows:
                    switch (Environment.OSVersion.Version.Minor) {
                        case 0:
                            return "Win95";
                        case 10:
                            return "Win98";
                        case 90:
                            return "WinME";
                    }
                    break;

                case PlatformID.Win32NT:
                    switch (Environment.OSVersion.Version.Major) {
                        case 3:
                            return "NT 3.51";
                        case 4:
                            return "NT 4.0";
                        case 5:
                            switch (Environment.OSVersion.Version.Minor) {
                                case 0:
                                    return "Win2000";
                                case 1:
                                    return "WinXP";
                                case 2:
                                    return "Win2003";
                            }
                            break;

                        case 6:
                            switch (Environment.OSVersion.Version.Minor) {
                                case 0:
                                    return "Vista/Win2008Server";
                                case 1:
                                    return "Win7/Win2008Server R2";
                                case 2:
                                    return "Win8/Win2012Server";
                                case 3:
                                    return "Win8.1/Win2012Server R2";
                            }
                            break;
                        case 10:  //this will only show up if the application has a manifest file allowing W10, otherwise a 6.2 version will be used
                            return "Windows 10";
                    }
                    break;

                case PlatformID.WinCE:
                    return "Win CE";
            }

            return "Unknown";
        }
        // Estimates their location, though US and England will be same
        public static string GetLocalLanguage() {
            return System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        }
        
        public static string GetCPUUsage () {
            System.Diagnostics.PerformanceCounter pc = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total");
            pc.NextValue();
            return pc.NextValue()+"%";
        }

        public static string GetRAMUsage () {
            System.Diagnostics.PerformanceCounter pc = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
            return pc.NextValue()+"MB";
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
