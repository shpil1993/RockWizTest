using CommunityToolkit.Mvvm.Input;
using RockWizTest.Helpers;
using RockWizTest.Model;
using RockWizTest.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RockWizTest.View
{
    public class PredictionListViewModel
    {
        #region Fields

        private readonly IWordPredictionService _wordPredictionService;
        private readonly ICustomWordPredictionService _customWordPredictionService;
        private readonly IUIAService _uIAService;

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

        public PredictionListViewModel(IWordPredictionService wordPredictionService, ICustomWordPredictionService customWordPredictionService, IUIAService uIAService)
        {
            _wordPredictionService = wordPredictionService;
            _customWordPredictionService = customWordPredictionService;
            _uIAService = uIAService;
            Predictions = new ObservableCollection<Word>();
            CustomPredictions = new ObservableCollection<Word>();
        }

        #region Methods

        public async Task LoadPredictions()
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

        public void ParsePrediction(string prediction)
        {
            _uIAService.SetText(prediction);
        }

        #endregion

        #region Commands

        #region LoadedCommand

        /// <summary>
        /// Command for window loaded event
        /// </summary>
        public ICommand LoadedCommand => new RelayCommand(Loaded);

        private async void Loaded()
        {
            await LoadPredictions();
        }

        #endregion

        #endregion

        #region Helpers

        private IntPtr? GetAutomationElement(string name)
        {
            var processes = Process.GetProcessesByName(name);

            if (processes == null || processes.Count() <= 0)
                return null;

            var process = processes.FirstOrDefault(x => x.ProcessName.Equals(name, System.StringComparison.OrdinalIgnoreCase));

            if (process == null)
                return null;

            return process.MainWindowHandle;
        }

        #endregion
    }
}
