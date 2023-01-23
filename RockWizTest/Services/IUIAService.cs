namespace RockWizTest.Services
{
    public interface IUIAService
    {
        #region Methods

        void GetUIAElement();

        (int, string)? GetCaretPositionAndText();

        void SetText(string text);

        #endregion
    }
}
