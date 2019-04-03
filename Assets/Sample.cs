using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class Sample : MonoBehaviour
    {
        #region Field

        public SampleBoundsBasedObjectGenerator generator;
        public SampleBounds resizeBounds;

        public KeyCode generateKey  = KeyCode.Return;
        public KeyCode removeAllKey = KeyCode.Delete;

        #endregion Field

        #region Method

        protected virtual void Update()
        {
            if (Input.GetKeyDown(this.generateKey))
            {
                if (this.generator.Manager.IsFilled)
                {
                    return;
                }

                var managedObject = this.generator.Generate<SampleBoundsBasedManagedObject>
                                    (Random.Range(0, this.generator.objects.Length));

                GameObject.Destroy(managedObject.gameObject, 30);
            }

            if (Input.GetKeyDown(this.removeAllKey))
            {
                foreach (var managedObject in this.generator.Manager.ManagedObjects)
                {
                    GameObject.Destroy(managedObject.gameObject);
                }
            }

            foreach (var managedObject in this.generator.Manager.ManagedObjects)
            {
                managedObject.transform.localScale = Vector3.one;
            }

            foreach (var managedObject in this.generator.Manager.ManagedObjectsInBounds[this.resizeBounds])
            {
                managedObject.transform.localScale = Vector3.one * 0.5f;
            }
        }

        protected virtual void OnGUI()
        {
            GUILayout.Label("Press " + this.generateKey  + " to Add a New Object.");
            GUILayout.Label("Press " + this.removeAllKey + " to Remove All Objects.");

            GUILayout.Label("Object Count : " + this.generator.Manager.ManagedObjects.Count
                                      + " / " + this.generator.Manager.maxCount);
        }

        #endregion Method
    }
}