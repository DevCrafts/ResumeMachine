using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ResumeMachine.Helper
{
  public static class FileAccessProvider
  {
    [DllImport("kernel32.dll")]
    private static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(string lpFileName, System.UInt32 dwDesiredAccess, System.UInt32 dwShareMode, IntPtr pSecurityAttributes, System.UInt32 dwCreationDisposition, System.UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);

    private static readonly uint GENERIC_WRITE = 0x40000000;
    private static readonly uint OPEN_EXISTING = 3;

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(SafeHandle hObject);

    public static bool BoolFileLocked(string fPath)
    {
      if (!File.Exists(fPath))
      {
        return false;
      }

      SafeHandle sHandle = null;

      try
      {
        sHandle = CreateFile(fPath, GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

        bool inUse = sHandle.IsInvalid;

        return inUse;
      }
      finally
      {
        if (sHandle != null)
        {
          CloseHandle(sHandle);
        }
      }
    }
  }
}
