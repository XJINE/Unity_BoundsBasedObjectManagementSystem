using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class Sample : MonoBehaviour
    {
        #region Field

        public SampleBoundsBasedObjectGenerator generator;

        public SampleBounds paintBounds;
        public Color[]      paintColors = new Color[] { Color.red, Color.blue, Color.green };

        public KeyCode generateKey  = KeyCode.Return;
        public KeyCode removeAllKey = KeyCode.Delete;
        public KeyCode paintKey     = KeyCode.P;

        #endregion Field

        #region Method

        protected virtual void Update()
        {
            if (Input.GetKeyDown(this.generateKey))
            {
                var managedObject = this.generator.Generate<SampleBoundsBasedManagedObject>
                                    (Random.Range(0, this.generator.objects.Length));
            }

            if (Input.GetKeyDown(this.removeAllKey))
            {
                foreach (var managedObject in this.generator.Manager.ManagedObjects)
                {
                    GameObject.Destroy(managedObject.gameObject);
                }
            }

            if (Input.GetKeyDown(this.paintKey))
            {
                //int boundsIndex = Bounds
                foreach (var managedObject in this.generator.Manager.ManagedObjectsInBounds)
                {
                    managedObject.Data.SetColor(this.paintColors[Random.Range(0, this.paintColors.Length)]);
                }
            }
        }

        protected virtual void OnGUI()
        {
            GUILayout.Label("Press " + this.generateKey  + " to Add a New Object.");
            GUILayout.Label("Press " + this.removeAllKey + " to Remove All Objects.");
            GUILayout.Label("Press " + this.paintKey     + " to Paint Objects.");

            GUILayout.Label("Object Count : " + this.generator.Manager.ManagedObjects.Count
                                      + " / " + this.generator.Manager.maxCount);
        }

        #endregion Method
    }
}