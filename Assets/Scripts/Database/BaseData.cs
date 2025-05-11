using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.BaseSystem.DataStores.ScriptableObjects
{
    /// <summary>
    /// ScriptableObjectで管理されるデータのベース
    /// </summary>
    /// // Start is called before the first frame update
    public abstract class BaseData : ScriptableObject
    {
        public string Name
        {
            get => name;
            set => name = value;
        }
        [SerializeField]
        private new string name;
        
        public int Id => id;
        [SerializeField]
        private int id;
    }
}
