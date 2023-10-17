using System;
using System.Collections.Generic;
using UnityEngine;

public static class DependencyManager
{
    private static Dictionary<Type, MonoBehaviour> services = new Dictionary<Type, MonoBehaviour>();

    /// <summary>
    /// Get dependencies, make sure to call after awake
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dependency"></param>
    /// <returns></returns>
    public static bool GetDependency<T>(out T dependency) where T : MonoBehaviour
    {
        if (services.ContainsKey(typeof(T)))
        {
            dependency = (T)services[typeof(T)];
            return true;
        }

        dependency = null;
        return false;

    }

    /// <summary>
    /// Call this on Awake
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dependency"></param>
    public static void SetDependency<T>(T dependency) where T: MonoBehaviour
    {
        if(services.ContainsKey(typeof(T)))
        {
            services[typeof(T)] = dependency;
            return;
        }

        services.Add(typeof(T), dependency);
    }

    public static void CleanUp() => services.Clear();

}
