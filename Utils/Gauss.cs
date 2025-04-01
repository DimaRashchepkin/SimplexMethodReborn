using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Gauss
    {
        private static List<double[]> result = new List<double[]>();

        public static List<double[]> Count(int n, int m, List<double[]> matrix, List<int> indexes)
        {
            result.Clear();
            for (int i = 0; i < n; i++)
            {
                result.Add(new double[m]);
                for (int j = 0; j < m; j++)
                {
                    result[i][j] = matrix[i][j];
                }
            }

            for (int i = 0; i < indexes.Count - 1; i++)
            {
                for (int j = i + 1; j < indexes.Count; j++)
                {
                    double koef = result[j][indexes[i]] / result[i][indexes[i]];
                    SubtractLine(m, j, i, koef);
                }
            }

            for (int i = indexes.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    double koef = result[j][indexes[i]] / result[i][indexes[i]];
                    SubtractLine(m, j, i, koef);
                }
            }

            return result;
        }

        private static void SubtractLine(int m, int k, int l, double koef)
        {
            for (int i = 0; i < m; i++)
            {
                result[k][i] -= result[l][i] * koef;
            }
        }
    }
}
