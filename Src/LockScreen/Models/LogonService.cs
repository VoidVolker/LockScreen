using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using LockScreen.DataTypes.Enums;
using LockScreen.Exceptions;

namespace LockScreen.Models
{
    public class LogonServiceApp : Process
    {
        #region Public Constructors

        public LogonServiceApp()
        {
            FullPath = StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Dir, AppName);
            IsExists = Path.Exists(FullPath);
            //if (!Path.Exists(FullPath))
            //{
            //    throw new LogonServiceNotFoundException(FullPath);
            //}
            StartInfo.RedirectStandardOutput = true;
            StartInfo.RedirectStandardError = true;
            StartInfo.CreateNoWindow = true;
            StartInfo.UseShellExecute = false;
            EnableRaisingEvents = true;
        }

        private readonly bool IsExists;

        #endregion Public Constructors

        #region Public Fields

        public readonly string FullPath;

        #endregion Public Fields

        #region Private Fields

        private const string AppName = "LogonService.exe";
        private const string Dir = "LogonService";

        #endregion Private Fields

        #region Public Methods

        [SuppressMessage("Style", "IDE0046:Преобразовать в условное выражение", Justification = "<Ожидание>")]
        public ServiceState GetStatus()
        {
            if (!IsExists) { return ServiceState.NotFound; }

            RunCommand("status");
            string result = StandardOutput.ReadToEnd().Trim();
            if (Enum.TryParse(result, out ServiceState state))
            {
                return state;
            }

            throw new UnknownResponseException(result);
        }

        public void InstallCommand()
        {
            //StartInfo.CreateNoWindow = false;
            RunCommand("install");
        }

        public void StartCommand()
        {
            //StartInfo.CreateNoWindow = false;
            RunCommand("start");
        }

        public void StopCommand()
        {
            //StartInfo.CreateNoWindow = false;
            RunCommand("stop");
        }

        public void UninstallCommand()
        {
            //StartInfo.CreateNoWindow = false;
            RunCommand("uninstall");
        }

        #endregion Public Methods

        #region Private Methods

        private void RunCommand(string command)
        {
            if (!IsExists) { return; }

            StartInfo.Arguments = $"-{command}";
            Start();
            WaitForExit();
        }

        #endregion Private Methods
    }
}
