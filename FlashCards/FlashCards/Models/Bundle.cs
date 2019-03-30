using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashCards.Models
{
    public class Bundle
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Name { get; set; }

    }
}
