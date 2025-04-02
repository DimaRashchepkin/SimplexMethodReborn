using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class StartTable
    {
        public int I { get;} = 2;
        public int J { get;} = 2;

        public Fraction[] Function { get; } = [1, 1];

        public List<Fraction[]> Restrictions { get; } = [[1, 1]];

        public StartTable() { }

        public StartTable(int i, int j, Fraction[] function, List<Fraction[]> restrictions)
        {
            this.I = i;
            this.J = j;
            this.Function = function;
            this.Restrictions = restrictions;
        }
    }
}
