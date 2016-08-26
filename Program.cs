using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        static private Random r;
        static private Channel voiceChannel;
        //static public Discord.DiscordClient discordBot;

        //Enter your bot user Token
        static private string token = "MTk4MjI5MDU4NjY3MDIwMjkw.CldGsg.c285zBv1ZgcyTSYD9F1iT5nYnDo";
        //Enter your user ID
        private static ulong ownerId = 166671297923907584;


        static void Main(string[] args)
        {
            try
            {
                Bot iluvatarSuperBot = new Bot();
                Console.ReadKey();
            }
            catch
            { }
        }




        private static PermissionLevel GetPermissions(User u, Channel c)
        {
            if (u.Id == ownerId)
                return PermissionLevel.BotOwner;

            if (!c.IsPrivate)
            {
                if (u == c.Server.Owner)
                    return PermissionLevel.ServerOwner;

                var serverPerms = u.ServerPermissions;
                if (serverPerms.ManageRoles || u.Roles.Select(x => x.Name.ToLower()).Contains("admin"))
                    return PermissionLevel.ServerAdmin;
                if (serverPerms.ManageMessages && serverPerms.KickMembers && serverPerms.BanMembers)
                    return PermissionLevel.ServerModerator;

                var channelPerms = u.GetPermissions(c);
                if (channelPerms.ManagePermissions)
                    return PermissionLevel.ChannelAdmin;
                if (channelPerms.ManageMessages)
                    return PermissionLevel.ChannelModerator;
            }
            return PermissionLevel.User;
        }


        static public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}]  [{e.Source}]  {e.Message}");
        }
    }
}
