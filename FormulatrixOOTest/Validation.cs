using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;

namespace FormulatrixOOTest
{
    public class Validation
    {
        private int iterationNo;

        /// <summary>
        /// Check if schema of itemContent is valid on given itemType (1 for JSON and 2 for XML). This method also returns
        /// suggested itemType on specific given itemContent.
        /// </summary>
        /// <param name="itemContent"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public bool IsSchemaValid(string itemContent, ref int itemType)
        {
            if (String.IsNullOrEmpty(itemContent))
                return false;

            //check against 1 = JSON
            if(itemType == 1)
            {
                try
                {
                    JavaScriptSerializer jcs = new JavaScriptSerializer();
                    jcs.Deserialize<dynamic>(itemContent);
                    return true;
                }
                catch
                {
                    //check if iteration less than 2 times (XML and JSON once in each time)
                    if (iterationNo < 2)
                    {
                        //check against XML
                        itemType = 2;
                        iterationNo++;
                        return this.IsSchemaValid(itemContent, ref itemType);
                    }
                }
            }
            //check against 2 = XML
            else if(itemType == 2)
            {
                try
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(itemContent);
                    return true;
                }
              catch
                {
                    //check if iteration less than 2 times (XML and JSON once in each time)
                    if (iterationNo < 2)
                    {
                        //check against JSON
                        itemType = 1;
                        iterationNo++;
                        return this.IsSchemaValid(itemContent, ref itemType);
                    }
                    else return false;
                }
            }

            return false;
        }
    }
}
