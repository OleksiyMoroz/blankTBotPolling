using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Data
{
    public class Reminder
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public DateTime ReminderDateTime { get; set; }
        public string GameDetails { get; set; }
    }
}