using FirstXamarin.DatabaseContext;
using FirstXamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstXamarin.Services
{
    public class DataStore : IDataStore<PhonesBook>
    {
        private static readonly object Locker = new object();
        private static DataStore _dataStore;
        private readonly Context _context;

        public static IDataStore<PhonesBook> GetDataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    lock (Locker)
                    {
                        if (_dataStore == null)
                        {
                            _dataStore = new DataStore();
                        }
                    }
                }
                return _dataStore;
            }
        }

        private DataStore()
        {
            _context = new Context();
        }

        public async Task<bool> AddItemAsync(PhonesBook item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _context.PhonesBooks.Add(item);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public async Task<bool> DeleteItemAsync(PhonesBook item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _context.PhonesBooks.Remove(item);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public async Task<bool> UpdateItemAsync(PhonesBook item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    PhonesBook data = _context.PhonesBooks.FirstOrDefault(el => el.Id == item.Id);
                    if (data != null)
                    {
                        data.Name = item.Name;
                        data.Surname = item.Surname;
                        data.Phone = item.Phone;
                    }
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public async Task<PhonesBook> GetItemAsync(int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    PhonesBook data = _context.PhonesBooks.FirstOrDefault(item => item.Id == id);
                    return data;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public async Task<List<PhonesBook>> GetItemsAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    List<PhonesBook> data = _context.PhonesBooks.ToList();
                    return data;
                }
                catch (Exception)
                {
                    return new List<PhonesBook>();
                }
            });
        }
    }
}