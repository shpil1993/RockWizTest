using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using UIAutomationClient;

namespace RockWizTest.Helpers
{
    public class UIAHelper
    {
        public static (int, string)? GetCaretPositionAndText(IntPtr? automationIntpr)
        {
            var automation = new CUIAutomation8();

            if (automationIntpr != null)
            {
                do
                {
                    var element = automation.ElementFromHandle(automationIntpr.Value);

                    if (element != null)
                    {
                        var condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "15");
                        var txt = element.FindFirst(TreeScope.TreeScope_Descendants, condition);
                        var guid = typeof(IUIAutomationTextPattern2).GUID;
                        var ptr = txt.GetCurrentPatternAs(UIA_PatternIds.UIA_TextPattern2Id, ref guid);
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
                    Thread.Sleep(100);

                } while (true); 
            }

            return null;
        }

        public static void SetText(IntPtr? automationIntpr, string text)
        {
            var automation = new CUIAutomation8();

            if (automationIntpr != null)
            {
                do
                {
                    var element = automation.ElementFromHandle(automationIntpr.Value);

                    if (element != null)
                    {
                        var condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "15");
                        var txt = element.FindFirst(TreeScope.TreeScope_Descendants, condition);
                        if (txt != null)
                        {
                            txt.SetFocus(); 
                            
                            Thread.Sleep(100);

                            SendKeys.SendWait("^{LEFT}");
                            SendKeys.SendWait("^+{RIGHT}");
                            SendKeys.SendWait("{BACKSPACE}");
                            SendKeys.SendWait(text + " ");

                            return;
                        }
                    }
                    Thread.Sleep(100);

                } while (true); 
            }
        }
    }
}
