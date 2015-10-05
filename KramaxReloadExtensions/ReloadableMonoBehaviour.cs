using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KramaxReloadExtensions
{
    public class ReloadableMonoBehaviour : MonoBehaviour
    {
        public Dictionary<Type, Type> typeMapping = null;

        public MonoBehaviour AddComponentToObject(Type type, GameObject aGameObject)
        {
            Type newType = type;

            if (typeMapping != null)
            {
                try
                {
                    newType = typeMapping[type];
                }
                catch (KeyNotFoundException)
                {
                    Debug.Log(String.Format("ReloadableMonoBehavior.AddComponent: ERROR--no type mapping for {0}", type.Name));
                }
            }
            else
            {
                Debug.Log(String.Format("ReloadableMonoBehavior.AddComponent: ERROR--no type mapping present in parent {0}", this));
            }

            var result = aGameObject.AddComponent(newType) as MonoBehaviour;

            // if it is a reloadable we should propigate our type map
            var reloadable = result as ReloadableMonoBehaviour;

            if (reloadable != null)
            {
                Debug.Log(String.Format("ReloadableMonoBehavior.AddComponent: propigate type mapping for {0}", type.Name));
                reloadable.typeMapping = typeMapping;
            }

            return result;
        }

        public MonoBehaviour AddComponent(Type type)
        {
            return this.AddComponentToObject(type, this.gameObject);
        }
    }
}
