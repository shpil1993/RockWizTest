using CommunityToolkit.Mvvm.Input;
using RockWizTest.Helpers;
using RockWizTest.Model;
using RockWizTest.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RockWizTest
{
    public class MainWindowViewModel
    {
        #region Fields

        private readonly IWordPredictionService _wordPredictionService;
        private readonly ICustomWordPredictionService _customWordPredictionService;
        private readonly IUIAService _uIAService;
        private readonly GlobalKeyboardHook _globalKeyboardHook;
        private readonly DebounceDispatcher _debounceDispatcher;

        #endregion

        #region Properties

        /// <summary>
        /// Collection to display predictions from server
        /// </summary>
        public ObservableCollection<Word> Predictions { get; set; }

        /// <summary>
        /// Collection to display custom predictions
        /// </summary>
        public ObservableCollection<Word> CustomPredictions { get; set; }

        #endregion

        public MainWindowViewModel(IWordPredictionService wordPredictionService, ICustomWordPredictionService customWordPredictionService, IUIAService uIAService)
        {
            _wordPredictionService = wordPredictionService;
            _customWordPredictionService = customWordPredictionService;
            _uIAService = uIAService;
            _debounceDispatcher = new DebounceDispatcher();
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += KeyboardPressed;
            Predictions = new ObservableCollection<Word>();
            CustomPredictions = new ObservableCollection<Word>();
        }

        #region Methods

        public void ParsePrediction(string prediction)
        {
            _globalKeyboardHook.KeyboardPressed -= KeyboardPressed;

            _uIAService.SetText(prediction);

            _globalKeyboardHook.KeyboardPressed += KeyboardPressed;
        }

        #endregion

        #region Commands

        #region ClosingCommand

        public ICommand ClosingCommand => new RelayCommand(Closing); 

        public void Closing()
        {
            _globalKeyboardHook?.Dispose();
        }

        #endregion

        #endregion

        #region Helpers

        private void KeyboardPressed(object? sender, GlobalKeyboardHookEventArgs e)
        {
            _debounceDispatcher.Debounce(100, (o) => { ReadElementText(); });
        }

        private async void ReadElementText()
        {
            var posAndText = _uIAService.GetCaretPositionAndText();

            if (posAndText != null)
            {
                var text = posAndText.Value.Item2.Split(" ").Last().ToLower();

                var predictions = await _wordPredictionService.GetWordPrediction("en-GB", posAndText.Value.Item2);
                var predictions2 = await _customWordPredictionService.GetPredictions(text);

                Predictions.Clear();
                CustomPredictions.Clear();

                if (predictions != null)
                {
                    foreach (var item in predictions.Take(10))
                    {
                        Predictions.Add(item);
                    }
                }

                if (predictions2 != null)
                {
                    foreach (var item in predictions2.Take(10))
                    {
                        CustomPredictions.Add(item);
                    }
                }
            }
        }

        #endregion
    }
}
