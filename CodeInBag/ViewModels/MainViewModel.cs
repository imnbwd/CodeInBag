using CodeInBag.Base;
using CodeInBag.Models;
using CodeInBag.Utilities;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;

namespace CodeInBag.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        #region Variables

        public const string FileName = "code.dat";
        private ObservableCollection<CodeItem> _allCodeItems;
        private int _currentCodeType;
        private ICollectionView _itemsView;
        private string _keyword;

        #endregion Variables

        #region Constructors

        public MainViewModel(Package package)
        {
            Package = package;
            AllCodeItems = new ObservableCollection<CodeItem>();

            InsertCodeToEditorCommand = new RelayCommand(InsertCodeToEditor);
            RemoveItemCommand = new RelayCommand(RemoveItem);
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<CodeItem> AllCodeItems
        {
            get { return _allCodeItems; }
            set { SetProperty(ref _allCodeItems, value); }
        }

        public int CurrentCodeType
        {
            get { return _currentCodeType; }
            set
            {
                this.SetProperty(ref _currentCodeType, value);
                UpdateList();
            }
        }

        public string FilePath
        {
            get;
            private set;
        }

        public RelayCommand InsertCodeToEditorCommand { get; set; }

        public ICollectionView ItemsView
        {
            get { return _itemsView; }
            set { SetProperty(ref _itemsView, value); }
        }

        public string Keyword
        {
            get { return _keyword; }
            set
            {
                this.SetProperty(ref _keyword, value);
                UpdateList();
            }
        }

        public Package Package
        {
            get;
            private set;
        }

        public RelayCommand RemoveItemCommand { get; set; }
        public CodeItem SelectedCodeItem { get; set; }
        public IServiceProvider ServiceProvider => this.Package;

        #endregion Properties

        #region Methods

        public void ControlLoaded()
        {
            // create package folder under MyDocument folder
            var documentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var packageFolder = $@"{documentFolder}\{CodeInBagToolWindowPackage.Name}";
            if (!Directory.Exists(packageFolder))
            {
                Directory.CreateDirectory(packageFolder);
            }

            FilePath = $@"{packageFolder}\{FileName}";

            if (File.Exists(FilePath))
            {
                AllCodeItems = JsonConverter.DeserializeFromFile<ObservableCollection<CodeItem>>(FilePath);
            }

            ItemsView = CollectionViewSource.GetDefaultView(AllCodeItems);
        }

        public void SaveData()
        {
            JsonConverter.SerializeToFile(AllCodeItems, FilePath);
        }

        private void InsertCodeToEditor(object obj)
        {
            if (obj != null)
            {
                var codeItem = obj as CodeItem;
                var dte = ServiceProvider.GetService(typeof(DTE)) as DTE;
                if (dte.ActiveDocument != null)
                {
                    var text = dte.ActiveDocument.Selection as TextSelection;
                    text.Insert(codeItem.Content);
                }
            }
        }

        private void RemoveItem(object obj)
        {
            if (SelectedCodeItem != null)
            {
                AllCodeItems.Remove(SelectedCodeItem);
                SaveData();
            }
        }

        private void UpdateList()
        {
            if (CurrentCodeType > 0)
            {
                if (string.IsNullOrWhiteSpace(Keyword))
                {
                    ItemsView.Filter = item => (int)((CodeItem)item).Type == CurrentCodeType;
                }
                else
                {
                    ItemsView.Filter = item => (int)((CodeItem)item).Type == CurrentCodeType && ((CodeItem)item).Content.Contains(Keyword);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Keyword))
                {
                    ItemsView.Filter = item => true;
                }
                else
                {
                    ItemsView.Filter = item => ((CodeItem)item).Content.Contains(Keyword);
                }
            }
        }
    }

    #endregion Methods
}