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
            if (Time.frameCount % 180 == 0)
            {
                var objectsInBounds = this.manager.GetObjectsInBounds(this.bounds);

                float scale = Random.Range(0.5f, 3f);

                foreach (var objectInBounds in objectsInBounds)
                {
                    objectInBounds.Data.scale = scale;
                }
            }
        }

        #endregion Method
    }
}