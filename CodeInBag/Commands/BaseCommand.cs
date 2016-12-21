using System;

namespace CodeInBag.Commands
{
    public class BaseCommand
    {
        public static readonly Guid CommandSet = new Guid(Constant.CommandSetGuid);
    }
}