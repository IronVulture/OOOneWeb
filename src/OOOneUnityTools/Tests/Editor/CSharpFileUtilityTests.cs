using System.IO;
using NUnit.Framework;

namespace OOOneUnityTools.Editor.Tests
{
    public class CSharpFileUtilityTests
    {
        #region Private Variables

        private string _fileName;
        private string _animExtension;
        private string _pngExtension;
        private string _source1_ChildPath;
        private string _source1_PngFullPath;
        private string _source2_ChildPath;
        private string _source2_PngFullPath;
        private string _source1_AnimFullPath;
        private string _target_AnimFullPath;
        private string _target_PngFullPath;
        private string _targetChildPath;
        private string _targetChildPath2;
        private string _targetFolderPath;
        private string _source1_PngFolderPath;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _source1_ChildPath = "asdfasdlfja";
            _targetChildPath = "eedkcvjiosder";
            _source2_ChildPath = "lksdfkj";
            _fileName = "235432asdfasdf";
            _pngExtension = "png";
            _animExtension = "anim";
            _targetFolderPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFolderPath(_targetChildPath);
            _source1_PngFolderPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFolderPath(_source1_ChildPath);
            _source1_PngFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_source1_ChildPath, _fileName, _pngExtension);
            _source2_PngFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_source2_ChildPath, _fileName, _pngExtension);
            _source1_AnimFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_source1_ChildPath, _fileName, _animExtension);
            _target_PngFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_targetChildPath, _fileName, _pngExtension);
            _target_AnimFullPath =
                UnityPathUtility.GetCsharpUnityAbsoluteFullPath(_targetChildPath, _fileName, _animExtension);
        }

        #endregion

        #region Test Methods

        [Test]
        public void IsFolderExist()
        {
            CreateFolderUseTargetPath();
            var isFolderExist = CSharpFileUtility.IsFolderExist(_targetFolderPath);
            Assert.IsTrue(isFolderExist);
        }

        [Test]
        public void ParseSlashToCsharpTest()
        {
            var beforeParsePath = "Assets/asdfasdlfja";
            var afterParse = @"Assets\asdfasdlfja";
            var actual = CSharpFileUtility.ParseSlashToCsharp(beforeParsePath);
            Assert.AreEqual(afterParse, actual);
        }

        [Test]
        public void IsFileInPath()
        {
            CreateFolerUsePathSource1();
            CreateWhitePngInFolderSource1();
            var isFileInPath = CSharpFileUtility.IsFileInPath(_source1_PngFolderPath, _fileName, _pngExtension);
            Assert.AreEqual(true, isFileInPath);
        }

        [Test]
        public void CopyFile_If_Folder_Exist_And_File_NotExist()
        {
            CreateFolerUsePathSource1();
            CreateWhitePngInFolderSource1();
            CreateFolderUseTargetPath();
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            ShouldFileSource1EqualtoTarget(_source1_PngFullPath, _target_PngFullPath);
        }

        [Test]
        public void Dont_CopyFile_If_FileNameAndExtension_Is_Same()
        {
            CreateFolerUsePathSource1();
            CreateWhitePngInFolderSource1();
            CreateFolderUsePathSource2();
            CreateBlackPngInFolderSource2();
            CreateFolderUseTargetPath();
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            CopyFile(_source2_PngFullPath, _target_PngFullPath);
            ShouldFileSource1EqualtoTarget(_source1_PngFullPath, _target_PngFullPath);
        }

        [Test]
        public void CopyFile_If_FileExtension_Is_Not_Same()
        {
            CreateFolerUsePathSource1();
            CreateWhitePngInFolderSource1();
            UnityFileUtility.CreateAssetFile(UnityFileUtility.FileType.AnimationClip, _source1_ChildPath, _fileName);
            CreateFolderUseTargetPath();
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            CopyFile(_source1_AnimFullPath, _target_AnimFullPath);
            ShouldFileSource1EqualtoTarget(_source1_PngFullPath, _target_PngFullPath);
            ShouldFileSource1EqualtoTarget(_source1_AnimFullPath, _target_AnimFullPath);
        }

        [Test]
        public void CopyFile_If_TargetFolder_Is_NotExist()
        {
            CreateFolerUsePathSource1();
            CreateWhitePngInFolderSource1();
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            ShouldFileSource1EqualtoTarget(_source1_PngFullPath, _target_PngFullPath);
        }

        [Test]
        public void Not_CopyFile_If_Source_Is_NotExist()
        {
            _source1_PngFullPath = "asdjfk";
            CopyFile(_source1_PngFullPath, _target_PngFullPath);
            Assert.AreEqual(false, File.Exists(_target_PngFullPath));
        }

        #endregion

        #region Public Methods

        [TearDown]
        public void TearDown()
        {
            DeleteFolderSourceAndTarget();
        }

        #endregion

        #region Private Methods

        private void CopyFile(string sourcePath, string targetPath)
        {
            CSharpFileUtility.CopyFile(sourcePath, targetPath);
        }

        private void CreateBlackPngInFolderSource2()
        {
            UnityFileUtility.CreateTestPng(_source2_ChildPath, _fileName, TextureColor.black);
        }

        private void CreateFolderUsePathSource2()
        {
            UnityFileUtility.CreateUnityFolder(_source2_ChildPath);
        }

        private void CreateFolderUseTargetPath()
        {
            UnityFileUtility.CreateUnityFolder(_targetChildPath);
        }

        private void CreateFolerUsePathSource1()
        {
            UnityFileUtility.CreateUnityFolder(_source1_ChildPath);
        }

        private void CreateWhitePngInFolderSource1()
        {
            UnityFileUtility.CreateTestPng(_source1_ChildPath, _fileName, TextureColor.white);
        }

        private void DeleteFolderSourceAndTarget()
        {
            UnityFileUtility.DeleteUnityFolder(_source1_ChildPath);
            UnityFileUtility.DeleteUnityFolder(_source2_ChildPath);
            UnityFileUtility.DeleteUnityFolder(_targetChildPath);
        }

        private void ShouldFileSource1EqualtoTarget(string source1PngFullPath, string targetPngFullPath)
        {
            Assert.AreEqual(true, CSharpFileUtility.IsFileAreEqual(source1PngFullPath, targetPngFullPath));
        }

        #endregion
    }
}