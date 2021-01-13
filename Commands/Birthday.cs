using Berdthday_Bot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Berdthday_Bot.Commands
{
    class Birthday : BaseCommandModule
    {
        bool running = false;
        DateTime lastcheck;

        [Command("congratulate")]
        [Description("Congratulate user on birthday")]
        public async Task Congratulate(CommandContext ctx, DiscordMember member)
        {
            await ctx.Channel
                .SendMessageAsync("Happy Birthday " + member.Mention + "!").ConfigureAwait(false);
            await ctx.Channel
                .SendMessageAsync("https://www.youtube.com/watch?v=XtIBHfOdyX0").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Add a birthday")]
        public async Task Add(CommandContext ctx, DiscordMember member, string birthday)
        {
            string location = @"D:\C#\Bot\Birthday\Berdthday Bot\Berdthday Bot\Text\Birthdays.txt";
            // "Delete" previous mention of user by rewriting all users apart from the one mentioned
            Delete(GetUserCode(member));
            // write the new data to text file
            using (StreamWriter sw = File.AppendText(location))
            {
                sw.WriteLine(GetUserCode(member) + " " + DateTime.Parse(birthday).ToString("dd/MM/yyyy"));
            }
            await ctx.Channel.SendMessageAsync("Added!");
        }

        [Command("check")]
        public async Task Check(CommandContext ctx)
        {
            if (lastcheck.ToString("dd/MM") != DateTime.Now.ToString("dd/MM") || lastcheck == null)
            {
                // read through the file containing users and birthdays
                string[] file = File.ReadAllLines(@"D:\C#\Bot\Birthday\Berdthday Bot\Berdthday Bot\Text\Birthdays.txt");
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
                    if (DateTime.Now.ToString("dd/MM") == DateTime.Parse(birthdays[i]).ToString("dd/MM"))
                    {
                        await ctx.Channel.SendMessageAsync("Happy Birthday <@" + users[i] + ">");
                        await ctx.Channel.SendMessageAsync("https://www.youtube.com/watch?v=XtIBHfOdyX0");
                    }
                }
            }
        }

        [Command("run")]
        public async Task Run(CommandContext ctx)
        {
            running = true;
            while (running && lastcheck.ToString("dd/MM") != DateTime.Now.ToString("dd/MM"))
            {
                await ctx.Channel.SendMessageAsync("Starting bot...");
                await Check(ctx);
                Thread.Sleep(21600000);
                lastcheck = DateTime.Now;
            }
        }

        [Command("stop")]
        public async Task Stop(CommandContext ctx)
        {
            running = false;
            await ctx.Channel.SendMessageAsync("Stopping.");
        }

        [Command("delete")]
        public async Task Delete(CommandContext ctx, DiscordMember member)
        {
            Delete(GetUserCode(member));
            await ctx.Channel.SendMessageAsync("Deleted birthday.");
        }

        public void Delete(string user)
        {
            string location = @"D:\C#\Bot\Birthday\Berdthday Bot\Berdthday Bot\Text\Birthdays.txt";
            string tempFile = @"D:\C#\Bot\Birthday\Berdthday Bot\Berdthday Bot\Text\temp.txt";
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
        }

        public string GetUserCode(DiscordMember member)
        {
            string[] code = (member.ToString()).Split("; ");
            string[] usercode = code[0].Split("r ");
            return usercode[1];
        }
    }
}
