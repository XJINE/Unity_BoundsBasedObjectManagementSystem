using UnityEngine;

namespace ObjectManagementSystem
{
    // NOTE:
    // This is one of a sample.
    // ObjectManager<> is able to use without ObjectGenerator<>.

    [RequireComponent(typeof(ObjectManager<>))]
    public class ObjectGenerator<T> : MonoBehaviour
    {
        #region Field

        public GameObject[] objects;

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

        public virtual U Generate<U>(int index) where U : ManagedObject<T>
        {
            if (this.ObjectManager.IsFilled)
            {
                return null;
            }

            GameObject generatedObject = GameObject.Instantiate(this.objects[index]);

            return this.ObjectManager.AddManagedObject<U>(generatedObject);
        }

        #endregion Method
    }
}