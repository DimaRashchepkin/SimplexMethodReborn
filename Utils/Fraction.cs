using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Fraction : IComparable<Fraction>, IEquatable<Fraction>
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));

            int gcd = GCD(Math.Abs(numerator), Math.Abs(denominator));
            Numerator = numerator / gcd;
            Denominator = denominator / gcd;

            if (Denominator < 0)
            {
                Numerator *= -1;
                Denominator *= -1;
            }
        }

        public static Fraction Parse(string s)
        {
            if (TryParse(s, out Fraction result))
            {
                return result;
            }
            throw new FormatException($"Invalid fraction format: '{s}'");
        }

        public static bool TryParse(string s, out Fraction fraction)
        {
            fraction = null;
            if (string.IsNullOrWhiteSpace(s))
                return false;

            s = s.Replace(" ", "").Replace(",", ".");

            string[] fractionParts = s.Split('/');
            if (fractionParts.Length == 2)
            {
                if (int.TryParse(fractionParts[0], out int numerator) &&
                    int.TryParse(fractionParts[1], out int denominator) && denominator != 0)
                {
                    fraction = new Fraction(numerator, denominator);
                    return true;
                }
                return false;
            }
            else if (fractionParts.Length == 1)
            {
                if (s.Contains('.'))
                {
                    if (TryParseDecimal(s, out fraction))
                        return true;
                }

                if (int.TryParse(s, out int numerator))
                {
                    fraction = new Fraction(numerator, 1);
                    return true;
                }
            }

            return false;
        }

        private static bool TryParseDecimal(string s, out Fraction fraction)
        {
            fraction = null;

            string[] decimalParts = s.Split('.');
            if (decimalParts.Length != 2)
                return false;

            if (!int.TryParse(decimalParts[0], out int integerPart))
                return false;

            string fractionalPart = decimalParts[1].TrimEnd('0');
            if (fractionalPart.Length == 0)
            {
                fraction = new Fraction(integerPart, 1);
                return true;
            }

            if (!int.TryParse(fractionalPart, out int numerator))
                return false;

            int denominator = (int)Math.Pow(10, fractionalPart.Length);
            int sign = Math.Sign(integerPart);

            if (integerPart != 0)
            {
                fraction = new Fraction(Math.Abs(integerPart) * denominator + numerator, denominator) * sign;
            }
            else
            {
                fraction = new Fraction(numerator * sign, denominator);
            }

            return true;
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            int newNumerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
            int newDenominator = a.Denominator * b.Denominator;
            return new Fraction(newNumerator, newDenominator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            int newNumerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
            int newDenominator = a.Denominator * b.Denominator;
            return new Fraction(newNumerator, newDenominator);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            int newNumerator = a.Numerator * b.Numerator;
            int newDenominator = a.Denominator * b.Denominator;
            return new Fraction(newNumerator, newDenominator);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.Numerator == 0)
                throw new DivideByZeroException("Cannot divide by zero.");

            int newNumerator = a.Numerator * b.Denominator;
            int newDenominator = a.Denominator * b.Numerator;
            return new Fraction(newNumerator, newDenominator);
        }

        public static Fraction operator -(Fraction a)
        {
            return new Fraction(-a.Numerator, a.Denominator);
        }

        public static bool operator ==(Fraction a, Fraction b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Numerator == b.Numerator && a.Denominator == b.Denominator;
        }

        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }

        public static bool operator <(Fraction a, Fraction b)
        {
            if (a is null || b is null) return false;
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(Fraction a, Fraction b)
        {
            if (a is null || b is null) return false;
            return a.CompareTo(b) > 0;
        }

        public static bool operator <=(Fraction a, Fraction b)
        {
            if (a is null || b is null) return false;
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >=(Fraction a, Fraction b)
        {
            if (a is null || b is null) return false;
            return a.CompareTo(b) >= 0;
        }

        public int CompareTo(Fraction other)
        {
            if (other is null) return 1;

            long crossProduct1 = (long)this.Numerator * other.Denominator;
            long crossProduct2 = (long)other.Numerator * this.Denominator;

            return crossProduct1.CompareTo(crossProduct2);
        }

        public bool Equals(Fraction other)
        {
            if (other is null) return false;
            return this.Numerator == other.Numerator && this.Denominator == other.Denominator;
        }

        public override bool Equals(object obj)
        {
            if (obj is Fraction other)
            {
                return this == other;
            }
            return false;
        }

        public override string ToString()
        {
            if (Denominator == 1)
                return Numerator.ToString();
            return $"{Numerator}/{Denominator}";
        }

        private static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static implicit operator Fraction(int number)
        {
            return new Fraction(number, 1);
        }

        public static explicit operator double(Fraction fraction)
        {
            return (double)fraction.Numerator / fraction.Denominator;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
