using System;
using System.Collections.Generic;

namespace Bespoke.Common
{
	/// <summary>
	/// Container class which implements the IServiceProvider interface. Used to pass shared services between components.
	/// </summary>
	public class ServiceContainer : IServiceProvider
	{
		/// <summary>
        /// Initializes a new instance of the <see cref="ServiceContainer"/> class.
		/// </summary>
		public ServiceContainer()
		{
			mServices = new Dictionary<Type, object>();
		}

        /// <summary>
        /// Add a service to the container.
        /// </summary>
		/// <typeparam name="T">The type of the service.</typeparam>
		/// <param name="service">The service to add.</param>
		public void AddService<T>(T service)
		{
			mServices.Add(typeof(T), service);
		}

        /// <summary>
        /// Find the specified service.
        /// </summary>
		/// <param name="serviceType">The type of the service to find.</param>
		/// <returns>The object associated with the specified service type; otherwise, null if the service is not found.</returns>
		public object GetService(Type serviceType)
		{
			object service;
			mServices.TryGetValue(serviceType, out service);

			return service;
		}

        /// <summary>
        /// Find the specified service.
        /// </summary>
		/// <typeparam name="T">The type of the service to find.</typeparam>
        /// <returns>The object associated with the specified service type; otherwise, null if the service is not found.</returns>
		public T GetService<T>()
		{
			object service;
			mServices.TryGetValue(typeof(T), out service);

			return (T)service;
		}

		/// <summary>
		/// Remove a service from the container.
		/// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
		public void RemoveService(Type serviceType)
		{
			mServices.Remove(serviceType);
		}

		private Dictionary<Type, object> mServices;
	}
}
