using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Data
{
    public class FavoriteVideo
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string VideoTitle { get; set; }
        public string VideoUrl { get; set; }
    }
}