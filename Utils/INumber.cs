using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface INumber<T> where T : INumber<T>
    {
        static abstract T operator +(T value);
        static abstract T operator -(T value);

        static abstract T operator +(T left, T right);
        static abstract T operator -(T left, T right);
        static abstract T operator *(T left, T right);
        static abstract T operator /(T left, T right);
        static abstract T operator %(T left, T right);

        static abstract bool operator ==(T left, T right);
        static abstract bool operator !=(T left, T right);
        static abstract bool operator <(T left, T right);
        static abstract bool operator >(T left, T right);
        static abstract bool operator <=(T left, T right);
        static abstract bool operator >=(T left, T right);
    }
}
