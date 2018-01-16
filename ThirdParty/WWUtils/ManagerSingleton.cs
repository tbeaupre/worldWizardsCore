using UnityEngine;
using System.Collections;

public class ManagerSingleton<ChildType>: MonoBehaviour where ChildType:ManagerSingleton<ChildType>{
    public static ChildType _instance;

    public static ChildType Instance
    {
        get
        {
            if (_instance != null) {
                return _instance;
            } else
            {
                GameObject parent = GameObject.Find("Managers");
                if (parent == null)
                {
                    parent = new GameObject("Managers");
                }
                GameObject go = new GameObject(typeof(ChildType).Name);
                _instance = go.AddComponent<ChildType>();
                return _instance;
            }
        }
    }
	
}
