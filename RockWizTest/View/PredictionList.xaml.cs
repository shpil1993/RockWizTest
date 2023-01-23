using RockWizTest.Model;
using RockWizTest.Services;
using System.Windows;
using System.Windows.Controls;

namespace RockWizTest.View
{
    /// <summary>
    /// Interaction logic for PredictionList.xaml
    /// </summary>
    public partial class PredictionList : Window
    {
        public PredictionList(IWordPredictionService wordPredictionService, ICustomWordPredictionService customWordPredictionService)
        {
            InitializeComponent();

            DataContext = new PredictionListViewModel(wordPredictionService, customWordPredictionService);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem is Word item)
                {
                    if (DataContext is PredictionListViewModel dc)
                    {
                        dc.ParsePrediction(item.Value!);

                        listBox.SelectedItem = null;

                        this.Close(); 
                    }
                }
            }
        }
    }
}
