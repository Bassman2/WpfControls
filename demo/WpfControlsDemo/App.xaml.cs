namespace WpfControlsDemo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void OnApplicationStartup(object sender, StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += (s, a) =>
        {
            Exception ex = (Exception)a.ExceptionObject;
            Trace.TraceError(ex.ToString());
            MessageBox.Show(ex.ToString(), "Unhandled Error !!!");
        };

        new WpfControlsDemo.View.MainView() { DataContext = new WpfControlsDemo.ViewModel.MainViewModel() }.Show();
    }
}
