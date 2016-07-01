using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public enum Id
    {
        MessageError
    };

    /// <summary>
    /// This Class is intended for futur use, it might never be needed.
    /// </summary>
    public class Messages
    {
        private static string[] message;

        static Messages()
        {
            message = new string[Enum.GetNames(typeof(Id)).Length];
            message[(int)Id.MessageError] = "Error!";
        }

        /// <summary>
        /// Permet d'accéder en lecture à un message à partir de son Id.
        /// </summary>
        public static string[] Message
        {
            get { return Messages.message; }
        }
    }
}
