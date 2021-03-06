﻿using System;
using System.Diagnostics;

namespace ClangPowerTools
{
  public static class PowerShellWrapper
  {
    #region Properties

    public static DataReceivedEventHandler DataErrorHandler { get; set; }
    public static DataReceivedEventHandler DataHandler { get; set; }
    public static EventHandler ExitedHandler { get; set; }

    #endregion

    #region Public Methods

    public static Process Invoke(string aScript, RunningProcesses aRunningProcesses)
    {
      Process process = new Process();
      try
      {
        process.StartInfo = new ProcessStartInfo()
        {
          FileName = $"{Environment.SystemDirectory}\\{ScriptConstants.kPowerShellPath}",
          RedirectStandardError = true,
          RedirectStandardOutput = true,
          CreateNoWindow = true,
          UseShellExecute = false,
          Arguments = aScript
        };

        process.EnableRaisingEvents = true;
        process.ErrorDataReceived += DataErrorHandler;
        process.OutputDataReceived += DataHandler;
        process.Exited += ExitedHandler;
        process.Disposed += ExitedHandler;

        aRunningProcesses.Add(process);

        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
        process.WaitForExit();
      }
      catch (Exception)
      {
      }
      finally
      {
        process.ErrorDataReceived -= DataErrorHandler;
        process.OutputDataReceived -= DataHandler;
        process.Exited -= ExitedHandler;
        process.EnableRaisingEvents = false;

      }
      return process;
    }

    #endregion

  }
}

