using CodeInBag.ViewModels;
using Microsoft.VisualStudio.Shell;
using SimpleInjector;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace CodeInBag.Commands
{
    public class CodeTypeSwitchCommand : BaseCommand
    {
        private readonly Container Container;
        private int currentIndexComboChoice = 0;
        private string[] indexComboChoices = new string[] { "All", "C#", "VB", "Xaml", "Other" };

        public CodeTypeSwitchCommand(Container container, IMenuCommandService commandService)
        {
            Container = container;

            if (commandService != null)
            {
                var cmdId = new CommandID(CommandSet, Constant.CodeTypeSwitchCommandId);
                var cmd = new OleMenuCommand(OnCodeTypeSwitchCommandInvoked, cmdId);
                commandService.AddCommand(cmd);

                var cmdListId = new CommandID(CommandSet, Constant.CodeTypeSwitchCommandItemListId);
                var cmdList = new OleMenuCommand(OnCodeTypeSwitchCommandListInvoked, cmdListId);
                commandService.AddCommand(cmdList);
            }
        }

        private void OnCodeTypeSwitchCommandInvoked(object s, EventArgs e)
        {
            if ((null == e) || (e == EventArgs.Empty))
            {
                // We should never get here; EventArgs are required.
                throw (new ArgumentException("EventArgsRequired")); // force an exception to be thrown
            }

            OleMenuCmdEventArgs eventArgs = e as OleMenuCmdEventArgs;

            if (eventArgs != null)
            {
                object input = eventArgs.InValue;
                IntPtr vOut = eventArgs.OutValue;

                if (vOut != IntPtr.Zero && input != null)
                {
                    throw (new ArgumentException("BothInOutParamsIllegal")); // force an exception to be thrown
                }
                if (vOut != IntPtr.Zero)
                {
                    // when vOut is non-NULL, the IDE is requesting the current value for the combo
                    Marshal.GetNativeVariantForObject(indexComboChoices[currentIndexComboChoice], vOut);
                }
                else if (input != null)
                {
                    int newChoice = -1;
                    if (!int.TryParse(input.ToString(), out newChoice))
                    {
                        // user typed a string argument in command window.
                        for (int i = 0; i < indexComboChoices.Length; i++)
                        {
                            if (string.Compare(indexComboChoices[i], input.ToString(), StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                newChoice = i;
                                break;
                            }
                        }
                    }

                    // new value was selected or typed in
                    if (newChoice != -1)
                    {
                        currentIndexComboChoice = newChoice;
                        SwitchCodeType(currentIndexComboChoice);
                    }
                    else
                    {
                        throw (new ArgumentException("ParamMustBeValidIndexOrStringInList")); // force an exception to be thrown
                    }
                }
                else
                {
                    // We should never get here; EventArgs are required.
                    throw (new ArgumentException("EventArgsRequired")); // force an exception to be thrown
                }
            }
            else
            {
                // We should never get here; EventArgs are required.
                throw (new ArgumentException("EventArgsRequired")); // force an exception to be thrown
            }
        }

        private void OnCodeTypeSwitchCommandListInvoked(object s, EventArgs e)
        {
            if (e == EventArgs.Empty)
            {
                throw (new ArgumentException("EventArgsRequired"));
            }

            OleMenuCmdEventArgs eventArgs = e as OleMenuCmdEventArgs;

            if (eventArgs != null)
            {
                object inParam = eventArgs.InValue;
                IntPtr vOut = eventArgs.OutValue;

                if (inParam != null)
                {
                    throw (new ArgumentException("InParamIllegal"));
                }
                else if (vOut != IntPtr.Zero)
                {
                    Marshal.GetNativeVariantForObject(indexComboChoices, vOut);
                }
                else
                {
                    throw (new ArgumentException("OutParamRequired")); // force an exception to be thrown
                }
            }
        }

        private void SwitchCodeType(int currentIndexComboChoice)
        {
            Container.GetInstance<MainViewModel>().CurrentCodeType = currentIndexComboChoice;
        }
    }
}