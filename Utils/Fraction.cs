using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Fraction
    {
        int numerator;
        int denominator;

        public int Numerator { get; set; }

        public int Denominator { get; set; }

        public Fraction(int n, int d)
        {
            numerator = n;
            denominator = d;
        }

        public Fraction()
        {
            numerator = 1;
            denominator = 1;
        }

        public static int NOZ(int den0, int den1)
        {
            if (den1 % den0 == 0)
            {
                return den1;
            }

            int temp = 0;
            for (int i = 2; ; i++)
            {
                temp = den1 * i;
                if (temp % den0 == 0)
                {
                    break;
                }
            }

            return temp;
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            Fraction fraction = new Fraction(0, NOZ(a.Denominator, b.Denominator));

            fraction.numerator = fraction.denominator / a.Denominator * a.Numerator;
            fraction.numerator += fraction.denominator / b.Denominator * b.Numerator;

            return fraction;
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            Fraction fraction = new Fraction(0, NOZ(a.Denominator, b.Denominator));

            fraction.numerator = fraction.denominator / a.Denominator * a.Numerator;
            fraction.numerator -= fraction.denominator / b.Denominator * b.Numerator;

            return fraction;
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            Fraction fraction = new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
            return fraction;
        }

        public static Fraction operator *(Fraction a, int b)
        {
            Fraction fraction = new Fraction(a.numerator * b, a.denominator * b);
            return fraction;
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            Fraction fraction = new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);
            return fraction;
        }

        public static Fraction operator /(Fraction a, int b)
        {
            Fraction fraction = new Fraction(a.numerator, a.denominator * b);
            return fraction;
        }

        public void Reduce()
        {
            this.Numerator = this.Numerator > 0 ? this.Numerator : -this.Numerator;
            this.Denominator = this.Denominator > 0 ? this.Denominator : -this.Denominator;

            int maxval = Numerator > Denominator ? Numerator : Denominator;
            for (int i = maxval; i >= 2; maxval--)
            {
                if (Numerator % maxval == 0 && Denominator % maxval == 0)
                {
                    this.Numerator /= maxval;
                    this.Denominator /= maxval;
                    break;
                }
            }
        }
    }
}
