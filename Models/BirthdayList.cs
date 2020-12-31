using System;
using System.Collections.Generic;
using System.Text;

namespace Berdthday_Bot.Models
{
    public class BirthdayList
    {
        public string username { get; set; }
        public DateTime birthday { get; set; }

        public void AddBirthday(string User, DateTime Birthday)
        {
            username = User;
            birthday = Birthday;
        }
    }
}
