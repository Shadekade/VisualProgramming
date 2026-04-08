using System;
using System.IO;
using System.Linq;

namespace Lab_rab_4Kirichenko.Helper
{
    public static class PathHelper
    {
        public static string GetActualPath(string fileName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(baseDir);


            while (dir != null && !dir.GetFiles("*.csproj").Any() && !dir.GetFiles("*.sln").Any())
            {
                dir = dir.Parent;
            }

            if (dir != null)
            {

                string projectFilePath = Path.Combine(dir.FullName, "Model", fileName);
                if (File.Exists(projectFilePath))
                {
                    return projectFilePath;
                }
            }

            return Path.Combine(baseDir, fileName);
        }
    }
}