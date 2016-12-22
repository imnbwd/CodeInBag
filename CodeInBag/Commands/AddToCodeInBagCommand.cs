using CodeInBag.ViewModels;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using SimpleInjector;
using System;
using System.ComponentModel.Design;
using System.IO;

namespace CodeInBag.Commands
{
    internal sealed class AddToCodeInBagCommand : BaseCommand
    {
        private readonly Container container;
        private readonly Package package;

        public AddToCodeInBagCommand(Container container, IMenuCommandService commandService, Package package)
        {
            this.package = package;
            this.container = container;

            if (commandService != null)
            {
                var cmdIdForCodeWin = new CommandID(CommandSet, Constant.AddToCodeInBagCodeWinCommandId);
                var cmdForCodeWin = new MenuCommand(AddtoCodeInBagHanlder, cmdIdForCodeWin);

                var cmdIdForXamlEditor = new CommandID(CommandSet, Constant.AddToCodeInBagXamlEditorCommandId);
                var cmdForXamlEditor = new MenuCommand(AddtoCodeInBagHanlder, cmdIdForXamlEditor);

                commandService.AddCommand(cmdForCodeWin);
                commandService.AddCommand(cmdForXamlEditor);
            }
        }

        private IServiceProvider ServiceProvider => this.package;

        private void AddtoCodeInBagHanlder(object sender, EventArgs e)
        {
            var dte = ServiceProvider.GetService(typeof(DTE)) as DTE;
            if (dte.ActiveDocument != null && dte.ActiveDocument.Selection != null)
            {
                var type = CodeType.Other;
                var fileExtension = Path.GetExtension(dte.ActiveDocument.FullName).ToLower();
                switch (fileExtension)
                {
                    case ".cs":
                        type = CodeType.CSharp;
                        break;

                    case ".vb":
                        type = CodeType.VB;
                        break;

                    case ".xaml":
                        type = CodeType.Xaml;
                        break;

                    default:
                        type = CodeType.Other;
                        break;
                }
                var text = dte.ActiveDocument.Selection as TextSelection;
                if (string.IsNullOrWhiteSpace(text.Text))
                {
                    VsShellUtilities.ShowMessageBox(this.ServiceProvider, "No selected text", CodeInBagPackage.Name,
                        Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_WARNING,
                         Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK,
                          Microsoft.VisualStudio.Shell.Interop.OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST
                        );
                    return;
                }

                // Activate the tool window before inserting new item to the list
                var toolWindow = this.package.FindToolWindow(typeof(CodeInBagToolWindow), 0, true);

                // https://social.msdn.microsoft.com/Forums/vstudio/en-US/2da1f1b1-e160-4330-b30b-3c1d3c02142b/how-to-force-activation-of-toolwindow?forum=vsx
                dte.Windows.Item(toolWindow.Caption).Activate();

                var mainViewModel = container.GetInstance<MainViewModel>();
                mainViewModel.AllCodeItems.Add(
                    new Models.CodeItem
                    {
                        Title = $"Code added at {DateTime.Now.ToString("yyyy/M/d HH:mm")}",
                        Content = text.Text,
                        Type = type
                    });

                mainViewModel.SaveData();
            }
        }
    }
}