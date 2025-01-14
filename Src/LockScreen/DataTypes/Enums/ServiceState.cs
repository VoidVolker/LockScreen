namespace LockScreen.DataTypes.Enums
{
    public enum ServiceState : byte
    {
        /// <summary>
        /// Service state not available
        /// </summary>
        None = 0,

        /// <summary>
        /// Service not installed
        /// </summary>
        NotInstalled,

        /// <summary>
        /// The service is not running. This corresponds to the Win32 SERVICE_STOPPED constant,
        /// which is defined as 0x00000001.
        /// </summary>
        Stopped,

        /// <summary>
        /// The service is starting. This corresponds to the Win32 SERVICE_START_PENDING constant,
        /// which is defined as 0x00000002.
        /// </summary>
        StartPending,

        /// <summary>
        /// The service is stopping. This corresponds to the Win32 SERVICE_STOP_PENDING constant,
        /// which is defined as 0x00000003.
        /// </summary>
        StopPending,

        /// <summary>
        /// The service is running. This corresponds to the Win32 SERVICE_RUNNING constant, which is
        /// defined as 0x00000004.
        /// </summary>
        Running,

        /// <summary>
        /// The service continue is pending. This corresponds to the Win32 SERVICE_CONTINUE_PENDING
        /// constant, which is defined as 0x00000005.
        /// </summary>
        ContinuePending,

        /// <summary>
        /// The service pause is pending. This corresponds to the Win32 SERVICE_PAUSE_PENDING
        /// constant, which is defined as 0x00000006.
        /// </summary>
        PausePending,

        /// <summary>
        /// The service is paused. This corresponds to the Win32 SERVICE_PAUSED constant, which is
        /// defined as 0x00000007.
        /// </summary>
        Paused,

        /// <summary>
        /// Service is in process of installation
        /// </summary>
        Installing,

        /// <summary>
        /// Service is in process of uninstallation
        /// </summary>
        Uninstalling,

        /// <summary>
        /// Service not founded at expected location
        /// </summary>
        NotFound
    }
}
