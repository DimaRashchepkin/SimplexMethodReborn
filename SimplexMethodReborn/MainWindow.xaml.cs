using Microsoft.Win32;
using Symplex_method.Views;
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

namespace Symplex_method
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        private readonly Storage storage;
        private readonly FileWR fileWR;
        private readonly ConditionsUC conditionsUC = new ConditionsUC();
        private readonly SymplexUC symplexUC = new SymplexUC();

        public MainWindow()
		{
			InitializeComponent();
            storage = new Storage();
            fileWR = new FileWR(storage);
            conditionsUC.storage = storage;
            symplexUC.storage = storage;
            ConditionsButton_Click(null, null);
        }

		private void ConditionsButton_Click(object sender, RoutedEventArgs e)
		{
			ConditionsButton.BorderBrush = SystemColors.ActiveBorderBrush;
			SymplexMethodButton.BorderBrush = SystemColors.InactiveBorderBrush;

            this.OutputView.Content = conditionsUC;
            conditionsUC.ShowTable();
        }

		private void SymplexMethodButton_Click(object sender, RoutedEventArgs e)
		{
			ConditionsButton.BorderBrush = SystemColors.InactiveBorderBrush;
			SymplexMethodButton.BorderBrush = SystemColors.ActiveBorderBrush;

            this.OutputView.Content = symplexUC;
            // symplexUC.ShowTable();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
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
