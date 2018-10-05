using UnityEngine;

public interface ITransformMonoBehaviour
{
    #region Property

    // NOTE:
    // This property named in lowercase, because default MonoBehaviour's field name is 'transform'.

    Transform transform { get; }

    #endregion Property
}