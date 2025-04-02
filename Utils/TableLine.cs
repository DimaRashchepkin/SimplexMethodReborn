using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class TableLine
    {
        public String X1 { get; set; } = "0";
        public String X2 { get; set; } = "0";
        public String X3 { get; set; } = "0";
        public String X4 { get; set; } = "0";
        public String X5 { get; set; } = "0";
        public String X6 { get; set; } = "0";
        public String X7 { get; set; } = "0";
        public String X8 { get; set; } = "0";
        public String X9 { get; set; } = "0";
        public String X10 { get; set; } = "0";
        public String X11 { get; set; } = "0";
        public String X12 { get; set; } = "0";
        public String X13 { get; set; } = "0";
        public String X14 { get; set; } = "0";
        public String X15 { get; set; } = "0";
        public String C { get; set; } = "0";

        public TableLine() { }

        public TableLine(Fraction[] values)
        {
            this.X1 = values[0].ToString();
            this.X2 = values[1].ToString();
            this.X3 = values[2].ToString();
            this.X4 = values[3].ToString();
            this.X5 = values[4].ToString();
            this.X6 = values[5].ToString();
            this.X7 = values[6].ToString();
            this.X8 = values[7].ToString();
            this.X9 = values[8].ToString();
            this.X10 = values[9].ToString();
            this.X11 = values[10].ToString();
            this.X12 = values[11].ToString();
            this.X13 = values[12].ToString();
            this.X14 = values[13].ToString();
            this.X15 = values[14].ToString();
            this.C = values[15].ToString();
        }

        public Fraction[] GetTable()
        {
            return 
            [
                Fraction.Parse(X1),
                Fraction.Parse(X2),
                Fraction.Parse(X3),
                Fraction.Parse(X4),
                Fraction.Parse(X5),
                Fraction.Parse(X6),
                Fraction.Parse(X7),
                Fraction.Parse(X8),
                Fraction.Parse(X9),
                Fraction.Parse(X10),
                Fraction.Parse(X11),
                Fraction.Parse(X12),
                Fraction.Parse(X13),
                Fraction.Parse(X14),
                Fraction.Parse(X15),
                Fraction.Parse(C),
            ];
        }
    }
}
