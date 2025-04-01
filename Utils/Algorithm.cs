using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
	public class Algorithm
	{
		public List<double[]> Table = new List<double[]>();

		public int M; // количество переменных + 1 на столбец констант
		public int N; // количество ограничений

		public List<int> Basis = new List<int>(); //список базисных переменных
		public List<int> Free = new List<int>(); // список свободных переменных

		private List<List<double[]>> memoryTable = new List<List<double[]>>();
		private List<List<int>> memoryBasis = new List<List<int>>();
		private List<List<int>> memoryFree = new List<List<int>>();

		public Algorithm() { }

		public void SimulatedBasis(List<double[]> rest)
		{
			N = rest.Count;
			M = rest[0].Length;
            Table.Clear();
            Basis.Clear();
            Free.Clear();
            ClearMemory();

            for (int i = 0; i < N; i++)
			{
				Table.Add(new double[M]);
			}

			for (int i = M - 1; i < N + M - 1; i++)
			{
				Basis.Add(i);
			}

			for (int i = 0; i < M - 1; i++)
			{
				Free.Add(i);
			}

			for (int i = 0; i < N; i++)
			{
				for (int j = 0; j < M; j++)
				{
					Table[i][j] = rest[i][j];
				}
			}

			double[] buf = new double[M];
			double res = 0;
			for (int j = 0; j < M; j++)
			{
				res = 0;
				for (int i = 0; i < N; i++)
				{
					res += Table[i][j];
				}
				buf[j] = res * -1;
			}

			Table.Add(buf);
		}

		public void GaussBasis(List<double[]> rest, List<int> indexes)
		{
			N = rest.Count;
			M = rest[0].Length - N;
            List<double[]> buf = new List<double[]>();

            Table.Clear();
			Basis.Clear();
			Free.Clear();
			ClearMemory();
			
            for (int i = 0; i < N + 1; i++)
            {
                Table.Add(new double[M]);
            }

            Basis = indexes.ToList();
			for (int i = 0; i < rest[0].Length - 1; i++)
			{
				if (!Basis.Contains(i))
				{
					Free.Add(i);
				}
			}

            buf = Gauss.Count(N, rest[0].Length, rest, indexes);
			int k;
			for (int i = 0; i < buf.Count; i++)
			{
				k = 0;
				for (int j = 0; j < buf[i].Length; j++)
				{
					if (Free.Contains(j) || j == buf[i].Length - 1)
					{
						Table[i][k] = buf[i][j] / buf[i][Basis[i]];
						k++;
					}
				}
			}
        }

		public void NextStep(int mc, int mr, bool real)
		{
			List<double[]> result = new List<double[]>();
			for (int i = 0; i < Table.Count; i++)
			{
				result.Add(new double[Table[i].Length]);
			}
			int buf;

			memoryTable.Add(Table.ToList());
			memoryBasis.Add(Basis.ToList());
			memoryFree.Add(Free.ToList());

			buf = Basis[mr];
			Basis[mr] = Free[mc];
			Free[mc] = buf;

			result[mr][mc] = 1 / Table[mr][mc];

			for (int j = 0; j < M; j++)
			{
				if (j != mc)
				{
					result[mr][j] = result[mr][mc] * Table[mr][j];
				}
			}

			for (int i = 0; i <= N; i++)
			{
				if (i != mr)
				{
					result[i][mc] = result[mr][mc] * Table[i][mc] * -1;
				}
			}

			for (int i = 0; i <= N; i++)
			{
				if (i != mr)
				{
					for (int j = 0; j < M; j++)
					{
						if (j != mc)
						{
							result[i][j] = Table[i][j] - Table[i][mc] * result[mr][j];
						}
					}
				}
			}

			Table = result;
			if (!real)
			{
				int index = 0;
				for (int i = 0; i < Free.Count; i++)
				{
					if (Free[i] >= M)
					{
						index = i;
						Free.RemoveAt(i);
						break;
					}
				}

				for (int i = 0; i <= N; i++)
				{
					double[] newLine = new double[M - 1];
					for (int j = 0; j < M; j++)
					{
						if (j < index)
						{
							newLine[j] = Table[i][j];
						}
						else if (j > index) 
						{
							newLine[j - 1] = Table[i][j];
						}
					}

					Table[i] = newLine;
				}

				M--;
			}
		}

		public void PreviousStep()
		{
			Table = memoryTable.Last().ToList();
			N = Table.Count - 1;
			M = Table[0].Length;
			memoryTable.RemoveAt(memoryTable.Count - 1);

            Basis = memoryBasis.Last().ToList();
            memoryBasis.RemoveAt(memoryBasis.Count - 1);

            Free = memoryFree.Last().ToList();
            memoryFree.RemoveAt(memoryFree.Count - 1);
        }

		public void UpdateToMain(double[] func)
		{
            memoryTable.Add(Table.ToList());
            memoryBasis.Add(Basis.ToList());
            memoryFree.Add(Free.ToList());

            foreach (int i in Basis)
			{
				foreach (int j in Free)
				{
					Table[N][Free.IndexOf(j)] += func[i] * Table[Basis.IndexOf(i)][Free.IndexOf(j)] * -1;
				}
				Table[N][M - 1] += func[i] * Table[Basis.IndexOf(i)][M - 1];
			}
            foreach (int j in Free)
			{
				Table[N][Free.IndexOf(j)] += func[j];
			}
			Table[N][M - 1] += func.Last();
			Table[N][M - 1] *= -1;
        }

		public int IsItEnd() // 1 - решение получено, 0 - решение не получено, -1 - решений нет
		{
			int flag = 1;
			int buf = 0;

			for (int j = 0; j < M - 1; j++)
			{
				if (Table[N][j] < 0)
				{
					flag = 0;
					break;
				}
			}

			if (flag == 0)
			{
				buf = 0;
				for (int j = 0; j < M; j++)
				{
					if (Table[N][j] < 0)
					{
						for (int i = 0; i < N; i++)
						{
							if (Table[i][j] > 0)
							{
								buf = 1;
								break;
							}
						}

						if (buf == 0)
						{
							return -1;
						}
					}
				}
			}

			return flag;
		}

		private List<int> FindMainCols()
		{
			List<int> cols = new List<int>();

			for (int j = 0; j < M - 1; j++)
				if (Table[N][j] < 0)
					cols.Add(j);

			return cols;
		}

		public List<int[]> FindMainRows()
		{
			List<int> cols = FindMainCols();
			List<int[]> result = new List<int[]>();
			int mainRow = 0;

			foreach (int col in cols)
			{
				mainRow = 0;
				for (int i = 0; i < N; i++)
				{
					if (Table[i][col] > 0)
					{
						mainRow = i;
						break;
					}
				}

				for (int i = mainRow + 1; i < N; i++)
				{
					if ((Table[i][col] > 0) && ((Table[i][M - 1] / Table[i][col]) < (Table[mainRow][M - 1] / Table[mainRow][col])))
					{
						mainRow = i;
					}
				}

				result.Add(new int[] { col, mainRow });
			}

			return result;
		}

		public void ClearMemory()
		{
			memoryBasis.Clear();
			memoryTable.Clear();
			memoryFree.Clear();
		}
	}
}
