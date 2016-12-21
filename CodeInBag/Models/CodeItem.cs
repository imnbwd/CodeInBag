using CodeInBag.Base;
using System.Collections.Generic;

namespace CodeInBag.Models
{
    public class CodeItem : NotificationObject
    {
        private string _content;
        private List<string> _tags;
        private string _title;

        private CodeType _type;

        /// <summary>
        /// Code content
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        /// <summary>
        /// Tags for the code item
        /// </summary>
        public List<string> Tags
        {
            get { return _tags; }
            set { SetProperty(ref _tags, value); }
        }

        /// <summary>
        /// Title, a short description for the code item
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// Code language type
        /// </summary>
        public CodeType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}