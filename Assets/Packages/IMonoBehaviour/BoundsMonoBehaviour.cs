using UnityEngine;

[ExecuteInEditMode]
public abstract class BoundsMonoBehaviour : MonoBehaviour, IBounds
{
    #region Field

    public bool gizmo = true;

    public Color gizmoColor = Color.white;

    #endregion Field

    #region Property

    public abstract Bounds Bounds { get; protected set; }

    #endregion Property

    #region Method

    protected virtual void OnDrawGizmos()
    {
        if (!this.gizmo)
        {
            return;
        }

        Color prevColor = Gizmos.color;

        Gizmos.color = this.gizmoColor;
        Gizmos.DrawWireCube(this.Bounds.center, this.Bounds.size);
        Gizmos.color = prevColor;
    }

    #endregion Method
}