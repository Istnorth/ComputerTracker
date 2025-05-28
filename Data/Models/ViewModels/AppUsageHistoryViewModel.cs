using ComputerTracker.Data.DbModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class AppUsageHistoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<AppUsageEntry> Entries { get; } = new();

        public AppUsageHistoryViewModel(int computerId)
        {
            LoadEntries(computerId);
        }

        private void LoadEntries(int computerId)
        {
            using var ctx = new AppDbContext();
            var list = ctx.AppUsageEntries
                          .Where(a => a.ComputerId == computerId)
                          .ToList();
            Entries.Clear();
            foreach (var e in list) Entries.Add(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
