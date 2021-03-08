using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Injector : MonoBehaviour
{
    private static List<Container> Instances = new List<Container>();

    public static void RegisterContainer<T, U>(U instance) where U : class, T
    {
        if (Instances.Where(i => i.IsOfType(instance)).FirstOrDefault() == null)
        {
            Instances.Add(new Container() { Contract = typeof(T), Instance = instance });
        }
    }
    public static void RegisterContainer<U>(U instance) where U : class
    {
        if (Instances.Where(i => i.IsOfType(instance)).FirstOrDefault() == null)
        {
            Instances.Add(new Container() { Instance = instance });
        }
    }

    public static T GetInstance<T>()
    {
        var instance = Instances.Where(i => i.IsOfType<T>()).FirstOrDefault();
        if (instance == null) Debug.Log($"Couldn't find instance of type - {nameof(T)}");
        return (T)instance.Instance;
    }

}

public class Container
{
    public Type Contract { get; set; }
    public object Instance { get; set; }

    public bool IsOfType(object obj)
    {
        return Contract != null ? obj.GetType().IsAssignableFrom(Contract) || obj.GetType().IsAssignableFrom(Instance.GetType()) : obj.GetType().IsAssignableFrom(Instance.GetType());
    }

    public bool IsOfType<T>()
    {
        return Contract != null ? typeof(T) == Contract || typeof(T) == Instance.GetType() : typeof(T) == Instance.GetType();
    }
}