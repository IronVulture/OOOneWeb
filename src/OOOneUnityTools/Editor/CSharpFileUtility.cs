using System.IO;

namespace OOOneUnityTools.Editor
{
    public class CSharpFileUtility
    {
        #region Public Methods

        public static bool CheckFileExtension(string fullPath, string extension)
        {
            var fileExtension = GetExtensionFromFullPath(fullPath);
            return fileExtension == extension;
        }

        public static void CopyFile(string sourcePath, string targetPath)
        {
            if (File.Exists(sourcePath) == false) return;
            var directoryName = Path.GetDirectoryName(targetPath);
            CreateFolderIfNotExist(directoryName);
            if (IsFileAreEqual(sourcePath, targetPath) == false)
            {
                File.Copy(sourcePath, targetPath, false);
                UnityFileUtility.RefreshAsset();
            }
        }

        public static void CreateFolderIfNotExist(string directoryName)
        {
            if (IsFolderExist(directoryName) == false) Directory.CreateDirectory(directoryName);
        }

        public static string GetExtensionFromFullPath(string fullPath)
        {
            var fileExtension = Path.GetExtension(fullPath);
            fileExtension = fileExtension.Replace(".", "");
            return fileExtension;
        }

        public static string GetFullPath(string fileName, string FileExtension, string newFolderPath)
        {
            var fullPath = newFolderPath + @"\" + fileName + "." + FileExtension;
            return fullPath;
        }

        public static bool IsFileAreEqual(string pathA, string pathB)
        {
            if (File.Exists(pathA) == false || File.Exists(pathB) == false)
                return false;
            var sourceName = Path.GetFileName(pathA);
            var targetName = Path.GetFileName(pathB);
            var nameEqual = sourceName == targetName;
            return nameEqual;
        }

        public static bool IsFileInPath(string folderPath, string fileName, string fileExtension)
        {
            var newFolderPath = ParseSlashToCsharp(folderPath);
            var fullPath = GetFullPath(fileName, fileExtension, newFolderPath);
            return IsFileInPath(fullPath);
        }

        public static bool IsFileInPath(string fullPath)
        {
            var fileExists = File.Exists(fullPath);
            return fileExists;
        }

        public static bool IsFolderExist(string folderPath)
        {
            var parsedFolderPath = ParseSlashToCsharp(folderPath);
            return Directory.Exists(parsedFolderPath);
        }

        public static string ParseSlashToCsharp(string beforeParse)
        {
            return beforeParse.Replace("/", @"\");
        }

        #endregion
    }
}