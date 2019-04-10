using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashCards.Models
{
    public class PhotoCard : ICard
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Information { get; set; }
        public string Answer { get; set; }

        [Indexed]
        public int BundleId { get; set; }

    }
}
