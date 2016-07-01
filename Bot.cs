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
    public enum PermissionLevel
    {
        User = 0,
        ChannelModerator, //Manage Messages (Channel)
        ChannelAdmin, //Manage Permissions (Channel)
        ServerModerator, //Manage Messages, Kick, Ban (Server)
        ServerAdmin, //Manage Roles (Server)
        ServerOwner, //Owner (Server)
        BotOwner //Bot Owner (Global)
    }

    public class Bot
    {
        private DiscordClient discordBot;
        private Random r;

        //Enter your bot user Token
        private string token = "MTk4MjI5MDU4NjY3MDIwMjkw.CldGsg.c285zBv1ZgcyTSYD9F1iT5nYnDo";        


        public Bot()
        {
            r = new Random();
            discordBot = new DiscordClient(x =>
            {
                x.AppName = "IluvatarSuperBot";
                x.LogLevel = LogSeverity.Debug;
                x.LogHandler = Log;
            });

            discordBot.UsingCommands(x =>
            {
                x.PrefixChar = '~';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Public;
            });

            discordBot.UsingPermissionLevels((u, c) => (int)GetPermissions(u, c));

            CreateCommand();

            discordBot.ExecuteAndWait(async () =>
            {
                await discordBot.Connect(token);
            });
        }

        public void CreateCommand()
        {
            var cService = discordBot.GetService<CommandService>();

            #region Ping Command
            cService.CreateCommand("ping")
                .Description("returns 'Pong'")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Pong");
                });
            #endregion

            #region Hello Command
            cService.CreateCommand("Hello")
                .Description("Return the user parameter")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string userArg = $"{e.GetArg("user")}";

                    string message = $"{e.User.NicknameMention} says hello to {userArg}";
                    await e.Channel.SendMessage(message);
                });
            #endregion

            #region HelloDoubleMention Command
            cService.CreateCommand("Hello2you")
                .Description("Say hello to an other user, the username entered must have the same characters")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    User user;

                    user = e.Server.Users.Where(x => x.Name.ToLower().Equals(e.GetArg("user").ToLower())).Select(x => x).First();

                    await e.Channel.SendMessage($"{e.User.NicknameMention} says Hello to {user.NicknameMention}");
                });
            #endregion

            #region Roll Command
            cService.CreateGroup("roll", cg =>
            {

                cg.CreateCommand("1d4")
                    .Description("Roll 1d4")
                    .Do(async (e) =>
                    {
                        int d = r.Next(1, 5);

                        await e.Channel.SendMessage($"{e.User.NicknameMention} rolled {d}");
                    });

                cg.CreateCommand("1d6")
                    .Description("Roll 1d6")
                    .Do(async (e) =>
                    {
                        int d = r.Next(1, 7);

                        await e.Channel.SendMessage($"{e.User.NicknameMention} rolled {d}");
                    });

                cg.CreateCommand("1d12")
                    .Description("Roll 1d12")
                    .Do(async (e) =>
                    {
                        int d = r.Next(1, 13);

                        await e.Channel.SendMessage($"{e.User.NicknameMention} rolled {d}");
                    });

                cg.CreateCommand("1d20")
                    .Description("Roll 1d20")
                    .Do(async (e) =>
                    {
                        int d = r.Next(1, 21);

                        await e.Channel.SendMessage($"{e.User.NicknameMention} rolled {d}");
                    });

                cg.CreateCommand("custom")
                    .Parameter("qtyDice", ParameterType.Required)
                    .Parameter("sizeDice", ParameterType.Required)
                    .Description("Make a custom dice roll ~roll custom <qtyDice> <sizeDice>")
                    .Do(async (e) =>
                    {
                        int qtyDice = Convert.ToInt32(e.GetArg("qtyDice"));
                        int sizeDice = Convert.ToInt32(e.GetArg("sizeDice"));

                        int d = 0;
                        for (int i = 0; i < qtyDice; i++)
                        {
                            d += r.Next(1, sizeDice + 1);
                        }

                        await e.Channel.SendMessage($"{e.User.NicknameMention} rolled {qtyDice}d{sizeDice} and got {d}");
                    });
            });

            #endregion

            #region Credit Command
            cService.CreateCommand("Credit")
                .Description("Try it!")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Iluvatar's Super Bot was coded by Iluvatar/Samuel Reid");
                });
            #endregion       

            #region Role Command
            cService.CreateGroup("role", cg =>
            {
                cg.CreateCommand("list")
                    .Do(async (e) =>
                    {
                        IEnumerable<Role> roles = e.Server.Roles.OrderByDescending(r => r.Position);
                        string message = $"The role in this server are :\n";
                        foreach (var r in roles)
                        {
                            message += $"{r.Mention}\n";
                        }
                        await e.Channel.SendMessage(message);
                    });

                cg.CreateCommand("userin")
                    .Description("List all user with the chosen role")
                    .Parameter("role", ParameterType.Unparsed)
                    .Do(async (e) =>
                    {
                        Role role;
                        IEnumerable<User> users;

                        role = e.Server.Roles.Where(x => x.Name.ToLower().Equals(e.GetArg("role").ToLower())).Select(x => x).First();
                        users = e.Server.Users.Where(x => x.Roles.Contains(role));

                        string message = $"The {role.Mention} are :\n";
                        foreach (var u in users)
                        {
                            message += $"{u.NicknameMention}\n";
                        }

                        await e.Channel.SendMessage(message);
                    });
            });
            #endregion

            #region Chat Command
            cService.CreateGroup("chat", cg =>
            {
                cg.CreateCommand("clear")
                    .Description("Clear the channel removing all message sent since the bot went online.")
                    .Do(async (e) =>
                    {
                        await e.Channel.DeleteMessages(e.Channel.Messages.ToArray());
                    });

                cg.CreateCommand("permission")
                    .Description("Return your permission status")
                    .Do(async (e) =>
                    {
                        PermissionLevel perm = GetPermissions(e.User, e.Channel);

                        if (perm== PermissionLevel.ServerAdmin)
                            await e.Channel.SendMessage($"SERVER ADMIN");

                        else if (perm == PermissionLevel.ChannelAdmin)
                            await e.Channel.SendMessage($"CHANNEL ADMIN");

                        else if (perm == PermissionLevel.ServerModerator)
                            await e.Channel.SendMessage($"SERVER MODERATOR");

                        else if (perm == PermissionLevel.ChannelModerator)
                            await e.Channel.SendMessage($"CHANNEL MODERATOR");

                        else if (perm == PermissionLevel.ServerOwner)
                            await e.Channel.SendMessage($"SERVER OWNER");

                        else if (perm == PermissionLevel.User)
                            await e.Channel.SendMessage($"USER");

                    });
            });
            #endregion
        }

        private static PermissionLevel GetPermissions(User u, Channel c)
        {
            if (u.Id == 0000) // Replace this with your own UserId
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

        public void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}]  [{e.Source}]  {e.Message}");
        }

    }
}
