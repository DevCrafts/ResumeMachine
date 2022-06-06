using ResumeMachine.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ResumeMachine.ViewModels
{
  public interface ISettingsViewModel
  {
    string? FolderPath { get; set; }
    ICommand SelectFolderCommand { get; }
    List<Setting> Settings { get; set; }
    string? TemplatePath { get; set; }
    string? VaultGuid { get; set; }

    event EventHandler<EventArgs> SettingsChanged;
  }
}