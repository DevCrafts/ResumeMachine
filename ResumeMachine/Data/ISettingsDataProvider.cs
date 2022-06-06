using ResumeMachine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResumeMachine.Data
{
  public interface ISettingsDataProvider
  {
    Task<List<Setting>> LoadSettingsFromJsonAsync();
    Task SaveSettingsToJsonAsync(List<Setting> settings);
  }
}