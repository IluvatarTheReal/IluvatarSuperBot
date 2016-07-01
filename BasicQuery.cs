using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace DiscordBot
{
    public static class BasicQuery
    {
        public static IEnumerable<User> SearchUsers(CommandEventArgs e, string user)
        {
            return e.Server.Users.Where(u => u.Name.ToLower().Contains(e.GetArg(user)));
        }

        public static User GetUser(CommandEventArgs e, string user)
        {
            return e.Channel.Users.Where(u => u.Name.ToLower().Equals(e.GetArg(user).ToLower())).Select(x => x).First();
        }

        public static Role GetRole(CommandEventArgs e, string role)
        {
            return e.Server.Roles.Where(x => x.Name.ToLower().Equals(e.GetArg(role).ToLower())).Select(x => x).First();
        }

        public static string PermissionNameUser(User u, PermissionLevel perm)
        {
            if (perm == PermissionLevel.BotOwner)
                return ($"{u.NicknameMention} is 6 - BOT OWNER");

            else if (perm == PermissionLevel.ServerOwner)
                return ($"{u.NicknameMention} is 5 - SERVER OWNER");

            else if (perm == PermissionLevel.ServerAdmin)
                return ($"{u.NicknameMention} is 4 - SERVER ADMIN");

            else if (perm == PermissionLevel.ServerModerator)
                return ($"{u.NicknameMention} is 3 - SERVER MODERATOR");

            else if (perm == PermissionLevel.ChannelAdmin)
                return ($"{u.NicknameMention} is 2 - CHANNEL ADMIN");

            else if (perm == PermissionLevel.ChannelModerator)
                return ($"{u.NicknameMention} is 1 - CHANNEL MODERATOR");

            else if (perm == PermissionLevel.User)
                return ($"{u.NicknameMention} is 0 - USER");

            else
                return ($"{u.NicknameMention} is 0 - USER");
        }

    }
}
