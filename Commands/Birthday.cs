using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Berdthday_Bot.Commands
{
    class Birthday : BaseCommandModule
    {
        bool running = false;
        DateTime lastcheck;
        string location = @"temp/Birthdays.txt";
        CultureInfo provider = CultureInfo.InvariantCulture;

        [Command("add")]
        [Description("Add a birthday")]
        public async Task Add(CommandContext ctx, DiscordMember member, string birthday)
        {
            // "Delete" previous mention of user by rewriting all users apart from the one mentioned
            Console.WriteLine("Before code execution");
            Delete(GetUserCode(member));
            // write the new data to text file
            Console.WriteLine("After Deletion");
            try
            {
                using (StreamWriter sw = File.AppendText(location))
                {
                    sw.WriteLine(GetUserCode(member) + " " + DateTime.ParseExact(birthday, "dd/MM", provider).ToString("dd/MM"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Placed code");
            await ctx.Channel.SendMessageAsync("Added!");
        }

        [Command("check")]
        [Description("This function will only react once a day. It can be called manually or via the \"run\" command")]
        public async Task Check(CommandContext ctx)
        {
            Console.WriteLine(lastcheck);
            Console.WriteLine(running);
            if (lastcheck.ToString("dd/MM") != DateTime.Now.ToString("dd/MM") || lastcheck == null)
            {
                // read through the file containing users and birthdays
                string[] file = File.ReadAllLines(location);
                List<string> birthdayList = new List<string>();
                List<string> userList = new List<string>();
                for (int i = 0; i < file.Length; i++)
                {
                    if(file[i].Length > 2)
                    {
                        userList.Add(file[i].Split(" ")[0]);
                        birthdayList.Add(file[i].Split(" ")[1]);
                    }
                }
                // make an array of birthdays and users
                string[] birthdays = birthdayList.ToArray();
                string[] users = userList.ToArray();
                // if today is someone's birthday, message them.
                for (int i = 0; i < birthdays.Length; i++)
                {
                    if (DateTime.Now.ToString("dd/MM") == DateTime.ParseExact(birthdays[i], "dd/MM", provider).ToString("dd/MM"))
                    {
                        await ctx.Channel.SendMessageAsync("Happy Birthday <@" + users[i] + ">");
                        await ctx.Channel.SendMessageAsync("https://www.youtube.com/watch?v=XtIBHfOdyX0");
                    }
                }
            }
        }

        [Command("run")]
        [Description("Turn on the bot to check for birthdays daily")]
        public async Task Run(CommandContext ctx)
        {
            running = true;
            while (running && lastcheck.ToString("dd/MM") != DateTime.Now.ToString("dd/MM"))
            {
                await Check(ctx);
                lastcheck = DateTime.Now;
                Console.WriteLine("After running, today it is: " + lastcheck);
            }
            Thread.Sleep(21600000);
        }

        [Command("stop")]
        [Description("Turn off the bot")]
        public async Task Stop(CommandContext ctx)
        {
            running = false;
            await ctx.Channel.SendMessageAsync("Stopping.");
        }

        [Command("delete")]
        [Description("Delete the mentioned user's birthday")]
        public async Task Delete(CommandContext ctx, DiscordMember member)
        {
            Delete(GetUserCode(member));
            await ctx.Channel.SendMessageAsync("Deleted birthday.");
        }

        public void Delete(string user)
        {
            string tempFile = @"temp/temp.txt";
            // delete line if mentioned before
            using (var writer = new StreamWriter(tempFile))
                foreach (string line in File.ReadLines(location))
                {
                    if (!line.Contains(user))
                    {
                        writer.WriteLine(line);
                    }
                }
            // delete old main file, change temp file to main file.
            File.Delete(location);
            File.Move(tempFile, location);
            Console.WriteLine("Deletion/ overwrite worked correctly");
        }

        public string GetUserCode(DiscordMember member)
        {
            string[] code = (member.ToString()).Split("; ");
            string[] usercode = code[0].Split("r ");
            return usercode[1];
        }
    }
}
