using ResumeMachine.Commands;
using ResumeMachine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using ResumeMachine.Helper;
using System.Windows.Threading;
using System.Timers;
using Notification.Wpf;
using System.Windows.Media;

namespace ResumeMachine.ViewModel
{
  public class MainViewModel : BaseViewModel
  {
    private static readonly NotificationManager notificationManager = new();
    private System.Timers.Timer Timer = new System.Timers.Timer();
    private volatile bool RequestStop = false;

    public MainViewModel()
    {
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

    private ResumeData InitializeExampleData()
    {
      return new ResumeData
      {
        FirstName = "John",
        LastName = "Smith",
        PresentPosition = "Director",
        YearsWithCompany = "2",
        CurrentCompany = "Google",
        Nationality = this.nationalities[3],
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
            Name = this.availableLanguages[5],
            Level = this.languageLevels[1],
          },
          new Language
          {
            Id = 1,
            Name = this.availableLanguages[2],
            Level = this.languageLevels[3],
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

    public ICommand LoadDataCommand => new AsyncRelayCommand(async param => await this.LoadFromJsonAsync());
    public ICommand AddPositionCommand => new RelayCommand(param => this.AddNewPosition());
    public ICommand RemovePositionCommand => new RelayCommand(param => this.RemovePosition(), canExecute: param => this.CanRemovePosition());
    public ICommand AddEducationCommand => new RelayCommand(param => this.AddNewEducation());
    public ICommand RemoveEducationCommand => new RelayCommand(param => this.RemoveEducation(), canExecute: param => this.CanRemoveEducation());
    public ICommand AddLanguageCommand => new RelayCommand(param => this.AddNewLanguage());
    public ICommand RemoveLanguageCommand => new RelayCommand(param => this.RemoveLanguage(), canExecute: param => this.CanRemoveLanguage());
    public ICommand ToMFilesCommand => new RelayCommand(param => this.ToMFiles());
    public ICommand ChangeAllCVsCommand => new RelayCommand(param => this.ChangeAllCVs());
    public ICommand SaveToJsonCommand => new AsyncRelayCommand(async param => await this.SaveToJsonAsync());
    public ICommand PrintCommand => new AsyncRelayCommand(async param => await this.PrintAsync());

    private void ChangeAllCVs()
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
      this.resumeData.Languages.Add(new Language { });
    }

    private void RemoveLanguage()
    {
      this.resumeData.Languages.RemoveAt(this.ResumeData.Languages.Count - 1);
    }

    private bool CanRemoveLanguage()
    {
      return this.resumeData.Languages.Count > 1;
    }

    private void ToMFiles()
    {
      throw new NotImplementedException();
    }

    private async Task SaveToJsonAsync()
    {
      string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);

      string? fileName = $"{this.ResumeData.FirstName} {this.ResumeData.LastName} CV.json";
      using FileStream? stream = File.Create(Path.Combine(strPath, fileName));
      await JsonSerializer.SerializeAsync(stream, resumeData);
      await stream.DisposeAsync();
    }

    private async Task LoadFromJsonAsync()
    {
      string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
      string? fileName = $"{this.ResumeData.FirstName} {this.ResumeData.LastName} CV.json";

      if (!File.Exists(Path.Combine(strPath, fileName)))
      {
        this.AlertMessage = "No file found from where to load";
        this.ShowWarning("No file found", this.AlertMessage);
        return;
      }

      using FileStream? fileStream = new FileStream(Path.Combine(strPath, fileName), FileMode.Open, FileAccess.Read);
      ResumeData? dict = await JsonSerializer.DeserializeAsync<ResumeData>(fileStream);

      if (dict != null)
      {
        this.ResumeData.FirstName = dict.FirstName;
        this.ResumeData.LastName = dict.LastName;
        this.ResumeData.DateOfBirth = dict.DateOfBirth;
        this.ResumeData.Nationality = dict.Nationality;
        this.ResumeData.PresentPosition = dict.PresentPosition;
        this.ResumeData.CurrentCompany = dict.CurrentCompany;
        this.ResumeData.YearsWithCompany = dict.YearsWithCompany;
        this.ResumeData.JobRecords = new ObservableCollection<JobRecord>(dict.JobRecords.Select(s => new JobRecord
        {
          Id = s.Id,
          FromDate = s.FromDate,
          ToDate = s.ToDate,
          CompanyName = s.CompanyName,
          JobDescription = s.JobDescription,
          PositionTitle = s.PositionTitle,
          IsPresent = s.IsPresent,
        }));
        this.ResumeData.Educations = new ObservableCollection<Education>(dict.Educations.Select(s => new Education
        {
          Id = s.Id,
          Description = s.Description,
        }));
        this.ResumeData.Languages = new ObservableCollection<Language>(dict.Languages.Select(s => new Language
        {
          Id = s.Id,
          Name = s.Name,
          Level = s.Level,
        }));
      }
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
      this.ProgressBarIsRunning = true;
      this.AlertMessage = await WordWriter.WriteToWordTemplate(this.ResumeData);
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
      notificationManager.Show(content);
    }

    private void ShowWarning(string title, string message)
    {
      NotificationContent? content = new NotificationContent
      {
        Title = title,
        Message = message,
        Type = NotificationType.Warning,
      };
      notificationManager.Show(content);
    }

    private void ShowSuccess(string title, string message)
    {
      NotificationContent? content = new NotificationContent
      {
        Title = title,
        Message = message,
        Type = NotificationType.Success,
      };
      notificationManager.Show(content);
    }

    private ResumeData resumeData;
    public ResumeData ResumeData
    {
      get => this.resumeData;
      set
      {
        this.resumeData = value;
        this.OnPropertyChanged();
      }
    }

    private List<string> nationalities;
    public List<string> Nationalities
    {
      get => this.nationalities;
      set
      {
        this.nationalities = value;
        this.OnPropertyChanged();
      }
    }

    private List<string> availableLanguages;
    public List<string> AvailableLanguages
    {
      get => this.availableLanguages;
      set
      {
        this.availableLanguages = value;
        this.OnPropertyChanged();
      }
    }

    private List<string> languageLevels;
    public List<string> LanguageLevels
    {
      get => this.languageLevels;
      set
      {
        this.languageLevels = value;
        this.OnPropertyChanged();
      }
    }

    private string selectedEmployee;
    public string SelectedEmployee
    {
      get => this.selectedEmployee;
      set
      {
        this.selectedEmployee = value;
        this.OnPropertyChanged();
      }
    }

    private string alertMessage;
    public string AlertMessage
    {
      get => this.alertMessage;
      set
      {
        this.alertMessage = value;
        this.OnPropertyChanged();
      }
    }

    private bool progressBarIsRunning;
    public bool ProgressBarIsRunning
    {
      get => this.progressBarIsRunning;
      set
      {
        this.progressBarIsRunning = value;
        this.OnPropertyChanged();
      }
    }

    private bool isPresent;
    public bool IsPresent
    {
      get => this.isPresent;
      set
      {
        this.isPresent = value;
        this.OnPropertyChanged();
      }
    }

    private ObservableCollection<string> employees;
    public ObservableCollection<string> Employees
    {
      get => this.employees;
      set
      {
        this.employees = value;
        this.OnPropertyChanged();
      }
    }
  }
}
