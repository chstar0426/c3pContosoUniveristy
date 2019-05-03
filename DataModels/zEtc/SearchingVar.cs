using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class SearchingVar
    {
        public bool SearchMode { get; set; }
        public string SearchField { get; set; }
        public string SearchQuery { get; set; }
        public Dictionary<string,string> DicField { get; set; }

    }
}
