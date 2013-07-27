using System;
using System.ServiceProcess;

namespace Bespoke.Common
{
    /// <summary>
    /// Helper class for managing Windows services.
    /// </summary>
    public static class ServiceManager
    {
        /// <summary>
        /// The amount of time to wait for a service to respond, before giving up.
        /// </summary>
        public static readonly int DefaultTimeout = 5000;

        /// <summary>
        /// Determines if a specified service is running.
        /// </summary>
        /// <param name="serviceName">The name of the service.</param>
        /// <returns>true if the service is running; otherwise, false.</returns>
        public static bool IsServiceRunning(string serviceName)
        {
            bool isServiceRunning;

            try
            {
                ServiceController service = new ServiceController(serviceName);
                isServiceRunning = (service.Status == ServiceControllerStatus.Running);
            }
            catch
            {
                isServiceRunning = false;
            }

            return isServiceRunning;
        }

        /// <summary>
        /// Start a service.
        /// </summary>
        /// <param name="serviceName">The name of the service to start.</param>
        /// <returns>true if the service was successfully started; otherwise, false.</returns>
        public static bool StartService(string serviceName)
        {
            return StartService(serviceName, DefaultTimeout);
        }

        /// <summary>
        /// Start a service.
        /// </summary>
        /// <param name="serviceName">The name of the service to start.</param>
        /// <param name="timeoutMilliseconds">The amount of time to wait for the service to start, before giving up.</param>
        /// <returns>true if the service was successfully started; otherwise, false.</returns>
        public static bool StartService(string serviceName, int timeoutMilliseconds)
        {
            bool serviceStarted;

            try
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeoutMilliseconds));
                    serviceStarted = true;
                }
                else
                {
                    serviceStarted = false;
                }
            }
            catch
            {
                serviceStarted = false;
            }

            return serviceStarted;
        }

        /// <summary>
        /// Stop a service.
        /// </summary>
        /// <param name="serviceName">The name of the service to stop.</param>
        /// <returns>true if the service was successfully stopped; otherwise, false.</returns>
        public static bool StopService(string serviceName)
        {
            return StopService(serviceName, DefaultTimeout);
        }

        /// <summary>
        /// Stop a service.
        /// </summary>
        /// <param name="serviceName">The name of the service to stop.</param>
        /// <param name="timeoutMilliseconds">The amount of time to wait for the service to stop, before giving up.</param>
        /// <returns>true if the service was successfully stopped; otherwise, false.</returns>
        public static bool StopService(string serviceName, int timeoutMilliseconds)
        {
            bool serviceStopped;

            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeoutMilliseconds));
                serviceStopped = true;
            }
            else
            {
                serviceStopped = false;
            }

            return serviceStopped;
        }

        /// <summary>
        /// Restart a service.
        /// </summary>
        /// <param name="serviceName">The name of the service to restart.</param>
        /// <returns>true if the service was successfully restarted; otherwise, false.</returns>
        public static bool RestartService(string serviceName)
        {
            return RestartService(serviceName, DefaultTimeout);
        }

        /// <summary>
        /// Restart a service.
        /// </summary>
        /// <param name="serviceName">The name of the service to restart.</param>
        /// <param name="timeoutMilliseconds">The amount of time to wait for the service to restart, before giving up.</param>
        /// <returns>true if the service was successfully restarted; otherwise, false.</returns>
        public static bool RestartService(string serviceName, int timeoutMilliseconds)
        {
            bool serviceRestarted;

            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeoutMilliseconds));
            }

            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeoutMilliseconds));
                serviceRestarted = true;
            }
            else
            {
                serviceRestarted = false;
            }

            return serviceRestarted;
        }
    }
}
