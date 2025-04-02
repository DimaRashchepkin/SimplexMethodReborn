using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
	public class FileWR(Storage storage)
    {
        private Storage Storage { get; set; } = storage;
        private static StreamReader reader;
		private static StreamWriter writer;
		public static int n, m;
		public static Fraction[] indexes = [];
		public static List<Fraction[]> matrix = [];

        public bool Read()
		{
			reader = new StreamReader(Storage.FilePath);
			try
			{
				GetMatrix(reader);
			}
			catch (Exception)
			{
				reader.Close();
				return false;
			}
			
			Storage.StartTable = new StartTable(n, m, indexes, matrix);

			reader.Close();
			return true;
		}

		public bool Write()
		{
			writer = new StreamWriter(Storage.FilePath);
			n = Storage.StartTable.I;
			m = Storage.StartTable.J;

            try
			{
				writer.Write(Storage.StartTable.I);
				writer.Write(" ");
				writer.Write(Storage.StartTable.J);
				writer.Write("\n");

				for (int i = 0; i < m; i++)
				{
					writer.Write(Storage.StartTable.Function[i]);
					writer.Write(" ");
				}
				writer.Write("\n");

				for (int i = 0; i < n - 1; i++)
				{
					for (int j = 0; j < m; j++)
					{
						writer.Write(Storage.StartTable.Restrictions[i][j]);
						writer.Write(" ");
					}
                    writer.Write("\n");
                }
			}
			catch (Exception)
			{
				writer.Close();
				return false;
			}

			writer.Close();
			return true;
		}

		private static void GetMatrix(StreamReader reader)
		{
			Fraction[] buffer;

			buffer = GetFractionLine(reader);
			if (buffer.Length != 2)
			{
				throw new FormatException("Wrong format");
			}
			
			n = buffer[0].Numerator;
			m = buffer[1].Numerator;

            indexes = GetFractionLine(reader);
            if (indexes.Length != m)
            {
                throw new FormatException("Wrong format: wrong indexes");
            }

			matrix = [];
			for (int i = 0; i < n - 1; i++)
			{
                buffer = GetFractionLine(reader);
                if (buffer.Length != m)
                {
                    throw new FormatException("Wrong indexes");
                }

				matrix.Add(new Fraction[m]);
                for (int j = 0; j < m; j++)
				{
					matrix[i][j] = buffer[j];
				}
			}
		}

		private static Fraction[] GetFractionLine(StreamReader reader)
		{
			int i;
			Fraction buf;
			Fraction[] result;
			string str = reader.ReadLine() ?? throw new FormatException("Wrong format: str null");
            str = str.Trim();
			string[] strs = str.Split(' ');

			result = new Fraction[strs.Length];
			for (i = 0; i < strs.Length; i++)
			{
				try
				{
					buf = Fraction.Parse(strs[i]);
				}
				catch (FormatException)
				{
					throw;
				}
				result[i] = buf;
			}

			return result;
		}
	}
}
