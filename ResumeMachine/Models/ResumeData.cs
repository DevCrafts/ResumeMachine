using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.Models
{
  public class ResumeData
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? PresentPosition { get; set; }
    public string? CurrentCompany { get; set; }
    public string? YearsWithCompany { get; set; }
    public ObservableCollection<JobRecord>? JobRecords { get; set; }
    public ObservableCollection<Education>? Educations { get; set; }
    public ObservableCollection<Language>? Languages { get; set; }
  }
}
