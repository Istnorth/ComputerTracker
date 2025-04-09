using System.Configuration;
using System.Data;
using System.Windows;

namespace ComputerTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            base.OnStartup(e);
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Необработанная ошибка: {e.Exception.Message}\n\n{e.Exception.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            // Можно записать ошибку в лог-файл
            e.Handled = true; // Если хотите продолжить работу приложения
        }
    }

}
