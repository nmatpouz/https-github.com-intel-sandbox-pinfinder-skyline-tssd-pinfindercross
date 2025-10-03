using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PinFinder.Core.Extension
{
    internal class FileManagement
    {
        public static void EmptyDirectory(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }
            foreach (var dir in Directory.GetDirectories(path))
            {
                Directory.Delete(dir, true);
            }
        }
        public static string SanitizeFileName(string fileName)
        {
            // Replace invalid characters with an underscore
            return Regex.Replace(fileName, @"[<>:""/\\|?*]", "_");
        }
    }
}
