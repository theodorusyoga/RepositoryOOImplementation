using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulatrixOOTest
{
    interface IRepository
    {
        void Register(string itemName, string itemContent, int itemType);
        string Retrieve(string itemName);
        int GetType(string itemName);
        void Deregister(string itemName);
    }
}
