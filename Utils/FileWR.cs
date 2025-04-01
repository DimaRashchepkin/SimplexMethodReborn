using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
	public class FileWR
	{
		private Storage Storage { get; set; } = new Storage();
		private static StreamReader reader;
		private static StreamWriter writer;
		public static int n, m;
		public static double[] indexes;
		public static List<double[]> matrix;

		public FileWR(Storage storage)
		{
			this.Storage = storage;
		}

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
			double[] buffer;

			buffer = GetDoubleLine(reader);
			if (buffer.Length != 2)
			{
				throw new FormatException("Wrong format");
			}
			
			n = Convert.ToInt16(buffer[0]);
			m = Convert.ToInt16(buffer[1]);

            indexes = GetDoubleLine(reader);
            if (indexes.Length != m)
            {
                throw new FormatException("Wrong format: wrong indexes");
            }

			matrix = new List<double[]>();
			for (int i = 0; i < n - 1; i++)
			{
                buffer = GetDoubleLine(reader);
                if (buffer.Length != m)
                {
                    throw new FormatException("Wrong indexes");
                }

				matrix.Add(new double[m]);
                for (int j = 0; j < m; j++)
				{
					matrix[i][j] = buffer[j];
				}
			}
		}

		private static double[] GetDoubleLine(StreamReader reader)
		{
			int i;
			double buf;
			double[] result;
			string str = reader.ReadLine();
			if (str == null)
			{
				throw new FormatException("Wrong format: str null");
			}
			str = str.Trim();
			string[] strs = str.Split(' ');

			result = new double[strs.Length];
			for (i = 0; i < strs.Length; i++)
			{
				try
				{
					buf = Convert.ToDouble(strs[i]);
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
