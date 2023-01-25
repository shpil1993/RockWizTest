using RockWizTest.Helpers;
using RockWizTest.Model;
using RockWizTest.Services;
using RockWizTest.View;
using System.Windows;
using System.Windows.Controls;

namespace RockWizTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IWordPredictionService wordPredictionService, ICustomWordPredictionService customWordPredictionService, IUIAService uIAService)
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(wordPredictionService, customWordPredictionService, uIAService);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem is Word item)
                {
                    if (DataContext is MainWindowViewModel dc)
                    {
                        dc.ParsePrediction(item.Value!);

                        listBox.SelectedItem = null;
                    }
                }
            }
        }
    }
}
