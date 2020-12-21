using UnityEngine;

namespace OOOneUnityTools.Editor
{
    public class UnityPathUtility
    {
        private static readonly string UnityAbsolutePath = Application.dataPath;
        private const string UnityPath = "Assets";

        public static string GetUnityPath()
        {
            return UnityPath;
        }

        public static string GetUnityFullPath(string childPath, string fileName, string extension)
        {
            return CombineUnityFullPath(childPath, fileName, extension);
        }

        public static string GetUnityFullPath(string childPath) => $@"{Application.dataPath}\{childPath}";

        public static string GetUnityFolderPath(string childPath)
        {
            return CombineUnityPath(UnityPathUtility.GetUnityPath(), childPath);
        }

        public static string GetUnityAbsoluteFullPath(string childPath, string fileName, string extension)
        {
            return CombineAbsolutePath(GetUnityAbsoluteFolderPath(childPath), fileName, extension);
        }

        public static string GetUnityAbsoluteFolderPath(string childPath)
        {
            return GetAbsolutePath() + "/" + childPath;
        }

        public static string GetAbsolutePath()
        {
            return UnityAbsolutePath;
        }

        private static string CombineUnityFullPath(string childPath, string fileName, string extension)
        {
            return CombineAbsolutePath(UnityPathUtility.GetUnityFolderPath(childPath), fileName, extension);
        }

        private static string CombineAbsolutePath(string absolutePath, string fileName,
            string extension)
        {
            // var path = CombineUnityPath(absolutePath, childPath);
            var unityFullPath = $"{absolutePath}/{fileName}.{extension}";
            return unityFullPath;
        }

        private static string CombineUnityPath(string basePath, string childPath)
        {
            return basePath + "/" + childPath;
        }

        public static string GetCsharpUnityAbsoluteFolderPath(string childPath)
        {
            return CSharpFileUtility.ParseSlashToCsharp(GetUnityAbsoluteFolderPath(childPath));
        }

        public static string GetCsharpUnityAbsoluteFullPath(string childPath, string fileName, string extension)
        {
            return CSharpFileUtility.ParseSlashToCsharp(GetUnityAbsoluteFullPath(childPath, fileName, extension));
        }
    }
}