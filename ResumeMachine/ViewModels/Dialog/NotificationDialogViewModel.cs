using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.ViewModels.Dialog
{
  public class NotificationDialogViewModel : BaseViewModel
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
