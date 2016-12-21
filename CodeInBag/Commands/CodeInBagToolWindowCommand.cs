using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;

namespace CodeInBag.Commands
{
    internal sealed class CodeInBagToolWindowCommand : BaseCommand
    {
        private readonly Package package;

        public CodeInBagToolWindowCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            var commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, Constant.CodeInBagWindowCommandId);
                var menuItem = new MenuCommand(this.ShowToolWindow, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static CodeInBagToolWindowCommand Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider => this.package;

        public static void Initialize(Package package)
        {
            Instance = new CodeInBagToolWindowCommand(package);
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            var window = this.package.FindToolWindow(typeof(CodeInBagToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            var windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}