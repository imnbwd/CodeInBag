using CodeInBag.Views;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace CodeInBag
{
    [Guid("2b26dbc8-5995-451a-b7e2-1e0bb4e89028")]
    public class CodeInBagToolWindow : ToolWindowPane
    {
        public CodeInBagToolWindow() : base(null)
        {
            this.Caption = CodeInBagToolWindowPackage.Name;

            this.Content = CodeInBagToolWindowPackage.Container.GetInstance<MainView>();
            this.ToolBar = new CommandID(new Guid(Constant.CommandSetGuid), Constant.ToolbarId);
        }
    }
}