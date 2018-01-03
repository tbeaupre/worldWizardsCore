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
    private string fqtn;
    [SerializeField]
    private string assemblyName;


    public void OnBeforeSerialize()
    {
        fqtn = type.FullName;
        assemblyName = type.Assembly.FullName;
    }

    public void OnAfterDeserialize()
    {
        Assembly assembly = Assembly.Load(assemblyName);
        type = assembly.GetType(fqtn);
    }
}
