using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormulatrixOOTest
{
    public class Repository : IRepository
    {
        private string path = Directory.GetCurrentDirectory();
        private FileOperation fileOp;
        private Validation val;
        public Repository()
        {
            this.fileOp = new FileOperation();
            this.val = new Validation();

            if (!Directory.Exists(path + "/XmlFiles"))
                Directory.CreateDirectory(path + "/XmlFiles");
            if (!File.Exists(path + "/XmlFiles/baseXml.xml"))
            {
                var file = File.Create(path + "/XmlFiles/baseXml.xml");
                file.Close();
            }
        }

        public void Deregister(string itemName)
        {
            var contents = fileOp.ReadBaseXml(path + "/XmlFiles/baseXml.xml");
            var deserialized = fileOp.DeserializeContent(contents).ToList();

            var items = deserialized.Where(p => p.ItemName == itemName).ToList();
            if (items.Count != 0)
            {
                deserialized.Remove(items.FirstOrDefault());
                var serialize = fileOp.SerializeContent(deserialized).ToList();
                fileOp.InsertBaseXml(path + "/XmlFiles/baseXml.xml", serialize);
                File.Delete(path + "/XmlFiles/" + items.FirstOrDefault().guid + ".xml");
            }
            else
                Debug.WriteLine("No match on given item name!");
        }

        public int GetType(string itemName)
        {
            var contents = fileOp.ReadBaseXml(path + "/XmlFiles/baseXml.xml");
            var deserialized = fileOp.DeserializeContent(contents).ToList();

            var items = deserialized.Where(p => p.ItemName == itemName);
            if (items.Count() != 0)
                return items.FirstOrDefault().ItemType;
            else
            {
                Debug.WriteLine("No match on given item name!");
                return 0;
            }
        }


        public void Register(string itemName, string itemContent, int itemType)
        {
            var contents = fileOp.ReadBaseXml(path + "/XmlFiles/baseXml.xml");
            var deserialized = fileOp.DeserializeContent(contents).ToList();
            if (deserialized.Where(p => p.ItemName == itemName).Count() == 0)
            {
                if (val.IsSchemaValid(itemContent, ref itemType))
                {
                    Guid newid = Guid.NewGuid();
                    deserialized.Add(new RepositoryObject()
                    {
                        guid = newid,
                        ItemName = itemName,
                        ItemType = itemType
                    });
                    var serialized = fileOp.SerializeContent(deserialized).ToList();
                    fileOp.InsertBaseXml(path + "/XmlFiles/baseXml.xml", serialized);
                    fileOp.InsertContent(path + "/XmlFiles/" + newid + ".xml", itemContent);
                }
                else
                {
                    Debug.WriteLine("The given content is neither XML or JSON! Please correct your content format and try again.");
                }
            }
            else
                Debug.WriteLine("Item with the name " + itemName + " is already registered! Please choose another name!");
        }

        public string Retrieve(string itemName)
        {
            var contents = fileOp.ReadBaseXml(path + "/XmlFiles/baseXml.xml");
            var deserialized = fileOp.DeserializeContent(contents).ToList();

            var items = deserialized.Where(p => p.ItemName == itemName);
            if(items.Count() != 0)
            {
                return fileOp.ReadContent(path + "/XmlFiles/" + items.FirstOrDefault().guid + ".xml");
            }
            else
            {
                Debug.WriteLine("No match on given item name!");
                return string.Empty;
            }
        }
    }
}
