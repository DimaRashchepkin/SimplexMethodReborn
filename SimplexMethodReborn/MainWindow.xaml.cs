using Microsoft.Win32;
using Simplex_method.Views;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
using System.Windows.Shapes;
using Utils;

namespace Simplex_method
{
	public partial class MainWindow : Window
	{
        private readonly Storage storage;
        private readonly FileWR fileWR;
        private readonly ConditionsUC conditionsUC = new();
        private readonly SimplexUC simplexUC = new();

        public MainWindow()
		{
			InitializeComponent();
            storage = new Storage();
            fileWR = new FileWR(storage);
            conditionsUC.storage = storage;
            simplexUC.storage = storage;
            ConditionsButton_Click(new(), new());
        }

		private void ConditionsButton_Click(object sender, RoutedEventArgs e)
		{
			ConditionsButton.BorderBrush = SystemColors.ActiveBorderBrush;
			SymplexMethodButton.BorderBrush = SystemColors.InactiveBorderBrush;

            this.OutputView.Content = conditionsUC;
            conditionsUC.ShowTable();
        }

		private void SimplexMethodButton_Click(object sender, RoutedEventArgs e)
		{
			ConditionsButton.BorderBrush = SystemColors.InactiveBorderBrush;
			SymplexMethodButton.BorderBrush = SystemColors.ActiveBorderBrush;

            this.OutputView.Content = simplexUC;
            // simplexUC.ShowTable();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == true)
            {
                storage.FilePath = openFileDialog.FileName;
                if (fileWR.Read() == true)
                {
                    conditionsUC.NewLabel.Content = "Loaded";
                }
                else
                {
                    conditionsUC.NewLabel.Content = "Loading error";
                    return;
                }

                conditionsUC.ShowTable();
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            if (saveFileDialog.ShowDialog() == true)
            {
                storage.FilePath = saveFileDialog.FileName;
                if (fileWR.Write() == true)
                {
                    conditionsUC.NewLabel.Content = "File saved";
                }
                else
                {
                    conditionsUC.NewLabel.Content = "Saving error";
                    return;
                }
            }
        }
    }
}
