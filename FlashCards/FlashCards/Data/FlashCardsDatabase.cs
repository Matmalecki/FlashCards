using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Models;
using SQLite;

namespace FlashCards.Data
{
    public class FlashCardsDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public FlashCardsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Bundle>().Wait();
            _database.CreateTableAsync<Card>().Wait();
            _database.CreateTableAsync<PhotoCard>().Wait();
        }

        public Task<List<Card>> GetCardsFromBundle(int id)
        {
            return _database.Table<Card>().Where(c => c.BundleId == id).ToListAsync();
        }

        public Task<Card> GetCardAsync(int id)
        {
            return _database.Table<Card>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<PhotoCard>> GetPhotoCardsFromBundle(int id)
        {
            return _database.Table<PhotoCard>().Where(c => c.BundleId == id).ToListAsync();
        }

        public Task<PhotoCard> GetPhotoCardAsync(int id)
        {
            return _database.Table<PhotoCard>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }


        public Task<List<Bundle>> GetBundlesAsync()
        {
            return _database.Table<Bundle>().ToListAsync();
        }

        public Task<Bundle> GetBundleAsync(int id)
        {
            return _database.Table<Bundle>().Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveCardAsync(ICard card)
        {
            if (card.Id != 0)
            {
                return _database.UpdateAsync(card);
            }
            else
            {
                return _database.InsertAsync(card);
            }
        }

        public Task<int> SaveBundleAsync(Bundle bundle)
        {

            if (bundle.Id != 0)
            {
                return _database.UpdateAsync(bundle);
            }
            else
            {
                return _database.InsertAsync(bundle);
            }
        }

        public Task<int> DeleteCardAsync(ICard card)
        {
            return _database.DeleteAsync(card);
        }
        public Task<int> DeleteBundleAsync(Bundle bundle)
        {
            return _database.DeleteAsync(bundle);
        }


    }
}
