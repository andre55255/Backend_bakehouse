using System;
using System.IO;
using System.Linq;

namespace Bakehouse.Helpers
{
    public static class StaticMethods
    {
        public static string SaveFileDirectoryFromBase64(string folder, string base64, string nameFile)
        {
            try
            {
                int index = base64.IndexOf(",");
                string base64file = base64.Remove(0, index + 1);

                index = base64.IndexOf(";");
                string base64type = base64.Substring(0, index);
                index = base64.IndexOf("/");

                string extension = base64type.Substring(index + 1);

                byte[] bytes = Convert.FromBase64String(base64file);

                DateTime today = DateTime.Now;
                string suffix = string.Concat("_", Guid.NewGuid().ToString());
                string name = string.Concat(nameFile, suffix);

                string path = Path.Combine(folder, name);

                if (File.Exists(path))
                    File.Delete(path);

                Directory.CreateDirectory(folder);
                File.WriteAllBytes(path + "." + extension, bytes);

                return name + "." + extension;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool VerifyFileExistsAndDelete(string folder, string nameFile)
        {
            try
            {
                if (Directory.Exists(folder))
                {
                    string[] files = Directory.GetFiles(folder);
                    string file = files.Where(x => x.StartsWith(folder + nameFile))
                                       .FirstOrDefault();

                    if (file is null)
                        return false;

                    if (!File.Exists(file))
                        return false;

                    File.Delete(file);
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}