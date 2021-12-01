using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement
{
    public class WebshopModel
    {
        public string Name { get; set; }
        public string URL { get; set; }

        public WebshopModel(string name, string uRL)
        {
            Name = name;
            URL = uRL;
        }
    }
}
