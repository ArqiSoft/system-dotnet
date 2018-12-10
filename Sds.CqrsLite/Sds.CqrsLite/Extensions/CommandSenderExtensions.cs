using CQRSlite.Commands;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public static class CommandSenderExtensions
    {
        public static Task Redelivery<T>(this ICommandSender sender, T @event) where T : class, ICommand
        {
            return sender.Send<T>(@event);
        }
    }
}
