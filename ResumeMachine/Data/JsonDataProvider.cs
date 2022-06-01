using ResumeMachine.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResumeMachine.Data
{
  public class JsonDataProvider : IJsonDataProvider
  {
    public async Task<ResumeData?> LoadFromJsonAsync(string destinationPath)
    {
      if (!File.Exists(destinationPath))
      {
        return null;
      }

      using FileStream? fileStream = new FileStream(destinationPath, FileMode.Open, FileAccess.Read);
      ResumeData? resumeData = await JsonSerializer.DeserializeAsync<ResumeData>(fileStream);

      if (resumeData != null)
      {
        return resumeData;
      }

      return null;
    }

    public async Task SaveToJsonAsync(ResumeData resumeData, string destinationPath)
    {
      using FileStream? stream = File.Create(destinationPath);
      await JsonSerializer.SerializeAsync(stream, resumeData);
      await stream.DisposeAsync();
    }
  }
}