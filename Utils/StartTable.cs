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

        public double[] Function { get; } = new double[] {1, 1};

        public List<double[]> Restrictions { get; } = new List<double[]> { new double[] {1, 1}};

        public StartTable() { }

        public StartTable(int i, int j, double[] function, List<double[]> restrictions)
        {
            this.I = i;
            this.J = j;
            this.Function = function;
            this.Restrictions = restrictions;
        }
    }

    public class TableLine
    {
        public double X1 { get; set; } = 0;
        public double X2 { get; set; } = 0;
        public double X3 { get; set; } = 0;
        public double X4 { get; set; } = 0;
        public double X5 { get; set; } = 0;
        public double X6 { get; set; } = 0;
        public double X7 { get; set; } = 0;
        public double X8 { get; set; } = 0;
        public double X9 { get; set; } = 0;
        public double X10 { get; set; } = 0;
        public double X11 { get; set; } = 0;
        public double X12 { get; set; } = 0;
        public double X13{ get; set; } = 0;
        public double X14 { get; set; } = 0;
        public double X15 { get; set; } = 0;
        public double C { get; set; } = 0;

        public TableLine() { }

        public TableLine(double [] values)
        {
            this.X1 = values[0];
            this.X2 = values[1];
            this.X3 = values[2];
            this.X4 = values[3];
            this.X5 = values[4];
            this.X6 = values[5];
            this.X7 = values[6];
            this.X8 = values[7];
            this.X9 = values[8];
            this.X10 = values[9];
            this.X11 = values[10];
            this.X12 = values[11];
            this.X13 = values[12];
            this.X14 = values[13];
            this.X15 = values[14];
            this.C = values[15];
        }

        public double [] GetTable()
        {
            double[] result = new double [16];
            result[0] = this.X1; 
            result[1] = this.X2; 
            result[2] = this.X3; 
            result[3] = this.X4; 
            result[4] = this.X5; 
            result[5] = this.X6; 
            result[6] = this.X7; 
            result[7] = this.X8;
            result[8] = this.X9;
            result[9] = this.X10;
            result[10] = this.X11; 
            result[11] = this.X12; 
            result[12] = this.X13; 
            result[13] = this.X14;
            result[14] = this.X15;
            result[15] = this.C;

            return result;
        }

        // public override string ToString()
        // {
        //     return X1.ToString() + X2.ToString() + X3.ToString();
        // }
    }

    public class VarName
    {
        public string X { get; set; } = "";

        public VarName(int index)
        {
            X = "X" + index.ToString();
        }
    }
}
