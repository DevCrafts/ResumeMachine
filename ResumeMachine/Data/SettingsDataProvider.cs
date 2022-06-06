using ResumeMachine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResumeMachine.Data
{
  public class SettingsDataProvider : ISettingsDataProvider
  {
    public async Task<List<Setting>> LoadSettingsFromJsonAsync()
    {
      string settingsFolderLocationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
      string settingsLocationPath = Path.Combine(settingsFolderLocationPath, "ResumeMachineSettings.json");

      if (!File.Exists(settingsLocationPath))
      {
        return null;
      }

      using FileStream? fileStream = new FileStream(settingsLocationPath, FileMode.Open, FileAccess.Read);
      List<Setting> setting = await JsonSerializer.DeserializeAsync<List<Setting>>(fileStream);

      if (setting != null)
      {
        return setting;
      }

      return null;
    }

    public async Task SaveSettingsToJsonAsync(List<Setting> settings)
    {
      string settingsFolderLocationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
      string settingsLocationPath = Path.Combine(settingsFolderLocationPath, "ResumeMachineSettings.json");

      using FileStream? stream = File.Create(settingsLocationPath);
      await JsonSerializer.SerializeAsync(stream, settings);
      await stream.DisposeAsync();
    }
  }
}
