using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SmartCart.Models;
using SmartCart.Services;

namespace SmartCart.ModelsView
{
    public class GroceryListItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<GroceryItem> GroceryItems { get; set; } = new();
        private GroceryListServices _groceryListServices;

        public ICommand DeleteCommand { get; }

        public GroceryListItemViewModel() 
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "groceryitems.db3");
            _groceryListServices = new GroceryListServices(dbPath);

            DeleteCommand = new Command<GroceryItem>(async (groceryItem) => await DeleteItem(groceryItem));
            LoadItems();
        }

        public async void LoadItems()
        {
            var items = await _groceryListServices.GetItemAsync();
            GroceryItems.Clear();
            foreach (var item in items) 
                GroceryItems.Add(item);
        }

        private async Task DeleteItem(GroceryItem item)
        {
            await _groceryListServices.DeleteItemAsync(item);
            GroceryItems.Remove(item);
        }
    }
}
