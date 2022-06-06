using MaterialDesignThemes.Wpf;
using Notification.Wpf;
using ResumeMachine.Commands;
using ResumeMachine.Data;
using ResumeMachine.Helper;
using ResumeMachine.Models;
using ResumeMachine.UserControls;
using ResumeMachine.ViewModels.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ResumeMachine.ViewModels
{
  public class HomeViewModel : BaseViewModel, IHomeViewModel
  {
    public HomeViewModel(ISettingsViewModel settingsViewModel)
    {
      this.ConfirmationDialogViewModel = new ConfirmationDialogViewModel();
      this.NotificationViewModel = new NotificationDialogViewModel();
      this.PasswordDialogViewModel = new PasswordDialogViewModel();
      this.JsonDataProvider = new JsonDataProvider();

      this.SettingsViewModel = settingsViewModel;
      this.SettingsViewModel.SettingsChanged += this.OnSettingsChanged;

      this.nationalities = this.LoadNationalities();
      this.availableLanguages = this.LoadAvailableLanguages();
      this.languageLevels = new List<string> { "Beginner", "Intermediate", "Advanced", "Mother tongue" };

      this.employees = new ObservableCollection<string> { "Jack Smith" };

      this.resumeData = this.InitializeExampleData();

      this.ProgressBarIsRunning = false;

      this.Timer.Interval = 15000;
      this.Timer.Elapsed += this.OnTimerElapsed;
      this.Timer.AutoReset = false;
      this.Timer.Start();

      //this.cvData = new CvData() 
      //{ 
      //  Educations = new ObservableCollection<Education>
      //  {
      //    new Education { Id = 1, Description = "", },
      //  },
      //  Languages = new ObservableCollection<Language>
      //  {
      //    new Language { Id = 1, Name = "", Level = "", }
      //  },
      //  JobRecords = new ObservableCollection<JobRecord>
      //  {
      //    new JobRecord { Id = 1 },
      //  },
      //};
    }

    private void OnSettingsChanged(object? sender, EventArgs e)
    {
      this.DestinationFolderPath = this.SettingsViewModel.FolderPath;
      this.TemplateLocationPath = this.SettingsViewModel.TemplatePath;
    }

    private ResumeData InitializeExampleData()
    {
      return new ResumeData
      {
        FirstName = "John",
        LastName = "Smith",
        PresentPosition = "Director",
        YearsWithCompany = "2",
        CurrentCompany = "Google",
        Nationality = nationalities[3],
        DateOfBirth = new DateTime(1989, 11, 21),
        JobRecords = new ObservableCollection<JobRecord>
          {
            new JobRecord
            {
              Id = 1,
              FromDate = DateTime.Today,
              ToDate = DateTime.Today,
              CompanyName = "Amazon",
              PositionTitle = "Project Manager",
              JobDescription = "I worked as a Project manager",
              IsPresent = true,
            },
            new JobRecord
            {
              Id = 2,
              FromDate = DateTime.Today,
              ToDate = DateTime.Today,
              CompanyName = "Google",
              PositionTitle = "Sales Manager",
              JobDescription = "I worked as a Sales manager"
            },
          },
        Educations = new ObservableCollection<Education>
          {
            new Education
            {
              Id = 1,
              Description = "Harvard University, 2015",
            },
            new Education
            {
              Id = 1,
              Description = "London school of economics, 2008",
            },
        },
        Languages = new ObservableCollection<Language>
        {
          new Language
          {
            Id = 1,
            Name = availableLanguages[5],
            Level = languageLevels[1],
          },
          new Language
          {
            Id = 1,
            Name = availableLanguages[2],
            Level = languageLevels[3],
          }
        },
      };
    }

    private List<string>? LoadAvailableLanguages()
    {
      string applicationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

      string[]? availableLanguages = File.ReadAllLines(Path.Combine(applicationPath, "Resources", "Languages.txt"));
      return new List<string>(availableLanguages);
    }

    private List<string>? LoadNationalities()
    {
      string applicationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

      string[]? nationalities = File.ReadAllLines(Path.Combine(applicationPath, "Resources", "Nationalities.txt"));
      return new List<string>(nationalities);
    }

    private async Task ChangeAllCvsAsync()
    {
      PasswordDialog view = new PasswordDialog
      {
        DataContext = this.PasswordDialogViewModel
      };

      this.PasswordDialogViewModel.Message = "Please provide password in order to proceed:";
      object result = await DialogHost.Show(view, "MainDialogHost", null, this.ChangeAllNotificationClosingEventHandler);
    }

    private void ChangeAllNotificationClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
    {
      this.PasswordDialogViewModel.NotificationMessage = "";

      if ((bool)eventArgs.Parameter == false)
      {
        return;
      }
      eventArgs.Cancel();

      if (this.PasswordDialogViewModel.Password == "1234")
      {
        Task.Delay(TimeSpan.FromSeconds(0)).ContinueWith((t, _) => eventArgs.Session.Close(false), this.PerformChangeOfAllCvsAsync(), TaskScheduler.FromCurrentSynchronizationContext());
      }

      this.PasswordDialogViewModel.NotificationMessage = "Wrong password, try again!";
    }

    private Task PerformChangeOfAllCvsAsync()
    {
      throw new NotImplementedException();
    }

    private void AddNewEducation()
    {
      this.resumeData.Educations.Add(new Education { Id = 1, Description = "" });
    }

    private void RemoveEducation()
    {
      this.resumeData.Educations.RemoveAt(this.ResumeData.Educations.Count - 1);
    }

    private bool CanRemoveEducation()
    {
      return this.resumeData.Educations.Count > 1;
    }

    private void AddNewLanguage()
    {
      resumeData.Languages.Add(new Language { });
    }

    private void RemoveLanguage()
    {
      this.resumeData.Languages.RemoveAt(this.ResumeData.Languages.Count - 1);
    }

    private bool CanRemoveLanguage()
    {
      return this.resumeData.Languages.Count > 1;
    }

    private async Task LoadFromJsonAsync()
    {
      string destinationPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
      string? fileName = $"{resumeData.FirstName} {resumeData.LastName} CV.json";

      ResumeData? result = await this.JsonDataProvider.LoadFromJsonAsync(Path.Combine(destinationPath, fileName));

      if (result == null)
      {
        this.AlertMessage = "No file found from where to load";
        this.ShowWarning("No file found", this.AlertMessage);
        return;
      }

      this.ResumeData = result;
    }

    private bool CanRemovePosition()
    {
      return this.ResumeData.JobRecords.Count > 1;
    }

    private void RemovePosition()
    {
      this.ResumeData.JobRecords.RemoveAt(this.ResumeData.JobRecords.Count - 1);
    }

    private async Task PrintAsync()
    {
      ConfirmationDialog view = new ConfirmationDialog
      {
        DataContext = this.ConfirmationDialogViewModel
      };

      this.ConfirmationDialogViewModel.NotificationMessage = "Are you sure you want to print CV's?";
      object result = await DialogHost.Show(view, "MainDialogHost", null, this.PrintNotificationClosingEventHandler);
    }

    private void PrintNotificationClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
    {
      this.ConfirmationDialogViewModel.AlertMessage = "";

      if (!Directory.Exists(this.destinationFolderPath))
      {
        Directory.CreateDirectory(this.destinationFolderPath);
      }

      if ((bool)eventArgs.Parameter == false)
      {
        return;
      }
      eventArgs.Cancel();

      string fullPDFDestinationPath = Path.Combine(this.destinationFolderPath, $"CV {this.resumeData.FirstName} {this.resumeData.LastName}.pdf");
      string fullWordDestinationPath = Path.Combine(this.destinationFolderPath, $"CV {this.resumeData.FirstName} {this.resumeData.LastName}.docx");

      if (Directory.Exists(this.destinationFolderPath) && !FileAccessProvider.IsLocked(fullWordDestinationPath) && !FileAccessProvider.IsLocked(fullPDFDestinationPath))
      {
        Task.Delay(TimeSpan.FromSeconds(0)).ContinueWith((t, _) => eventArgs.Session.Close(false), this.PrintCvsAsync(), TaskScheduler.FromCurrentSynchronizationContext());
      }

      if (FileAccessProvider.IsLocked(fullWordDestinationPath))
      {
        this.ConfirmationDialogViewModel.AlertMessage = "File is in use. Close file and try again!";
      }
    }

    private async Task PrintCvsAsync()
    {
      string? fileName = $"CV {this.resumeData.FirstName} {this.resumeData.LastName}.json";

      this.ProgressBarIsRunning = true;
      this.AlertMessage = await WordWriter.WriteToWordTemplate(this.ResumeData, this.destinationFolderPath, this.templateLocationPath);
      await this.JsonDataProvider.SaveToJsonAsync(this.ResumeData, Path.Combine(this.destinationFolderPath, fileName));
      this.ProgressBarIsRunning = false;
      this.ShowSuccess("Files are ready", this.AlertMessage);

      this.StartTimer();
    }

    private void AddNewPosition()
    {
      this.ResumeData.JobRecords.Add(new JobRecord { FromDate = DateTime.Today, ToDate = DateTime.Today });
    }

    private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      this.AlertMessage = "";
      this.StopTimer();

      if (!this.RequestStop)
      {
        this.Timer.Start();  // restart the timer
      }
    }

    private void StopTimer()
    {
      this.RequestStop = true;
      this.Timer.Stop();
    }

    private void StartTimer()
    {
      this.RequestStop = false;
      this.Timer.Start();
    }

    private void ShowNotification(string title, string message)
    {
      NotificationContent? content = new NotificationContent
      {
        Title = title,
        Message = message,
        Type = NotificationType.Information,
      };
      NotificationManager.Show(content);
    }

    private void ShowWarning(string title, string message)
    {
      NotificationContent? content = new NotificationContent
      {
        Title = title,
        Message = message,
        Type = NotificationType.Warning,
      };
      NotificationManager.Show(content);
    }

    private void ShowSuccess(string title, string message)
    {
      NotificationContent? content = new NotificationContent
      {
        Title = title,
        Message = message,
        Type = NotificationType.Success,
      };
      NotificationManager.Show(content);
    }

    private ResumeData resumeData;
    public ResumeData ResumeData
    {
      get => resumeData;
      set
      {
        resumeData = value;
        this.OnPropertyChanged();
      }
    }

    private List<string> nationalities;
    public List<string> Nationalities
    {
      get => nationalities;
      set
      {
        nationalities = value;
        this.OnPropertyChanged();
      }
    }

    private List<string> availableLanguages;
    public List<string> AvailableLanguages
    {
      get => availableLanguages;
      set
      {
        availableLanguages = value;
        this.OnPropertyChanged();
      }
    }

    private List<string> languageLevels;
    public List<string> LanguageLevels
    {
      get => languageLevels;
      set
      {
        languageLevels = value;
        this.OnPropertyChanged();
      }
    }

    private string selectedEmployee;
    public string SelectedEmployee
    {
      get => selectedEmployee;
      set
      {
        selectedEmployee = value;
        this.OnPropertyChanged();
        _ = this.LoadFromJsonAsync();
      }
    }

    private string alertMessage;
    public string AlertMessage
    {
      get => alertMessage;
      set
      {
        alertMessage = value;
        this.OnPropertyChanged();
      }
    }

    private string templateLocationPath;
    public string TemplateLocationPath
    {
      get => templateLocationPath;
      set
      {
        templateLocationPath = value;
        this.OnPropertyChanged();
      }
    }

    private string destinationFolderPath;
    public string DestinationFolderPath
    {
      get => destinationFolderPath;
      set
      {
        destinationFolderPath = value;
        this.OnPropertyChanged();
      }
    }

    private bool progressBarIsRunning;
    public bool ProgressBarIsRunning
    {
      get => progressBarIsRunning;
      set
      {
        progressBarIsRunning = value;
        this.OnPropertyChanged();
      }
    }

    private bool isPresent;
    public bool IsPresent
    {
      get => isPresent;
      set
      {
        isPresent = value;
        this.OnPropertyChanged();
      }
    }

    private ObservableCollection<string> employees;
    public ObservableCollection<string> Employees
    {
      get => employees;
      set
      {
        employees = value;
        this.OnPropertyChanged();
      }
    }

    public ICommand LoadDataCommand => new AsyncRelayCommand(param => this.LoadFromJsonAsync());
    public ICommand AddPositionCommand => new RelayCommand(param => this.AddNewPosition());
    public ICommand RemovePositionCommand => new RelayCommand(param => this.RemovePosition(), canExecute: param => this.CanRemovePosition());
    public ICommand AddEducationCommand => new RelayCommand(param => this.AddNewEducation());
    public ICommand RemoveEducationCommand => new RelayCommand(param => this.RemoveEducation(), canExecute: param => this.CanRemoveEducation());
    public ICommand AddLanguageCommand => new RelayCommand(param => this.AddNewLanguage());
    public ICommand RemoveLanguageCommand => new RelayCommand(param => this.RemoveLanguage(), canExecute: param => this.CanRemoveLanguage());
    public ICommand ChangeAllCVsCommand => new AsyncRelayCommand(param => this.ChangeAllCvsAsync());
    public ICommand PrintCommand => new AsyncRelayCommand(param => this.PrintAsync());

    private static readonly NotificationManager NotificationManager = new();
    private readonly System.Timers.Timer Timer = new System.Timers.Timer();
    private volatile bool RequestStop = false;

    private ConfirmationDialogViewModel ConfirmationDialogViewModel { get; set; }
    private NotificationDialogViewModel NotificationViewModel { get; set; }
    private PasswordDialogViewModel PasswordDialogViewModel { get; set; }
    private ISettingsViewModel SettingsViewModel { get; set; }
    private JsonDataProvider JsonDataProvider { get; set; }
  }
}
