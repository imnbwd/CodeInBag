using CodeInBag.ViewModels;
using Microsoft.VisualStudio.Shell;
using SimpleInjector;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace CodeInBag.Commands
{
    public class KeywordSearchCommand : BaseCommand
    {
        private readonly Container Container;
        private readonly Package Package;
        private string currentMRUComboChoice = null;

        public KeywordSearchCommand(Container container, IMenuCommandService commandService, Package package)
        {
            this.Package = package;
            Container = container;

            if (commandService != null)
            {
                CommandID menuMyMRUComboCommandID = new CommandID(CommandSet, Constant.KeywordSearchMRUComboId);
                OleMenuCommand menuMyMRUComboCommand = new OleMenuCommand(new EventHandler(OnKeywordSearchMRUCombo), menuMyMRUComboCommandID);
                commandService.AddCommand(menuMyMRUComboCommand);
            }
        }

        public IServiceProvider ServiceProvider => Package;

        private void FilterItemsWithKeyword(string currentMRUComboChoice)
        {
            Container.GetInstance<MainViewModel>().Keyword = currentMRUComboChoice;
        }

        private void OnKeywordSearchMRUCombo(object sender, EventArgs e)
        {
            if (e == EventArgs.Empty)
            {
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
                else if (vOut != IntPtr.Zero)
                {
                    // when vOut is non-NULL, the IDE is requesting the current value for the combo
                    Marshal.GetNativeVariantForObject(currentMRUComboChoice, vOut);
                }
                else if (input != null)
                {
                    string newChoice = input.ToString();

                    // new value was selected or typed in
                    if (string.IsNullOrEmpty(newChoice))
                    {
                        //VsShellUtilities.ShowMessageBox(ServiceProvider, "Keyword cannot be empty", CodeInBagToolWindowPackage.Name,
                        //     Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_WARNING,
                        //      Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        //      Microsoft.VisualStudio.Shell.Interop.OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                        //return;

                        newChoice = string.Empty;
                    }

                    currentMRUComboChoice = newChoice;
                    FilterItemsWithKeyword(currentMRUComboChoice);
                }
                else
                {
                    throw (new ArgumentException("BothInOutParamsIllegal")); // force an exception to be thrown
                }
            }
            else
            {
                throw (new ArgumentException("EventArgsRequired")); // force an exception to be thrown
            }
        }
    }
}