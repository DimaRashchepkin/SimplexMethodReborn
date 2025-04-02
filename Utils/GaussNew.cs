using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class GaussNew
    {
        public static List<Fraction[]> Count(List<Fraction[]> data, List<int> columnIndices)
        {
            List<Fraction[]> matrix = [];
            for (int i = 0; i < data.Count; i++)
            {
                matrix.Add(new Fraction[data[0].Length]);
                for (int j = 0; j < data[0].Length; j++)
                {
                    matrix[i][j] = data[i][j];
                }
            }

            int rows = matrix.Count;
            if (rows == 0) return matrix;

            int cols = matrix[0].Length;
            int currentRow = 0;

            foreach (int col in columnIndices)
            {
                if (col >= cols) continue;

                // Поиск строки с максимальным элементом в текущем столбце
                int maxRow = currentRow;
                for (int i = currentRow + 1; i < rows; i++)
                {
                    if (Math.Abs((double)matrix[i][col]) > Math.Abs((double)matrix[maxRow][col]))
                    {
                        maxRow = i;
                    }
                }

                // Перестановка строк
                if (maxRow != currentRow)
                {
                    Fraction[] temp = matrix[currentRow];
                    matrix[currentRow] = matrix[maxRow];
                    matrix[maxRow] = temp;
                }

                // Проверка на ноль (если ведущий элемент нулевой, столбец пропускаем)
                if (Math.Abs((double)matrix[currentRow][col]) < 1e-10)
                {
                    continue;
                }

                // Нормализация текущей строки
                Fraction pivot = matrix[currentRow][col];
                for (int j = 0; j < cols; j++)
                {
                    matrix[currentRow][j] /= pivot;
                }

                // Обнуление других строк в текущем столбце
                for (int i = 0; i < rows; i++)
                {
                    if (i != currentRow)
                    {
                        Fraction factor = matrix[i][col];
                        for (int j = 0; j < cols; j++)
                        {
                            matrix[i][j] -= factor * matrix[currentRow][j];
                        }
                    }
                }

                currentRow++;
                if (currentRow == rows) break;
            }

            return matrix;
        }
    }
}
