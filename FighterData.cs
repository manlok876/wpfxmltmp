using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WPF_XML_FL
{
    class FighterData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Club { get; set; }
        public string Phone { get; set; }
        public DateTime Birthdate { get; set; }

        public FighterData()
        {
            Name = "No Name";
            Email = "no@mail.com";
            City = "Nowhere";
            Club = "-";
            Phone = "8 800 555 35 35";
            Birthdate = new DateTime(2000, 1, 1);
        }
    }
}
