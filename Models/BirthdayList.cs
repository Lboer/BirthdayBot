using System;
using System.Collections.Generic;
using System.Text;

namespace Berdthday_Bot.Models
{
    public class BirthdayList
    {
        public string Username { get; set; }
        public DateTime Birthday { get; set; }

        public void AddBirthday(string User, DateTime birthday)
        {
            Username = User;
            Birthday = birthday;
        }
    }
}
