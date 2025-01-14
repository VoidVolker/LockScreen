using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using static Wallpaper.Tools.Info;

namespace LockScreen.Tools
{
    /// <summary>
    /// Information about application, working and temp directories
    /// </summary>
    public static class Info
    {
        #region Public Properties

        /// <summary>
        /// Company name
        /// </summary>
        public static string AppCompany { get; private set; }

        /// <summary>
        /// Application location directory
        /// </summary>
        public static string AppDirectory { get; private set; }

        /// <summary>
        /// Application name
        /// </summary>
        public static string AppName { get; private set; }

        /// <summary>
        /// Application full title
        /// </summary>
        public static string AppTitle { get; private set; }

        /// <summary>
        /// Full application name and version (version A.B.C.D)
        /// </summary>
        public static string AppTitleAboutVer { get; private set; }

        /// <summary>
        /// Full application name and version (v.A.B.C.D)
        /// </summary>
        public static string AppTitleVer { get; private set; }

        /// <summary>
        /// Application version as string
        /// </summary>
        public static string AppVersion { get => Version.ToString(); }

        /// <summary>
        /// Assembly.GetExecutingAssembly()
        /// </summary>
        public static Assembly Assembly { get; private set; }

        /// <summary>
        /// Data directory
        /// </summary>
        public static string CommonAppData { get; private set; }

        /// <summary>
        /// Application repository URL
        /// </summary>
        public static Uri RepositoryUrl { get; private set; }

        /// <summary>
        /// Application version
        /// </summary>
        public static Version Version { get; private set; }

        #endregion Public Properties

        #region Public Fields

        ///// <summary>
        ///// x86 or x64?
        ///// </summary>
        //public static readonly bool Is86 = nint.Size == 4;

        /// <summary>
        /// Platform CPU architecture
        /// </summary>
#if PLATFORM_X86
        public const CPUArch CPUArchitecture = CPUArch.x86;
#elif PLATFORM_X64
        public const CPUArch CPUArchitecture = CPUArch.x64;
#elif PLATFORM_ARM32
        public const CPUArch CPUArchitecture = CPUArch.arm32;
#elif PLATFORM_ARM64
        public const CPUArch CPUArchitecture = CPUArch.arm64;
#endif

        /// <summary>
        /// Platform CPU architecture string
        /// </summary>
        public static readonly string CPUArchitectureString = CPUArchitecture.ToString();

        /// <summary>
        /// Is module was initialized (automatic module initialization)
        /// </summary>
        public static readonly bool IsInitialized = Init();

        #endregion Public Fields

        #region Private Fields

        private const string MetaKey_RepositoryUrl = "RepositoryUrl";

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Get fullpath for work file in workdir
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static string WorkFile(
            string p1,
            string p2 = null,
            string p3 = null
        )
        {
            if (p2 == null)
            {
                return Path.Combine(CommonAppData, p1);
            }
            else if (p3 == null)
            {
                return Path.Combine(CommonAppData, p1, p2);
            }
            return Path.Combine(CommonAppData, p1, p2, p3);
        }

        #endregion Public Methods

        #region Private Methods

        private static bool Init()
        {
            Assembly = Assembly.GetExecutingAssembly();
            AppCompany = Assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            AppName = Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            AppTitle = Assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            Version = new Version(FileVersionInfo.GetVersionInfo(Assembly.Location).FileVersion);
            AppTitleVer = AppTitle + " v." + Version;
            AppTitleAboutVer = AppTitle + " version " + Version;

            AppDirectory = Path.GetDirectoryName(Assembly.Location);

            var meta = Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().ToList();
            if (meta.FirstOrDefault(a => a.Key == MetaKey_RepositoryUrl) is AssemblyMetadataAttribute attr)
            {
                RepositoryUrl = new Uri(attr.Value);
            }

            // Data dir init
            CommonAppData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                AppCompany,
                AppName
            );

            if (!Directory.Exists(CommonAppData))
            {
                Directory.CreateDirectory(CommonAppData);
            }

            return true;
        }

        #endregion Private Methods

        public enum CPUArch : byte
        {
            x86,
            x64,
            arm32,
            arm64
        }

        ///// <summary>
        ///// Version
        ///// </summary>
        //public class Version : IComparable<Version>
        //{
        //    /// <summary>
        //    /// Version
        //    /// </summary>
        //    /// <param name="version"></param>
        //    public Version(string version)
        //    {
        //        string[] versions = version.Split('.') ?? throw new Exception("Can't parse version from string: " + version);

        // if (versions.Length != 4) { throw new Exception("Can't parse version from string — wrong
        // count of numbers: " + version); }

        // Major = int.TryParse(versions[0], out int major) ? major : throw new Exception("Can't
        // parse major version from string: " + versions[0]);

        // Minor = int.TryParse(versions[1], out int minor) ? minor : throw new Exception("Can't
        // parse minor version from string: " + versions[1]);

        // Build = int.TryParse(versions[2], out int build) ? build : throw new Exception("Can't
        // parse build version from string: " + versions[2]);

        // Revision = int.TryParse(versions[3], out int revisison) ? revisison : throw new
        // Exception("Can't parse revisison version from string: " + versions[3]);

        // hash = ToString().GetHashCode(); }

        // ///
        // <summary>
        // /// Build number ///
        // </summary>
        // public readonly int Build = 0;

        // ///
        // <summary>
        // /// Major version ///
        // </summary>
        // public readonly int Major = 0;

        // ///
        // <summary>
        // /// Minor version ///
        // </summary>
        // public readonly int Minor = 0;

        // ///
        // <summary>
        // /// Revision ///
        // </summary>
        // public readonly int Revision = 0;

        // private readonly int hash;

        // /// <summary> /// Less? /// </summary> /// <param name="left"></param> /// <param
        // name="right"></param> /// <returns></returns> public static bool operator <(Version left,
        // Version right) { return left.CompareTo(right) == -1; }

        // /// <summary> /// Less or equal? /// </summary> /// <param name="left"></param> ///
        // <param name="right"></param> /// <returns></returns> public static bool operator
        // <=(Version left, Version right) { return left.CompareTo(right) <= 0; }

        // ///
        // <summary>
        // /// Bigger? ///
        // </summary>
        // ///
        // <param name="left"></param>
        // ///
        // <param name="right"></param>
        // ///
        // <returns></returns>
        // public static bool operator &gt;(Version left, Version right) { return
        // left.CompareTo(right) == 1; }

        // ///
        // <summary>
        // /// Bigger or equal? ///
        // </summary>
        // ///
        // <param name="left"></param>
        // ///
        // <param name="right"></param>
        // ///
        // <returns></returns>
        // public static bool operator &gt;=(Version left, Version right) { return
        // left.CompareTo(right) &gt;= 0; }

        // /// <summary> /// Versions compare /// </summary> /// <param name="other"></param> ///
        // <returns></returns> public int CompareTo(Version other) { if (Major > other.Major) {
        // return 1; } if (Major < other.Major) { return -1; } // Major == other.Major if (Minor >
        // other.Minor) { return 1; } if (Minor < other.Minor) { return -1; } // Minor ==
        // other.Minor if (Build > other.Build) { return 1; } if (Build < other.Build) { return -1;
        // } // Build == other.Build if (Revision > other.Revision) { return 1; } if (Revision <
        // other.Revision) { return -1; } // Revision == other.Revision return 0; }

        // ///
        // <summary>
        // /// Get hash ///
        // </summary>
        // ///
        // <returns></returns>
        // public override int GetHashCode() { return hash; }

        //    /// <summary>
        //    /// Convert version to string
        //    /// </summary>
        //    /// <returns></returns>
        //    public override sealed string ToString()
        //    {
        //        return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Build, Revision);
        //    }
        //}
    }
}
