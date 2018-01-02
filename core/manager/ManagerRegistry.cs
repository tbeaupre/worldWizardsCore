using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WorldWizards.core.manager
{
    /// <summary>
    ///     Manager Registry has references to all managers.
    /// </summary>
    public class ManagerRegistry : Singleton<ManagerRegistry>
    {
        // holds lists of reigsitered object by their inetrface and base class types
        Dictionary<Type, List<object>> registry = new Dictionary<Type, List<object>>(); 

        protected ManagerRegistry()
        {
        } // guarantee this will be always a singleton only - can't use the constructor!


        //For compatability, future code should call registry directly
        public SceneGraphManager sceneGraphManager {
            get { return GetAnInstance<SceneGraphManager>(); }
        }

        /// <summary>
        /// This method registers an object  with the registry so it can be efficeintly found
        /// by either an interface it implements or a class it inherits from (or its own class)
        /// </summary>
        /// <param name="obj"> The object to register</param>
        public void Register(object obj)
        {
            // add to all interface lists
            foreach(Type ti in obj.GetType().GetInterfaces())
            {

                if (!registry.ContainsKey(ti))
                {
                    registry.Add(ti, new List<object>());
                }
                registry[ti].Add(obj);
            }
            // add to all type lists
            Type t = obj.GetType();
            while (t != null)
            {
                if (!registry.ContainsKey(t))
                {
                    registry.Add(t, new List<object>());
                }
                registry[t].Add(obj);
                t = t.BaseType;
            }
        }

        /// <summary>
        /// This method queries the registry for an object that imnplements the passed type which can be
        /// either an interface or a class
        /// </summary>
        /// <typeparam name="T">The type the retruned object must match</typeparam>
        /// <returns>The first registered matching object, or null if none can be found</returns>
        public T GetAnInstance<T>() where  T: class // restricts it to ref tyoes, whsoe edfault is always null
        {
            Type t = typeof(T);
            if (registry.ContainsKey(t))
            {
                if (registry[t].Count > 0)
                {
                    return (T)registry[t][0];
                }
            }

            return default(T);
        }


        /// <summary>
        /// This method queries the registry for a list of all objects that imnplement the passed type which can be
        /// either an interface or a class
        /// </summary>
        /// <typeparam name="T">The type the retruned objects must match</typeparam>
        /// <returns>The list of mathcing objects, or null if none can be found</returns>
        public IEnumerable<T> GetAllInstances<T>() where T : class // restricts it to ref tyoes, whsoe default is always null
        {
            Type t = typeof(T);
            if (registry.ContainsKey(t))
            {
                return registry[t].Cast<T>();
            }      

            return null;
        }

        /// <summary>
        /// This method completely removes the passed object from the registry
        /// </summary>
        /// <param name="obj">The object to remove</param>
        public void Deregister(object obj)
        {
            // add to all interface lists
            foreach (Type ti in obj.GetType().GetInterfaces())
            {
                if (registry.ContainsKey(ti))
                {
                    if (registry[ti].Contains(obj))
                    {
                        registry[ti].Remove(obj);
                    }
                }
            }
            // add to all type lists
            Type t = obj.GetType();
            while (t != null)
            {
                if (registry.ContainsKey(t))
                {
                    if (registry[t].Contains(obj))
                    {
                        registry[t].Remove(obj);
                    }
                }
                t = t.BaseType;
            }
        }


        // Use this for initialization
        void Start()
        {
           
            
            TypeHelper.ForAllTypes<Manager>(type =>
            {
                if (!type.IsAbstract)
                {
                    Debug.Log("Registering " + type.Name);
                    Register(Activator.CreateInstance(type));
                   
                }
            });            
        }
    }
}