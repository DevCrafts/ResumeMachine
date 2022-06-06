using Microsoft.Win32;
using ResumeMachine.Commands;
using ResumeMachine.Data;
using ResumeMachine.Helper;
using ResumeMachine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ResumeMachine.ViewModels
{
  public class SettingsViewModel : BaseViewModel, ISettingsViewModel
  {
    public SettingsViewModel()
    {
      this.SettingsDataProvider = new SettingsDataProvider();

      this.LoadSettings();
    }

    private void SelectFolderPath()
    {
      FolderPicker? openFolderDialog = new FolderPicker();
      openFolderDialog.InputPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

      if (openFolderDialog.ShowDialog() == true)
      {
        this.FolderPath = openFolderDialog.ResultPath;
      }
    }

    private void SaveSettings()
    {
      List<Setting> settings = new List<Setting>();

      settings.Add(new Setting { Name = nameof(this.folderPath), Value = this.folderPath, });
      settings.Add(new Setting { Name = nameof(this.vaultGuid), Value = this.vaultGuid, });
      settings.Add(new Setting { Name = nameof(this.templatePath), Value = this.templatePath, });

      Task.Run(async () => await this.SaveSettingsAsync(settings));
    }

    private void LoadSettings()
    {
      Task.Run(async () => await this.LoadSettingsAsync());
    }

    private async Task SaveSettingsAsync(List<Setting> settings)
    {
      await this.SettingsDataProvider.SaveSettingsToJsonAsync(settings);
    }

    private async Task LoadSettingsAsync()
    {
      string templateLocationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

      this.settings = await this.SettingsDataProvider.LoadSettingsFromJsonAsync();

      this.FolderPath = this.settings.FirstOrDefault(w => w.Name == nameof(this.folderPath)).Value;
      this.VaultGuid = this.settings.FirstOrDefault(w => w.Name == nameof(this.vaultGuid)).Value;
      this.TemplatePath = this.settings.FirstOrDefault(w => w.Name == nameof(this.templatePath)).Value;

      if (string.IsNullOrEmpty(this.TemplatePath))
      {
        this.TemplatePath = Path.Combine(templateLocationPath, "Resources", "CV Template", "template.docx");
      }

      if (string.IsNullOrEmpty(this.FolderPath))
      {
        this.folderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
      }
    }

    private void SelectTemplatePath()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() == true)
      {
        this.TemplatePath = openFileDialog.FileName;
      }
    }

    private List<Setting> settings;
    public List<Setting> Settings
    {
      get => settings;
      set
      {
        settings = value;
        this.OnPropertyChanged();
        this.LoadSettings();
      }
    }

    private string? folderPath;
    public string? FolderPath
    {
      get => folderPath;
      set
      {
        folderPath = value;
        this.OnPropertyChanged();
        this.SaveSettings();
        this.OnSettingsChanged();
      }
    }

    private string? templatePath;
    public string? TemplatePath
    {
      get => templatePath;
      set
      {
        templatePath = value;
        this.OnPropertyChanged();
        this.SaveSettings();
        this.OnSettingsChanged();
      }
    }

    private string? vaultGuid;
    public string? VaultGuid
    {
      get => vaultGuid;
      set
      {
        vaultGuid = value;
        this.OnPropertyChanged();
        this.SaveSettings();
        this.OnSettingsChanged();
      }
    }

    public ICommand SelectFolderCommand => new RelayCommand(param => this.SelectFolderPath());
    public ICommand SelectTemplateCommand => new RelayCommand(param => this.SelectTemplatePath());

    public event EventHandler<EventArgs> SettingsChanged;
    protected virtual void OnSettingsChanged() => this.SettingsChanged?.Invoke(this, new EventArgs());

    private SettingsDataProvider SettingsDataProvider { get; }
  }
}
