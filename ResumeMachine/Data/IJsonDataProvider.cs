using ResumeMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.Data
{
  public interface IJsonDataProvider
  {
    Task<ResumeData?> LoadFromJsonAsync(string destinationPath);
    Task SaveToJsonAsync(ResumeData resumeData, string destinationPath);
  }
}
