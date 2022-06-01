using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.ViewModels
{
  public class NotificationViewModel : BaseViewModel
  {
    private string errorText;
    public string ErrorText
    {
      get => errorText;
      set
      {
        errorText = value;
        OnPropertyChanged();
      }
    }
  }
}
