using Berdthday_Bot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Berdthday_Bot.Commands
{
    class Birthday : BaseCommandModule
    {
        public string Mention { get; }
        private List<BirthdayList> birthdays = new List<BirthdayList>();
        [Command("congratulate")]
        [Description("Congratulate user on birthday")]
        public async Task congratulate(CommandContext ctx, DiscordMember member)
        {
            await ctx.Channel
                .SendMessageAsync("Happy Birthday " + member.Mention + "!").ConfigureAwait(false);
            await ctx.Channel
                .SendMessageAsync("https://www.youtube.com/watch?v=XtIBHfOdyX0").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Add a birthday")]
        public async Task add(CommandContext ctx, DiscordMember member, string birthday)
        {
            await ctx.Channel.SendMessageAsync("Adding!");
            var userBirthday = new BirthdayList();
            userBirthday.AddBirthday(member.ToString(), DateTime.Parse(birthday));
            await ctx.Channel.SendMessageAsync(userBirthday.username.ToString() + userBirthday.birthday.ToString());
            birthdays.Add(userBirthday);
            await ctx.Channel.SendMessageAsync("Added!");
        }
    }
}
