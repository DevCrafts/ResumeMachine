using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.ViewModels
{
  public class PasswordDialogViewModel : BaseViewModel
  {
    private string name;
    public string Name
    {
      get => name;
      set
      {
        name = value;
        OnPropertyChanged();
      }
    }

    private string message;
    public string Message
    {
      get => message;
      set
      {
        message = value;
        OnPropertyChanged();
      }
    }

    private string notificationMessage;
    public string NotificationMessage
    {
      get => notificationMessage;
      set
      {
        notificationMessage = value;
        OnPropertyChanged();
      }
    }

    private string password;
    public string Password
    {
      get => password;
      set
      {
        password = value;
        OnPropertyChanged();
      }
    }
  }
}
