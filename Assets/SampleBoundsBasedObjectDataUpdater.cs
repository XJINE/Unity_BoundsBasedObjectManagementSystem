using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class SampleBoundsBasedObjectDataUpdater : MonoBehaviour
    {
        #region Field

        public SampleBoundsBasedObjectManager manager;

        public SampleBounds bounds;

        #endregion Field

        #region Method

        protected void Update()
        {
            // NOTE:
            // You can reference in MangedObject & the Data in each Bounds.

            // Ex.
            // And following code is more fast.
            // protected int boundsIndex this.manager.GetBoundsIndex(this.bounds);
            // this.manager.GetObjectsInBounds(boundsIndex);

            var objectsInBounds = this.manager.GetObjectsInBounds(this.bounds);

            foreach (var objectInBounds in objectsInBounds)
            {
                objectInBounds.Data.lifeCount -= 1f;
            }
        }

        #endregion Method
    }
}