using UnityEngine;

public interface ITransform
{
    #region Property

    // NOTE:
    // This property named in lowercase,
    // because MonoBehaviour's field name is 'transform'.

    Transform transform { get; }

    #endregion Property
}