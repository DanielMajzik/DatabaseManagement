using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement
{
    public class ProductModel
    {
        public string Name { get; set; }
        public int CategoryID { get; set; }

        public ProductModel(string name, int id)
        {
            Name = name;
            CategoryID = id;
        }
    }
}
