using System;
using System.ComponentModel;

namespace ResumeMachine.ViewModels
{
  public class ConfirmationDialogViewModel : BaseViewModel
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

    private string alertMessage;
    public string AlertMessage
    {
      get => alertMessage;
      set
      {
        alertMessage = value;
        OnPropertyChanged();
      }
    }
  }
}
