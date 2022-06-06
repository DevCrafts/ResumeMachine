using Microsoft.Extensions.DependencyInjection;
using Notification.Wpf;
using ResumeMachine.Data;
using ResumeMachine.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
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
    private void Run(object sender, StartupEventArgs e)
    {
      this.MainViewModel = new MainViewModel();

      var mainWindow = new MainWindow
      {
        DataContext = this.MainViewModel,
      };

      mainWindow.Show();
    }

    private MainViewModel MainViewModel { get; set; }
  }
}
