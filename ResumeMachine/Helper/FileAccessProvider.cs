using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.Helper
{
  public static class FileAccessProvider
  {
    public static bool IsLocked(string filePath)
    {
      FileInfo file = new FileInfo(filePath);

      try
      {
        string fpath = file.FullName;
        FileStream fs = File.OpenWrite(fpath);
        fs.Close();
        return false;
      }

      catch (Exception) { return true; }
    }
  }
}
