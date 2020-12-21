using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OOOneUnityTools.Editor
{
    public class SecTextureData
    {
        #region Public Variables

        public string AssetPath;
        public string Name;

        #endregion
    }

    public class UnityFileUtility
    {
        #region Public Variables

        public enum FileType
        {
            AnimatorOverride,
            AnimationClip,
            Png
        }

        #endregion

        #region Private Variables

        private static readonly Dictionary<FileType, string> FileExtension = new Dictionary<FileType, string>
        {
            {FileType.AnimatorOverride, "overrideController"},
            {FileType.AnimationClip, "anim"},
            {FileType.Png, "png"}
        };

        #endregion

        #region Public Methods

        public static bool CreateAssetFile(FileType fileType, string childPath, string fileName)
        {
            var fileNotExist = IsFileInPath(UnityPathUtility.GetUnityFullPath(childPath), fileName, fileType) == false;
            if (fileNotExist)
            {
                if (IsUnityFolderExist(childPath) == false) CreateUnityFolder(childPath);
                switch (fileType)
                {
                    case FileType.AnimatorOverride:
                        CreateUnityAsset(childPath, fileName, typeof(AnimatorOverrideController),
                            GetExtension(fileType));
                        break;
                    case FileType.AnimationClip:
                        CreateUnityAsset(childPath, fileName, typeof(AnimationClip), GetExtension(fileType));
                        break;
                    case FileType.Png:
                        CreatePng(childPath, fileName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
                }

                RefreshAsset();
            }

            return fileNotExist;
        }

        public static void CreatePng(string childPath, string fileName)
        {
            var path = UnityPathUtility.GetUnityAbsoluteFullPath(childPath, fileName, GetExtension(FileType.Png));
            var texture2D = Texture2D.blackTexture;
            var bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            RefreshAsset();
        }

        public static void CreatePreset(string targetPath, string fileFullPath, FileType fileType)
        {
            // string presetChildPath = unityFileUtilityTests._presetChildPath;
            var preset = new Preset(AssetImporter.GetAtPath(fileFullPath));
            if (fileType == FileType.Png)
            {
                var textureImporterForPreset = AssetImporter.GetAtPath(fileFullPath) as TextureImporter;
                // textureImporterForPreset.filterMode = FilterMode.Trilinear;
                preset = new Preset(textureImporterForPreset);
            }
            else
            {
                var assetImporterForPreset = AssetImporter.GetAtPath(fileFullPath);
                preset = new Preset(assetImporterForPreset);
            }

            AssetDatabase.CreateAsset(preset, targetPath);
            RefreshAsset();
        }

        public static void CreateTestPng(string childPath, string fileName, TextureColor color)
        {
            var path = UnityPathUtility.GetUnityAbsoluteFullPath(childPath, fileName, GetExtension(FileType.Png));
            var texture2D = Texture2D.normalTexture;
            switch (color)
            {
                case TextureColor.black:
                    texture2D = Texture2D.blackTexture;
                    break;
                case TextureColor.white:
                    texture2D = Texture2D.whiteTexture;
                    break;
                default:
                    texture2D = Texture2D.normalTexture;
                    break;
            }

            var bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            RefreshAsset();
        }

        public static void CreateUnityAsset(string childPath, string fileName, Type type, string extension)
        {
            var instance = (Object) Activator.CreateInstance(type);
            var path = UnityPathUtility.GetUnityFullPath(childPath, fileName, extension);
            AssetDatabase.CreateAsset(instance, path);
            RefreshAsset();
        }

        public static void CreateUnityFolder(string childPath)
        {
            var fullPath = UnityPathUtility.GetUnityAbsoluteFolderPath(childPath);
            if (CSharpFileUtility.IsFolderExist(fullPath) == false)
            {
                Directory.CreateDirectory(fullPath);
                RefreshAsset();
            }
        }

        public static bool DataEquals(string presetPath, string assetFullPath)
        {
            return DataEquals(presetPath, GetImporter(assetFullPath));
        }

        public static bool DataEquals(string presetPath, AssetImporter assetImporter)
        {
            return LoadPresetAtPath(presetPath).DataEquals(assetImporter);
        }

        public static void DeleteUnityFolder(string childPath)
        {
            AssetDatabase.DeleteAsset(UnityPathUtility.GetUnityFolderPath(childPath));
            RefreshAsset();
        }

        public static string GetExtension(FileType fileType)
        {
            if (FileExtension.ContainsKey(fileType))
                return FileExtension[fileType];

            return "";
        }

        public static AssetImporter GetImporter(string assetFullPath)
        {
            return AssetImporter.GetAtPath(assetFullPath);
        }

        public static bool IsFileInPath(string unityFullFolderPath, string fileName, FileType fileType)
        {
            var slashToCsharp = CSharpFileUtility.ParseSlashToCsharp(unityFullFolderPath);
            return CSharpFileUtility.IsFileInPath(slashToCsharp, fileName, GetExtension(fileType));
        }

        public static bool IsFileInPath(string unityFileFullPath)
        {
            var slashToCsharp = CSharpFileUtility.ParseSlashToCsharp(unityFileFullPath);
            return CSharpFileUtility.IsFileInPath(slashToCsharp);
        }

        public static bool IsUnityFolderExist(string childPath)
        {
            return CSharpFileUtility.IsFolderExist("Assets/" + childPath);
        }

        public static Preset LoadPresetAtPath(string presetPath)
        {
            return AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
        }

        public static void RefreshAsset()
        {
            AssetDatabase.Refresh();
        }

        public static bool SetSecondaryTexture(string mainTexFullPath, List<SecTextureData> secTextureDatas,
            out List<string> message)
        {
            var importer = GetImporter(mainTexFullPath) as TextureImporter;
            var secondarySpriteTextures = new SecondarySpriteTexture[secTextureDatas.Count];
            message = new List<string>();
            for (var i = 0; i < secTextureDatas.Count; i++)
            {
                var secTextureData = secTextureDatas[i];
                var texturePath = secTextureData.AssetPath;
                var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                if (texture2D == null || secTextureData.Name == string.Empty)
                {
                    message.Add($"Name:{secTextureData.Name} Path:{texturePath}");
                    continue;
                }

                var secondarySpriteTexture = new SecondarySpriteTexture
                {
                    name = secTextureData.Name, texture = texture2D
                };
                secondarySpriteTextures[i] = secondarySpriteTexture;
            }

            var success = message.Count == 0 && secTextureDatas.Count > 0 &&
                          importer != null;
            if (success)
            {
                importer.secondarySpriteTextures = secondarySpriteTextures;
                importer.SaveAndReimport();
            }

            return success;
        }


        public static void SetTextureImporterSetting(string presetPath, string texturePath)
        {
            if (IsFileInPath(texturePath) == false) return;
            var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
            if (preset == null) return;

            var presetType = preset.GetTargetTypeName();
            var textureFileExtenstion = CSharpFileUtility.GetExtensionFromFullPath(texturePath);
            if (presetType == "TextureImporter" && textureFileExtenstion == "png")
            {
                var textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                preset.ApplyTo(textureImporter);
                textureImporter.SaveAndReimport();
            }
        }

        #endregion
    }

    public enum TextureColor
    {
        black,
        white
    }
}