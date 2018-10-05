using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ObjectManagementSystem
{
    [RequireComponent(typeof(ObjectManager<>))]
    public class ObjectGenerator<T> : MonoBehaviour
    {
        #region Field

        public bool spawn;

        public Transform objectParent;

        public GameObject[] generateObjects;

        public float[] generateObjectsRates;

        #endregion Field

        #region Property

        public ObjectManager<T> ObjectManager 
        {
            get;
            protected set;
        }

        #endregion Property

        #region Method

        public virtual void Awake()
        {
            this.ObjectManager = base.GetComponent<ObjectManager<T>>();
        }

        public virtual GameObject Generate<U>
        (int objectIndex, Vector3 position, Vector3? rotation = null, Vector3? scale = null) where U : ManagedObject<T>
        {
            if (this.ObjectManager.CheckManagedObjectCountIsMax())
            {
                return null;
            }

            // NOTE:
            // Fields which have SyncVar attribute is synced when spawn.
            // So if you need, set these fields before spawn.

            // NOTE:
            // Awake is called when GameObject instantiated.

            GameObject newObject = GameObject.Instantiate(this.generateObjects[objectIndex]);
            newObject.transform.position   = position;
            newObject.transform.rotation   = rotation == null ? newObject.transform.rotation : Quaternion.Euler((Vector3)rotation);
            newObject.transform.localScale = scale == null ? newObject.transform.localScale : (Vector3)scale;
            newObject.transform.parent     = this.objectParent;

            this.ObjectManager.AddManagedObject<U>(newObject);

            if (this.spawn)
            {
                NetworkServer.Spawn(newObject);
            }

            return newObject;
        }

        public virtual GameObject GenerateRandom<U>
        (Vector3 position, Vector3? rotation = null, Vector3? scale = null) where U : ManagedObject<T>
        {
            return Generate<U>(GetIndex(this.generateObjectsRates), position, rotation, scale);
        }

        private int GetIndex(IList<float> rates)
        {
            int index = 0;

            float seedValue = Random.value;

            for (index = 0; index < rates.Count; index++)
            {
                seedValue -= rates[index];

                if (seedValue <= 0)
                {
                    break;
                }
            }

            return index;
        }

        #endregion Method
    }
}