using RockWizTest.Helpers;
using RockWizTest.Services;
using RockWizTest.View;
using System.Windows;

namespace RockWizTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IAbstractFactory<PredictionList> predictionList)
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(predictionList);
        }
    }
}
