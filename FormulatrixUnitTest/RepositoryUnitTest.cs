    using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulatrixOOTest;

namespace FormulatrixUnitTest
{
    [TestClass]
    public class RepositoryUnitTest
    {
        public Repository repo;
        public FileOperation fileOp;
        [TestMethod]
        public void EmptyRetrieveTest()
        {
            repo = new Repository();
            string result = repo.Retrieve("item1");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void EmptyDeregisterTest()
        {
            repo = new Repository();
            repo.Deregister("item1");
            string result = repo.Retrieve("item1");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void EmptyGetTypeTest()
        {
            repo = new Repository();
            int itemType = repo.GetType("item1");
            Assert.AreEqual(0, itemType);
        }

        [TestMethod]
        public void RegisterXmlTest()
        {
            repo = new Repository();
            repo.Register("item1", "<students><student id=\"001\"><name>Budi</name><address>Kaliurang St.</address></student><student id=\"002\"><name>Tuti</name><address>Magelang St.</address></student></students>",
                2);

            //validate content
            string item1 = repo.Retrieve("item1");
            //remove unimportant string in the start and end
            item1 = item1.Trim();
            Assert.AreEqual("<students><student id=\"001\"><name>Budi</name><address>Kaliurang St.</address></student><student id=\"002\"><name>Tuti</name><address>Magelang St.</address></student></students>", item1, true);
        }

        [TestMethod]
        public void RegisterJsonTest()
        {
            repo = new Repository();
            repo.Register("item10", "{ \"students\" : [{ \"id\" : 001, \"name\" : \"Budi\", \"address\": \"Kaliurang St.\"}, {\"id\" : 002, \"name\" : \"Tuti\", \"address\": \"Magelang St.\"}]}",
                2);

            //validate content
            string item2 = repo.Retrieve("item10");
            //remove unimportant string
            item2 = item2.Trim();
            Assert.AreEqual("{ \"students\" : [{ \"id\" : 001, \"name\" : \"Budi\", \"address\": \"Kaliurang St.\"}, {\"id\" : 002, \"name\" : \"Tuti\", \"address\": \"Magelang St.\"}]}", item2);
        }

        [TestMethod]
        public void RegisterErrorItemTypeTest()
        {
            repo = new Repository();
            repo.Register("item3", "{ \"students\" : [{ \"id\" : 001, \"name\" : \"Budi\", \"address\": \"Kaliurang St.\"}, {\"id\" : 002, \"name\" : \"Tuti\", \"address\": \"Magelang St.\"}]}",
                2);

            //validate content
            //item3 is registered, but converted into type 1 (JSON).
            string item3 = repo.Retrieve("item3");
            //remove unimportant string
            item3 = item3.Trim();
            Assert.AreEqual("{ \"students\" : [{ \"id\" : 001, \"name\" : \"Budi\", \"address\": \"Kaliurang St.\"}, {\"id\" : 002, \"name\" : \"Tuti\", \"address\": \"Magelang St.\"}]}", item3);
            int item3Type = repo.GetType("item3");
            Assert.AreEqual(1, item3Type);
        }

        [TestMethod]
        public void RegisterErrorSchemaTest()
        {
            repo = new Repository();
            repo.Register("item4", "{ \"stude: 001, \"name\" : \"Budi\", \"address\": \"Kaliurang St.\"}, {\"id\" : 002, \"name\" : \"Tuti\", \"address\": \"Magelang St.\"}]}",
                1);

            //validate content
            //schema error causes the content for not to be registered
            string item4 = repo.Retrieve("item4");
            Assert.AreEqual(string.Empty, item4);
        }

        [TestMethod]
        public void RetrieveTest()
        {
            repo = new Repository();
            string result = repo.Retrieve("item1");
            result = result.Trim();
            Assert.AreEqual("<students><student id=\"001\"><name>Budi</name><address>Kaliurang St.</address></student><student id=\"002\"><name>Tuti</name><address>Magelang St.</address></student></students>", result);
        }

        [TestMethod]
        public void DeregisterTest()
        {
            repo = new Repository();
            repo.Deregister("item3");

            //validate content
            string item3 = repo.Retrieve("item3");
            Assert.AreEqual(string.Empty, item3);
        }

        [TestMethod]
        public void XmlGetTypeTest()
        {
            repo = new Repository();
            int itemType = repo.GetType("item1");
            Assert.AreEqual(2, itemType);
        }

        [TestMethod]
        public void JsonGetTypeTest()
        {
            repo = new Repository();
            int itemType = repo.GetType("item2");
            Assert.AreEqual(1, itemType);
        }
        
    }
}
