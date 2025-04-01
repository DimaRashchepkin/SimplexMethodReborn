using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
	public partial class ConditionsUC : UserControl
	{
		public Storage storage = new Storage();
		private int I;
		private int J;
		private ObservableCollection<TableLine> function = new ObservableCollection<TableLine>();
		private ObservableCollection<TableLine> restrictions = new ObservableCollection<TableLine>();

		public ConditionsUC()
		{
			InitializeComponent();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			double[] resFunc = new double[this.J];
            double[] buf = function[0].GetTable();
			for (int i = 0; i < this.J - 1; i++)
			{
				resFunc[i] = buf[i];
			}
            resFunc[this.J - 1] = buf[buf.Length - 1];

			List<double[]> resRest = new List<double[]>();
			for (int i = 0; i < this.I - 1; i++)
			{
				resRest.Add(new double[this.J]);
                buf = restrictions[i].GetTable();
                for (int j = 0; j < this.J - 1; j++)
                {
                    resRest[i][j] = buf[j];
                }
                resRest[i][this.J - 1] = buf[buf.Length - 1];
            }

			storage.StartTable = new StartTable(this.I, this.J, resFunc, resRest);
			NewLabel.Content = "Conditions saved";
        }

		public void ShowTable()
		{
			GetDataFromST();
			VarComboBox.SelectedIndex = this.J - 2;
			RestComboBox.SelectedIndex = this.I - 2;
            FunctionDataGrid.ItemsSource = function;
            ConditionsDataGrid.ItemsSource = restrictions;
        }

		private void VarComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            for (int i = 0; i < ConditionsDataGrid.Columns.Count - 1; i++)
			{
				if (i < VarComboBox.SelectedIndex + 1)
				{
                    FunctionDataGrid.Columns[i].Visibility = Visibility.Visible;
                    ConditionsDataGrid.Columns[i].Visibility = Visibility.Visible;
                }
				else
				{
                    FunctionDataGrid.Columns[i].Visibility = Visibility.Hidden;
					ConditionsDataGrid.Columns[i].Visibility = Visibility.Hidden;
                }
			}

            this.J = VarComboBox.SelectedIndex + 2;
			NewLabel.Content = "";
        }

		private void RestComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.I < RestComboBox.SelectedIndex + 2)
			{
				for (int i = 0; i < RestComboBox.SelectedIndex + 2 - this.I; i++)
				{
                    restrictions.Add(new TableLine());
                }
			}
			else
			{
                for (int i = this.I - 1; i > RestComboBox.SelectedIndex + 1; i--)
                {
                    restrictions.RemoveAt(i - 1);
                }
            }

			this.I = RestComboBox.SelectedIndex + 2;
            NewLabel.Content = "";
        }

		private void GetDataFromST()
		{
			this.I = storage.StartTable.I;
            this.J = storage.StartTable.J;
			function.Clear();
            function.Add(new TableLine(GetLine(storage.StartTable.Function)));

			restrictions.Clear();
			for (int i = 0; i < this.I - 1; i++)
			{
				restrictions.Add(new TableLine(GetLine(storage.StartTable.Restrictions[i])));
			}
        }

		private double [] GetLine(double[] table)
		{
            double[] buf = new double[16];

            for (int i = 0; i < 15; i++)
            {
                if (i < this.J - 1)
                {
                    buf[i] = table[i];
                }
                else
                {
                    buf[i] = 0;
                }
            }
            buf[15] = table[this.J - 1];

			return buf;
        }
	}
}
