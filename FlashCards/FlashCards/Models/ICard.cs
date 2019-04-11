using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashCards.Models
{
    public interface ICard
    {
        [PrimaryKey, AutoIncrement]
        int Id { get; set; }
        string Information { get; set; }
        string Answer { get; set; }
        int BundleId { get; set; }

    }
}
