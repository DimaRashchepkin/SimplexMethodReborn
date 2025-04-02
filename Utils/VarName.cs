using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class VarName(int index)
    {
        public string X { get; set; } = "X" + index.ToString();
    }
}
