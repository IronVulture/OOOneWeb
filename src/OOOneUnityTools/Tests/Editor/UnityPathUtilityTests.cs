using NUnit.Framework;
using UnityEngine;

namespace OOOneUnityTools.Editor.Tests
{
    public class UnityPathUtilityTests
    {
        #region Private Variables

        private static string _childPath;
        private static string _fileName;

        #endregion

        #region Setup/Teardown Methods

        [SetUp]
        public void SetUp()
        {
            _childPath = "SDFJHKL/SDFjkl";
            _fileName = "asdfeervsdklfjol";
        }

        #endregion

        #region Test Methods

        [Test]
        public static void GetUnityAbsoluteFullPath()
        {
            var extension = "overrideController";
            var expected = $"{Application.dataPath}/{_childPath}/{_fileName}.{extension}";
            ShouldEqualResult(expected,
                UnityPathUtility.GetUnityAbsoluteFullPath(_childPath, _fileName, extension));
        }

        [Test]
        public static void GetUnityPath()
        {
            ShouldEqualResult("Assets", UnityPathUtility.GetUnityPath());
        }

        [Test]
        public static void GetUnityAbsolutePath()
        {
            ShouldEqualResult(Application.dataPath, UnityPathUtility.GetAbsolutePath());
        }

        [Test]
        public static void GetUnityFolderPath()
        {
            ShouldEqualResult("Assets/" + _childPath,
                UnityPathUtility.GetUnityFolderPath(_childPath));
        }

        [Test]
        public static void GetUnityAbsoluteFolderPath()
        {
            ShouldEqualResult($"{Application.dataPath}/{_childPath}",
                UnityPathUtility.GetUnityAbsoluteFolderPath(_childPath));
        }

        [Test]
        public static void GetUnityFullPath()
        {
            var extension = "overrideController";
            var expected = $"Assets/{_childPath}/{_fileName}.{extension}";
            ShouldEqualResult(expected,
                UnityPathUtility.GetUnityFullPath(_childPath, _fileName, extension));
        }

        #endregion

        #region Private Methods

        private static void ShouldEqualResult(string expected, string result)
        {
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}