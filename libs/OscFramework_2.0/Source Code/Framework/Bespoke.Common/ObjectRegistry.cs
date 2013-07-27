using System;
using System.Collections.Generic;

namespace Bespoke.Common
{
    /// <summary>
    /// General-purpose container for associating objects with string names.
    /// </summary>
    public class ObjectRegistry
    {
        /// <summary>
        /// Gets the object associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the object to get.</param>
        /// <return> The object associated with the specified name. If the specified name is not
        /// found, a <see cref="KeyNotFoundException"/> is thrown.</return>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="KeyNotFoundException"><paramref name="name"/> does not exist in the collection.</exception>
        public object this[string name]
        {
            get
            {
                return mObjects[name];
            }
        }

        /// <summary>
        /// Gets the list of objects contained in the registry.
        /// </summary>
        public object[] Objects
        {
            get
            {
                object[] objects = new object[mObjects.Values.Count];
                mObjects.Values.CopyTo(objects, 0);

                return objects;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectRegistry"/> class.
        /// </summary>
        public ObjectRegistry()
        {
            mObjects = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the object associated with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of object expected to be stored with the specified name.</typeparam>
        /// <param name="name">The name of the object to get.</param>
        /// <return> The object associated with the specified name. If the specified name is not found, null is returned.</return>
        /// <exception cref="InvalidCastException"><paramref name="name"/> exists, but does not store specified type.</exception>
        public T GetRegisteredObject<T>(string name)
        {
            object value;

            if (mObjects.ContainsKey(name))
            {
                value = mObjects[name];
                if ((value != null) && (value is T == false))
                {
                    throw new InvalidCastException("Name [" + name.ToString() + "] does not store type: " + typeof(T).ToString());
                }
            }
            else
            {
                value = null;
            }

            return (T)value;
        }

        /// <summary>
        /// Gets the object associated with the type.
        /// </summary>
        /// <typeparam name="T">The type of object to search on.</typeparam>
        /// <return> The object associated with the specified type. If the specified type is not found, null is returned.</return>
        /// <exception cref="InvalidCastException">object exists with the associated type name, but does not store specified type.</exception>
        /// <remarks>Uses the type name as the key associated with the object.</remarks>
        public T GetRegisteredObject<T>()
        {
            Type type = typeof(T);
            return GetRegisteredObject<T>(type.Name);
        }

        /// <summary>
        /// Add an object to the registry.
        /// </summary>
        /// <param name="value">The object to add.</param>
        /// <remarks>Uses the type name as the key associated with the object.
        /// Overwrites any previously registered object associated with the same name.</remarks>
        public void RegisterObject(object value)
        {
            string typeName = value.GetType().Name;
            RegisterObject(typeName, value);
        }

        /// <summary>
        /// Add an object to the registry.
        /// </summary>
        /// <param name="name">The name to associate with the object.</param>
        /// <param name="value">The object to add.</param>
        /// <remarks>Overwrites any previously registered object associated with the same name.</remarks>
        public void RegisterObject(string name, object value)
        {
            mObjects[name] = value;
        }

        /// <summary>
        /// Remove an object from the registry.
        /// </summary>
        /// <param name="type">The type of the object to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <remarks>Uses the type name as the key associated with the object.</remarks>
        public void UnregisterObject(Type type)
        {
            Assert.ParamIsNotNull(type);

            UnregisterObject(type.Name);
        }

        /// <summary>
        /// Remove an object from the registry.
        /// </summary>
        /// <param name="name">The name of the object to remove.</param>
        public void UnregisterObject(string name)
        {
            mObjects.Remove(name);
        }

        private Dictionary<string, object> mObjects;
    }
}
