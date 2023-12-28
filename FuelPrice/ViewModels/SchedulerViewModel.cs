﻿using FuelPrice.Models;
using System.Collections.ObjectModel;

namespace FuelPrice.ViewModels
{
    public class SchedulerViewModel : BaseViewModel
    {
        public SchedulerViewModel()
        {
            Title = "Scheduler";
            Items = new ObservableCollection<Item>();

        }

        public ObservableCollection<Item> Items { get; private set; }

        async public void OnAppearing()
        {
            IEnumerable<Item> items = await DataStore.GetItemsAsync(true);
            Items.Clear();
            foreach (Item item in items)
            {
                Items.Add(item);
            }
        }
    }
}