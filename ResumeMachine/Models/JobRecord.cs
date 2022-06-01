using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.Models
{
  public class JobRecord
  {
    public int Id { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string? PositionTitle { get; set; }
    public string? CompanyName { get; set; }
    public string? JobDescription { get; set; }
    public bool IsPresent { get; set; }
  }
}
