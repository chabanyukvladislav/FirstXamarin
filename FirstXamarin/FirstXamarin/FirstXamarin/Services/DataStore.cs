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
        private static object locker = new object();
        private static DataStore dataStore;
        private Context context;

        public static IDataStore<PhonesBook> GetDataStore
        {
            get
            {
                if (dataStore == null)
                {
                    lock (locker)
                    {
                        if (dataStore == null)
                        {
                            dataStore = new DataStore();
                        }
                    }
                }
                return dataStore;
            }
        }

        private DataStore()
        {
            context = new Context();
        }

        public async Task<bool> AddItemAsync(PhonesBook item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    context.PhonesBooks.Add(item);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
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
                    context.PhonesBooks.Remove(item);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
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
                    PhonesBook data = context.PhonesBooks.FirstOrDefault(el => el.Id == item.Id);
                    data.Name = item.Name;
                    data.Surname = item.Surname;
                    data.Phone = item.Phone;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
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
                    PhonesBook data = context.PhonesBooks.FirstOrDefault(item => item.Id == id);
                    return data;
                }
                catch (Exception ex)
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
                    List<PhonesBook> data = context.PhonesBooks.ToList();
                    return data;
                }
                catch (Exception ex)
                {
                    return new List<PhonesBook>();
                }
            });
        }
    }
}