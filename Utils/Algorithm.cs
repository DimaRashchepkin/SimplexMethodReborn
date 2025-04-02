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
		public List<Fraction[]> Table = [];

		public int M; // количество переменных + 1 на столбец констант
		public int N; // количество ограничений

		public List<int> Basis = []; //список базисных переменных
		public List<int> Free = []; // список свободных переменных

		private List<List<Fraction[]>> memoryTable = [];
		private List<List<int>> memoryBasis = [];
		private List<List<int>> memoryFree = [];

		public Algorithm() { }

		public void SimulatedBasis(List<Fraction[]> rest)
		{
			N = rest.Count;
			M = rest[0].Length;
			Table.Clear();
			Basis.Clear();
			Free.Clear();
			ClearMemory();

			for (int i = 0; i < N; i++)
			{
				Table.Add(new Fraction[M]);
                for (int j = 0; j < M; j++)
                {
                    Table[i][j] = 0;
                }
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

			Fraction[] buf = new Fraction[M];
            for (int j = 0; j < M; j++)
			{
                Fraction res = 0;
                for (int i = 0; i < N; i++)
				{
					res += Table[i][j];
				}
				buf[j] = res * -1;
			}

			Table.Add(buf);
		}

		public bool GaussBasis(List<Fraction[]> rest, List<int> indexes)
		{
			N = rest.Count;
			M = rest[0].Length - N;

            Table.Clear();
			Basis.Clear();
			Free.Clear();
			ClearMemory();
			
			for (int i = 0; i < N + 1; i++)
			{
				Table.Add(new Fraction[M]);
				for (int j = 0; j < M; j++)
				{
					Table[i][j] = 0;
				}
			}

			Basis = [.. indexes];
			for (int i = 0; i < rest[0].Length - 1; i++)
			{
				if (!Basis.Contains(i))
				{
					Free.Add(i);
				}
			}

			List<Fraction[]> buf = [];

            try
			{
				// buf = Gauss.Count(N, rest[0].Length, rest, indexes);
                buf = GaussNew.Count(rest, indexes);
            }
			catch (DivideByZeroException)
            {
				return false;
			}
            
            int k;
			for (int i = 0; i < buf.Count; i++)
			{
				k = 0;
				for (int j = 0; j < buf[i].Length; j++)
				{
					if (Free.Contains(j) || j == buf[i].Length - 1)
					{
						Table[i][k] = buf[i][j];
						k++;
					}
				}
			}

			return true;
		}

		public void NextStep(int mc, int mr, bool real)
		{
			List<Fraction[]> result = [];
			for (int i = 0; i < Table.Count; i++)
			{
				result.Add(new Fraction[Table[i].Length]);
			}
			int buf;

			memoryTable.Add([.. Table]);
			memoryBasis.Add([.. Basis]);
			memoryFree.Add([.. Free]);

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
					if (Free[i] >= M - 1)
					{
						index = i;
						Free.RemoveAt(i);
						break;
					}
				}

				for (int i = 0; i <= N; i++)
				{
					Fraction[] newLine = new Fraction[M - 1];
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
			Table = [.. memoryTable.Last()];
			N = Table.Count - 1;
			M = Table[0].Length;
			memoryTable.RemoveAt(memoryTable.Count - 1);

			Basis = [.. memoryBasis.Last()];
			memoryBasis.RemoveAt(memoryBasis.Count - 1);

			Free = [.. memoryFree.Last()];
			memoryFree.RemoveAt(memoryFree.Count - 1);
		}

		public void UpdateToMain(Fraction[] func)
		{
			memoryTable.Add([.. Table]);
			memoryBasis.Add([.. Basis]);
			memoryFree.Add([.. Free]);

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
                int buf = 0;
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
			List<int> cols = [];

			for (int j = 0; j < M - 1; j++)
				if (Table[N][j] < 0)
					cols.Add(j);

			return cols;
		}

		public List<int[]> FindMainRows(bool real)
		{
			List<int> cols = FindMainCols();
			List<int[]> result = [];
			int mainRow;

			foreach (int col in cols)
			{
				mainRow = -1;
				for (int i = 0; i < N; i++)
				{
					if (!real)
					{
						if (Basis[i] >= M - 1)
						{
							if (Table[i][col] > 0)
							{
								mainRow = i;
								break;
							}
						}
					}
					else
					{
						if (Table[i][col] > 0)
						{
							mainRow = i;
							break;
						}
					}
				}
				if (mainRow == -1)
				{
					return result;
				}

				for (int i = mainRow + 1; i < N; i++)
				{
					if (!real)
					{
						if (Basis[i] >= M - 1)
						{
							if ((Table[i][col] > 0) && ((Table[i][M - 1] / Table[i][col]) < (Table[mainRow][M - 1] / Table[mainRow][col])))
							{
								mainRow = i;
							}
						}
					}
					else
					{
						if ((Table[i][col] > 0) && ((Table[i][M - 1] / Table[i][col]) < (Table[mainRow][M - 1] / Table[mainRow][col])))
						{
							mainRow = i;
						}
					}
				}

				result.Add([col, mainRow]);
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
