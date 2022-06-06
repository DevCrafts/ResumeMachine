using Microsoft.Extensions.DependencyInjection;
using Notification.Wpf;
using ResumeMachine.Data;
using ResumeMachine.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ResumeMachine
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private ServiceProvider serviceProvider;

    public App()
    {
      ServiceCollection services = new ServiceCollection();
      ConfigureServices(services);
      serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection services)
    {
      services.AddSingleton<INotificationManager, NotificationManager>();
      //services.AddSingleton<ISettingsViewModel, SettingsViewModel>();
      //services.AddSingleton<IJsonDataProvider, JsonDataProvider>();
      services.AddSingleton<MainWindow>();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
      MainWindow? mainWindow = serviceProvider.GetService<MainWindow>();
      mainWindow.Show();
    }
  }
}
