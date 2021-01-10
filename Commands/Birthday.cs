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
        private List<BirthdayList> birthdays = new List<BirthdayList>();
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
            // register locations for the main file and the temporary file that will be deleted
            string location = @"D:\C#\Bot\Birthday\Berdthday Bot\Berdthday Bot\Text\Birthdays.txt";
            string tempFile = @"D:\C#\Bot\Birthday\Berdthday Bot\Berdthday Bot\Text\temp.txt";
            // get discord user code
            string[] code = (member.ToString()).Split("; ");
            string[] usercode = code[0].Split("r ");
            // delete line if mentioned before
            using (var writer = new StreamWriter(tempFile))
            foreach(string line in File.ReadLines(location))
            {
                if (!line.Contains(usercode[1]))
                {
                    writer.WriteLine(line);
                }
            }
            // delete old main file, change temp file to main file.
            File.Delete(location);
            File.Move(tempFile, location);
            // write the new data to text file
            using (StreamWriter sw = File.AppendText(location))
            {
                sw.WriteLine(usercode[1].ToString() + " " + DateTime.Parse(birthday).ToString("dd/MM/yyyy"));
            }
            // Old way of adding, only lasts while bot remains on
            var userBirthday = new BirthdayList();
            userBirthday.AddBirthday(member.ToString(), DateTime.Parse(birthday));
            birthdays.Add(userBirthday);
            await ctx.Channel.SendMessageAsync("Added!");
        }

        [Command("check")]
        public async Task Check(CommandContext ctx, int position)
        {
            if (lastcheck.ToString("dd/MM") != DateTime.Now.ToString("dd/MM") || lastcheck == null)
            {
                
                if (DateTime.Now.ToString("dd/MM") == birthdays[position].Birthday.ToString("dd/MM"))
                {
                    string[] code = birthdays[position].Username.Split("; ");
                    string[] usercode = code[0].Split("r ");
                    await ctx.Channel.SendMessageAsync("Happy Birthday <@" + usercode[1] + ">");
                    await ctx.Channel.SendMessageAsync("https://www.youtube.com/watch?v=XtIBHfOdyX0");
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
                for(int i = 0; i < birthdays.Count; i++)
                {
                    await Check(ctx, i);
                }
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
    }
}
