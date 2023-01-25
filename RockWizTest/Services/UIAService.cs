using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using UIAutomationClient;

namespace RockWizTest.Services
{
    public class UIAService : IUIAService
    {
        #region Fields

        private readonly CUIAutomation8 cUIAutomation8;

        private IUIAutomationElement? cUIAutomationElement;

        #endregion

        public UIAService()
        {
            cUIAutomation8 = new CUIAutomation8();
        }

        #region Methods

        public void GetUIAElement()
        {
            if (cUIAutomationElement == null || !cUIAutomationElement.Equals(cUIAutomation8.GetFocusedElement()))
            {
                cUIAutomationElement = cUIAutomation8.GetFocusedElement(); 
            }
        }

        public (int, string)? GetCaretPositionAndText()
        {
            GetUIAElement();

            if (cUIAutomationElement != null)
            {
                var guid = typeof(IUIAutomationTextPattern2).GUID;
                var ptr = cUIAutomationElement.GetCurrentPatternAs(UIA_PatternIds.UIA_TextPattern2Id, ref guid);
                if (ptr != IntPtr.Zero)
                {
                    var pattern = (IUIAutomationTextPattern2)Marshal.GetObjectForIUnknown(ptr);
                    if (pattern != null)
                    {
                        var documentRange = pattern.DocumentRange;
                        var caretRange = pattern.GetCaretRange(out _);
                        if (caretRange != null)
                        {
                            var caretPos = caretRange.CompareEndpoints(
                                TextPatternRangeEndpoint.TextPatternRangeEndpoint_Start,
                                documentRange,
                                TextPatternRangeEndpoint.TextPatternRangeEndpoint_Start);

                            var text = documentRange.GetText(caretPos);

                            return (caretPos, text);
                        }
                    }
                }
            }

            return null;
        }

        public void SetText(string text)
        {
            if (cUIAutomationElement != null)
            {
                cUIAutomationElement.SetFocus();

                Thread.Sleep(100);

                SendKeys.SendWait("^{LEFT}");
                SendKeys.SendWait("^+{RIGHT}");
                SendKeys.SendWait("{BACKSPACE}");
                SendKeys.SendWait(text + " ");

                return;
            }
        }

        #endregion
    }
}
