using UnityEngine;

public class TransformMonoBehaviour : MonoBehaviour, ITransform
{
    #region Property

    public new Transform transform { get; protected set; }

    #endregion Property

    #region Method

    protected virtual void Awake()
    {
        this.transform = base.transform;
    }

    #endregion Method
}