using System.Collections.Generic;
using UnityEngine;

namespace WorldWizards.WWUtils.Unity.SerializationProxies.Templates
{
    /// <summary>
    /// USed to create serializable dictionaries.
    /// Not that Generics cannot be Unity serializable, so this class must be concretized in a sub class
    /// to use with Unity.   That Conretization should be tagged with {System.Serializable]
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SPDictionaryTemplate<K,V> : Dictionary<K,V>,ISerializationCallbackReceiver  {
        [SerializeField]
        private K[] keys;
        [SerializeField]
        private V[] values;

        public void OnAfterDeserialize()
        {
            for(int i = 0; i < keys.Length; i++)
            {
                Add(keys[i], values[i]);
            }
            //return memory
            keys = null;
            values = null;
        }

        public void OnBeforeSerialize()
        {
            List<K> keyList = new List<K>();
            List<V> valueList = new List<V>();
            foreach(KeyValuePair<K,V> kvp in this)
            {
                keyList.Add(kvp.Key);
                valueList.Add(kvp.Value);
            }
            keys = keyList.ToArray();
            values = valueList.ToArray();
        }


    }
}
