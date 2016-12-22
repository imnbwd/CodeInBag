using CodeInBag.Commands;
using CodeInBag.ViewModels;
using CodeInBag.Views;
using Microsoft.VisualStudio.Shell;
using SimpleInjector;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CodeInBag
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#1110", "#1112", "1.0", IconResourceID = 1400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(CodeInBagToolWindow))]
    [Guid(Constant.PackageGuid)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class CodeInBagPackage : Package
    {
        /// <summary>
        /// Package Name
        /// </summary>
        public const string Name = "Code In Bag";

        public static Container Container;

        public CodeInBagPackage()
        {
            // http://geekswithblogs.net/onlyutkarsh/archive/2013/06/02/loading-custom-assemblies-in-visual-studio-extensions-again.aspx
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        #region Package Members

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Container.Dispose();
        }

        protected override void Initialize()
        {
            CodeInBagToolWindowCommand.Initialize(this);

            Container = new Container();
            Container.Register(typeof(Package), () => this, Lifestyle.Singleton);
            Container.Register(typeof(IMenuCommandService), () => GetService(typeof(IMenuCommandService)), Lifestyle.Singleton);
            Container.Register<AddToCodeInBagCommand>(Lifestyle.Singleton);
            Container.Register<CodeTypeSwitchCommand>(Lifestyle.Singleton);
            Container.Register<KeywordSearchCommand>(Lifestyle.Singleton);
            Container.Register<MainView>(Lifestyle.Singleton);
            Container.Register<MainViewModel>(Lifestyle.Singleton);

            Container.Verify();
            base.Initialize();
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var System_Windows_Interactivity = "System.Windows.Interactivity".ToLower();
            var Microsoft_Expression_Interactions = "Microsoft.Expression.Interactions".ToLower();
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);

            var requestAssemblyName = args.Name.ToLower();    
            if (requestAssemblyName.Contains(System_Windows_Interactivity))
            {
                return Assembly.LoadFrom(Path.Combine(path, $"{System_Windows_Interactivity}.dll"));
            }
            else if (requestAssemblyName.Contains(Microsoft_Expression_Interactions))
            {
                return Assembly.LoadFrom(Path.Combine(path, $"{Microsoft_Expression_Interactions}.dll"));
            }
            else
            {
                return null;
            }
        }

        #endregion Package Members
    }
}