using TMXGlueLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TMXGlueTest
{
    
    
    /// <summary>
    ///This is a test class for TiledMapSaveTest and is intended
    ///to contain all TiledMapSaveTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TiledMapSaveTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ToCSVString
        ///</summary>
        [TestMethod()]
        public void ToCSVStringLayerPropertyTest()
        {
            var target = new TiledMapSave();
            const TiledMapSave.CSVPropertyType type = TiledMapSave.CSVPropertyType.Layer;
            target.layer = new mapLayer[2] {new mapLayer(), new mapLayer()};
            target.layer[0].properties = new List<property>
                {
                    new property() {name = "name1", value = "val"},
                    new property() {name = "name2", value = "val2"}
                };

            target.layer[0].name = "layer1";

            target.layer[1].properties = new List<property>
                {
                    new property() {name = "name1", value = "val"},
                    new property() {name = "name3", value = "val3"}
                };

            target.layer[1].name = "layer2";

            var expected = "Name (required),name2,name1,name3\r\nlayer1,\"val2\",\"val\",\r\nlayer2,,\"val\",\"val3\"\r\n"; // TODO: Initialize to an appropriate value
            var actual = target.ToCSVString(type, null);
            Assert.AreEqual(expected, actual);

            string layerName = "layer1";
            expected = "Name (required),name2,name1\r\nlayer1,\"val2\",\"val\"\r\n";
            actual = target.ToCSVString(type, layerName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BuildPropertyDictionaryConcurrently
        ///</summary>
        [TestMethod()]
        public void BuildPropertyDictionaryConcurrentlyTest()
        {
            IEnumerable<property> properties = null; // TODO: Initialize to an appropriate value
            IDictionary<string, string> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<string, string> actual;
            actual = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
