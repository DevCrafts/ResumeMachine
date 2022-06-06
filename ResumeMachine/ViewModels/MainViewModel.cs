using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.ViewModels
{
  public class MainViewModel : IMainViewModel
  {
    public MainViewModel()
    {
      this.SettingsViewModel = new SettingsViewModel();
      this.HomeViewModel = new HomeViewModel(this.SettingsViewModel);
    }

    public IHomeViewModel HomeViewModel { get; }
    public ISettingsViewModel SettingsViewModel { get; }
  }
}
