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
    public class FileOperation
    {
        public void InsertBaseXml(string filename, List<string> content)
        {
            if (File.Exists(filename))
                File.Delete(filename);

            var file =  File.Create(filename);
            file.Close();
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var item in content)
                {
                    sw.WriteLine(item);
                }

            }
        }

        public void InsertContent(string filename, string content)
        {
            if (File.Exists(filename))
            {
                Debug.WriteLine("Error! There is an item with the same name. Please try with different name.");
                return;
            }

            var file = File.Create(filename);
            file.Close();
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(content);
            }
        }

        public string ReadContent(string filename)
        {
            if (!File.Exists(filename))
                return string.Empty;
            using (StreamReader sr = new StreamReader(filename))
            {
                return sr.ReadToEnd();
            }
        }

        public List<String> ReadBaseXml(string filename)
        {
            List<String> result = new List<string>();
            if (!File.Exists(filename))
                return new List<string>();
            using (StreamReader sr = new StreamReader(filename))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    result.Add(line);
                }
            }
            return result;
        }

        public IEnumerable<RepositoryObject> DeserializeContent(List<string> content)
        {
            List<RepositoryObject> result = new List<RepositoryObject>();
            content.ForEach((p) =>
            {
                string[] split = p.Split('\t');
                var obj = new RepositoryObject();
                obj.guid = Guid.Parse(split[0]);
                obj.ItemName = split[1];
                obj.ItemType = Convert.ToInt32(split[2]);
                result.Add(obj);
            });
            return result;
        }

        public IEnumerable<string> SerializeContent(List<RepositoryObject> obj)
        {
            List<string> result = new List<string>();
            obj.ForEach((p) =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(p.guid);
                sb.Append('\t');
                sb.Append(p.ItemName);
                sb.Append('\t');
                sb.Append(p.ItemType);
                result.Add(sb.ToString());
            });
            return result;
        }
    }
}
