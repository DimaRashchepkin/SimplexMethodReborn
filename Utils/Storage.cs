using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Storage
    {
        public String FilePath { get; set; } = "";
        public StartTable StartTable { get; set; } = new StartTable();
        public List<Fraction[]> SimplexTable { get; set; } = [];

        public Algorithm Algo = new();
    }
}
