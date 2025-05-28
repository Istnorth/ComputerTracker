using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ComputerTracker.Data.DbModel;

namespace ComputerTracker.Data.Models.ViewModels
{
    public class KeyLogHistoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<KeyLogEntry> Entries { get; } = new();

        public KeyLogHistoryViewModel(int computerId)
        {
            LoadEntries(computerId);
        }

        private void LoadEntries(int computerId)
        {
            using var ctx = new AppDbContext();
            var list = ctx.KeyLogEntries
                          .Where(k => k.ComputerId == computerId)
                          .OrderByDescending(k => k.Time)
                          .ToList();

            foreach (var e in list)
                Entries.Add(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
