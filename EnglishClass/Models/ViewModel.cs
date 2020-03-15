using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishClass.Models
{
    public class ViewModel
    {
        public ViewModel(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public int ID { get; set; }
        public string Name { get; set; }
    }
}