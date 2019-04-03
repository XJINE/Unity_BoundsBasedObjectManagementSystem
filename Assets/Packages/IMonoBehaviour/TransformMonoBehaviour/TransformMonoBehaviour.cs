using UnityEngine;

public class TransformMonoBehaviour : MonoBehaviour, ITransformMonoBehaviour
{
    // CAUTION:
    // Initialize() is needed because of Awake() might be not called yet when this instance is referenced.
    // For example, when a scene starts.

    #region Property

    public new Transform transform
    {
        get;
        protected set;
    }

    public bool IsInitialized 
    {
        get;
        protected set;
    }

    #endregion Property

    #region Method

    protected virtual void Awake()
    {
        Initialize();
    }

    public virtual bool Initialize() 
    {
        if (this.IsInitialized)
        {
            return false;
        }

        this.transform = base.transform;
        this.IsInitialized = true;

        return true;
    }

    #endregion Method
}