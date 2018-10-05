using UnityEngine;

public class TransformMonoBehaviour : MonoBehaviour, ITransformMonoBehaviour
{
    // CAUTION:
    // Initialize() is needed because of Awake() might be not called yet when this instance is referenced.

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

    public virtual void Initialize() 
    {
        if (this.IsInitialized)
        {
            return;
        }

        this.transform = base.transform;
        this.IsInitialized = true;
    }

    #endregion Method
}