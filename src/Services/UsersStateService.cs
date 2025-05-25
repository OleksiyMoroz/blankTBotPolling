using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Services
{
    public class UsersStateService
    {
        private List<User> users = new List<User>();
        public User GetUser(long id) {
            var usr = users.FirstOrDefault(u => u.Id == id);
            if(usr is not null) return usr;

            users.Add(new User(id));

            return GetUser(id);
        }

        public User ResetUser(long id) {
            var usr = GetUser(id);

            usr.Step = Actions.None;
            usr.VideoTitle = default!;
            usr.VideoSite = default!;

            return usr;
        }
    }

    public class User {
        public long Id { get; set; } = default;
        public Actions Step { get; set; } = Actions.None;
        public string VideoTitle { get; set; } = default!;
        public string VideoSite { get; set; } = default!;
        public User(long id) {
            Id = id;
        }      
    }

    public enum Actions {
        None,
        TypePlayerId,
        TypeTeamId,
        TypeVideoTitle,
        TypeVideoSite,
        TypindRemind
    }
}