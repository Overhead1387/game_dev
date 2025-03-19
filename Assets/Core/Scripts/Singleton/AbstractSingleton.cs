using UnityEngine;

namespace HyperCasual.Core
{
    /// <summary>
    /// Base class implementing the thread-safe Singleton pattern for MonoBehaviour components.
    /// Provides automatic initialization and guaranteed single instance across the application.
    /// </summary>
    /// <typeparam name="T">The type of the singleton component. Must derive from Component.</typeparam>
    public abstract class AbstractSingleton<T> : MonoBehaviour where T : Component
    {
        private static T s_Instance;
        private static readonly object s_Lock = new object();
        private static bool s_IsInitialized;

        /// <summary>
        /// Global access point to the singleton instance.
        /// Automatically creates a new instance if none exists.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!s_IsInitialized)
                {
                    lock (s_Lock)
                    {
                        if (!s_IsInitialized)
                        {
                            s_Instance = FindObjectOfType<T>();
                            if (s_Instance == null)
                            {
                                GameObject obj = new GameObject(typeof(T).Name);
                                s_Instance = obj.AddComponent<T>();
                                DontDestroyOnLoad(obj);
                            }
                            s_IsInitialized = true;
                        }
                    }
                }

                return s_Instance;
            }
        }

        /// <summary>
        /// Ensures singleton instance uniqueness during MonoBehaviour initialization.
        /// </summary>
        protected virtual void Awake()
        {
            if (s_Instance == null)
            {
                s_Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}