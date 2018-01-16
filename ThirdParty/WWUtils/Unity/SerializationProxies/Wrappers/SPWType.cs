using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
 
/// <summary>
/// A Serialization wrapper around a Type variable
/// </summary>
/// 

[System.Serializable]
public class SPWType : ISerializationCallbackReceiver {
    public SPWType()
    {
        type = null;
    }

    public SPWType(Type t)
    {
        type = t;
    }

	public Type type
    {
        get;
        private set;
    }

    [SerializeField]
    [HideInInspector]
    private string fqtn=null;
    [SerializeField]
    [HideInInspector]
    private string assemblyName=null;


    public void OnBeforeSerialize()
    {
        if (type != null)
        {
            fqtn = type.FullName;
            assemblyName = type.Assembly.FullName;
        }
    }

    public void OnAfterDeserialize()
    {
        if (fqtn != null)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            type = assembly.GetType(fqtn);
        }
    }
}
