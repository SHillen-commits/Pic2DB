using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pic2DB
{
    public class PicturesDataBase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<PicturesDataBase> Instance = new AsyncLazy<PicturesDataBase>(async () =>
        {
            var instance = new PicturesDataBase();
            CreateTableResult result = await Database.CreateTableAsync<Pictures>();
            return instance;
        });

        public PicturesDataBase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<Pictures>> GetItemsAsync()
        {
            return Database.Table<Pictures>().ToListAsync();
        }

        public Task<List<Pictures>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<Pictures>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<Pictures> GetItemAsync(int id)
        {
            return Database.Table<Pictures>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Pictures item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Pictures item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<int> DeleteItemsAsync()
        {
            return Database.DeleteAllAsync<Pictures>();
        }
    }
}
