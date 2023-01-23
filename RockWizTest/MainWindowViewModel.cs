using CommunityToolkit.Mvvm.Input;
using RockWizTest.Helpers;
using RockWizTest.View;
using System.Collections.Generic;
using System.Windows.Input;

namespace RockWizTest
{
    public class MainWindowViewModel
    {
        #region Fields

        private int _id;
        private string? _message;

        private bool _show;

        private readonly IAbstractFactory<PredictionList> _predictionList;

        #endregion

        public MainWindowViewModel(IAbstractFactory<PredictionList> predictionList)
        {
            _predictionList = predictionList;
        }

        #region Commands

        #region LoadedCommand

        public ICommand LoadedCommand => new RelayCommand(Loaded); 

        public void Loaded()
        {
            _id = GlobalKeyboardHook.Instance.Hook(new List<Key>() { Key.LeftCtrl, Key.G }, KeysPushed, out _message);
        }

        #endregion

        #region ClosingCommand

        public ICommand ClosingCommand => new RelayCommand(Closing); 

        public void Closing()
        {
            GlobalKeyboardHook.Instance.UnHook(_id);
        }

        #endregion

        #endregion

        #region Helpers

        private void KeysPushed()
        {
            var predictionList = _predictionList.Create();

            predictionList.Closing += (s, e) =>
            {
                _show = false;
            };

            if (_show! == false)
            {
                predictionList.Show(); 
                
                _show = true;
            }
        }

        #endregion
    }
}
