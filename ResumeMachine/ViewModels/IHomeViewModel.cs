using ResumeMachine.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ResumeMachine.ViewModels
{
  public interface IHomeViewModel
  {
    ICommand AddEducationCommand { get; }
    ICommand AddLanguageCommand { get; }
    ICommand AddPositionCommand { get; }
    string AlertMessage { get; set; }
    List<string> AvailableLanguages { get; set; }
    ICommand ChangeAllCVsCommand { get; }
    string DestinationFolderPath { get; set; }
    ObservableCollection<string> Employees { get; set; }
    bool IsPresent { get; set; }
    List<string> LanguageLevels { get; set; }
    ICommand LoadDataCommand { get; }
    List<string> Nationalities { get; set; }
    ICommand PrintCommand { get; }
    bool ProgressBarIsRunning { get; set; }
    ICommand RemoveEducationCommand { get; }
    ICommand RemoveLanguageCommand { get; }
    ICommand RemovePositionCommand { get; }
    ResumeData ResumeData { get; set; }
    string SelectedEmployee { get; set; }
    string TemplateLocationPath { get; set; }
  }
}