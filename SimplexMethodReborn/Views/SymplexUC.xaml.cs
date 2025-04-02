﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utils;

namespace Symplex_method.Views
{
	public partial class SymplexUC : UserControl
	{
		public Storage storage = new Storage();
		private int N;
		private int M;
		private ObservableCollection<TableLine> symplexTable = new ObservableCollection<TableLine>();
        private ObservableCollection<VarName> basisTable = new ObservableCollection<VarName>();
        private bool tableDone = false; // false - вспомогательная задача
		private bool readyToUpdate = false; // при нажатии обновления получим основную задачу с базисом, а не сбросим прогресс 
		private bool isEnd = false;
		private int counter = 0;
        private int mainRow;
		private int mainCol;
		private List<int[]> mainCells;
		private ObservableCollection<string> cellStrings = new ObservableCollection<string>();

        public SymplexUC()
		{
			InitializeComponent();
            ForwardButton.IsEnabled = false;
            SolveButton.IsEnabled = false;
            BackButton.IsEnabled = false;
        }

		private void ShowButton_Click(object sender, RoutedEventArgs e)
		{
            if (readyToUpdate)
			{
				readyToUpdate = false;
				storage.Algo.UpdateToMain(storage.StartTable.Function);
				CheckEnd();
				counter = 0;
				storage.Algo.ClearMemory();
                BackButton.IsEnabled = false;
            }
            else
            {
                tableDone = false;
                if ((bool)SimulatedBasisButton.IsChecked)
                {
                    tableDone = false;
                    storage.Algo.SimulatedBasis(storage.StartTable.Restrictions);
                }
                else
                {
					int counter = 0;
					List<int> indexes = new List<int>();

					foreach (CheckBox box in BasisListBox.Items)
					{
						if (box.IsChecked == true)
						{
							counter++;
							indexes.Add(Convert.ToInt16(box.Content.ToString().Substring(1)) - 1);
						}
					}

					if (counter != storage.StartTable.Restrictions.Count)
					{
						ResultLabel.Content = "Неверное число базисных переменных";
						return;
					}
					else
					{
						storage.Algo.GaussBasis(storage.StartTable.Restrictions, indexes);
						storage.Algo.UpdateToMain(storage.StartTable.Function);
						tableDone = true;
                    }
                }
            }

            ForwardButton.IsEnabled = true;
            SolveButton.IsEnabled = true;
            ResultLabel.Content = "";
            GetDataFromSymTable();
			STableDataGrid.ItemsSource = symplexTable;
			BasisDataGrid.ItemsSource = basisTable;
            HideExtraColumns();
			MainCellComboBox.ItemsSource = cellStrings;
            mainCells = storage.Algo.FindMainRows(tableDone);
            EnableMainCells();
            CheckEnd();
        }

        private void GaussButton_Click(object sender, RoutedEventArgs e)
        {
			ObservableCollection<CheckBox> basisButtons = new ObservableCollection<CheckBox>();
            BasisListBox.IsEnabled = true;
			BasisListBox.ItemsSource = basisButtons;
			for (int i = 1; i < storage.StartTable.Function.Length; i++)
			{
				CheckBox x = new CheckBox();
				x.Content = "X" + i.ToString();
				basisButtons.Add(x);
			}
        }

        private void SimulatedBasisButton_Click(object sender, RoutedEventArgs e)
        {
            BasisListBox.IsEnabled = false;
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
		{
			while (!isEnd)
			{
				ForwardButton_Click(sender, e);
			}
		}

		private void ForwardButton_Click(object sender, RoutedEventArgs e)
		{
            mainCol = mainCells[MainCellComboBox.SelectedIndex][0];
            mainRow = mainCells[MainCellComboBox.SelectedIndex][1];
            storage.Algo.NextStep(mainCol, mainRow, tableDone);

            GetDataFromSymTable();
            HideExtraColumns();

            mainCells = storage.Algo.FindMainRows(tableDone);
			EnableMainCells();
			isEnd = CheckEnd();

            BackButton.IsEnabled = true;
			counter += 1;
        }

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
            counter -= 1;
			if (counter == 0)
			{
				BackButton.IsEnabled = false;
			}
			if (tableDone && readyToUpdate)
			{
                tableDone = false;
                readyToUpdate = false;
            }
			else if (tableDone)
			{
				isEnd = false;
			}

			ForwardButton.IsEnabled = true;
            SolveButton.IsEnabled = true;
            ResultLabel.Content = "";

            storage.Algo.PreviousStep();
            GetDataFromSymTable();
            HideExtraColumns();

            mainCells = storage.Algo.FindMainRows(tableDone);
            EnableMainCells();
        }

		private void GetDataFromSymTable()
		{
			this.N = storage.Algo.N;
			this.M = storage.Algo.M;
			
			symplexTable.Clear();
			for (int i = 0; i <= this.N; i++)
			{
				symplexTable.Add(new TableLine(GetLine(storage.Algo.Table[i])));
			}

			basisTable.Clear();
			for (int i = 0; i < storage.Algo.Basis.Count; i++) 
			{
				basisTable.Add(new VarName(storage.Algo.Basis[i] + 1));
			}
		}

		private double[] GetLine(double[] table)
		{
			double[] buf = new double[16];

			for (int i = 0; i < 15; i++)
			{
				if (storage.Algo.Free.Contains(i))
				{
					buf[i] = table[storage.Algo.Free.IndexOf(i)];
				}
				else
				{
					buf[i] = 0;
				}
			}
			buf[15] = table[this.M - 1];

			return buf;
		}

		private void HideExtraColumns()
		{
            for (int i = 0; i < STableDataGrid.Columns.Count - 1; i++)
            {
                if (storage.Algo.Free.Contains(i))
                {
                    STableDataGrid.Columns[i].Visibility = Visibility.Visible;
                }
                else
                {
                    STableDataGrid.Columns[i].Visibility = Visibility.Hidden;
                }
            }
        }

		private void EnableMainCells() 
		{
            cellStrings.Clear();
            for (int i = 0; i < mainCells.Count; i++)
			{
				cellStrings.Add("(" + mainCells[i][0].ToString() + "; " + mainCells[i][1].ToString() + ")");
			}
			MainCellComboBox.SelectedIndex = 0;
        }

		private void CountCornerPoint()
		{
			List<double> result = new List<double>();
			for (int i = 0; i < M + N - 1; i++) 
			{
				if (storage.Algo.Basis.Contains(i))
				{
					result.Add(storage.Algo.Table[storage.Algo.Basis.IndexOf(i)][M - 1]);
				}
				else
				{
					result.Add(0);
				}
			}

			CornerPointLabel.Content = "(";
			foreach (double value in result)
			{
				CornerPointLabel.Content += value.ToString() + "; ";
			}
            CornerPointLabel.Content += ")";
        }

        private void MainCellComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			CountCornerPoint();
        }

		private bool CheckEnd()
		{
            int isEnd = storage.Algo.IsItEnd();
            if (isEnd != 0)
            {
                if (isEnd == 1)
                {
                    if (tableDone == false)
                    {
                        if (storage.Algo.Table[N][M - 1] != 0)
                        {
                            ResultLabel.Content = "Решений нет";
                            ForwardButton.IsEnabled = false;
                            SolveButton.IsEnabled = false;
                        }
                        else
                        {
                            ResultLabel.Content = "Базис найден. Обновите таблицу";
                            ForwardButton.IsEnabled = false;
                            SolveButton.IsEnabled = false;
                            readyToUpdate = true;
                        }

                        tableDone = true;
                    }
                    else
                    {
                        ResultLabel.Content = "Ответ: f*(x) = " + (storage.Algo.Table[N][M - 1] * -1).ToString();
                        ForwardButton.IsEnabled = false;
                        SolveButton.IsEnabled = false;
                    }
                }
                else
                {
                    ResultLabel.Content = "Решений нет";
                    ForwardButton.IsEnabled = false;
                    SolveButton.IsEnabled = false;
                }

                return true;
            }

			return false;
        }
    }
}
