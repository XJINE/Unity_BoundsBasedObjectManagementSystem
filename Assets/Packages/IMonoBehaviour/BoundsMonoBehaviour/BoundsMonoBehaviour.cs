using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class BoundsMonoBehaviour : TransformMonoBehaviour, IBoundsMonoBehaviour
{
    // NOTE:
    // Some of these are implemented to fast calculation.
    // For example, default Bounds.min(max) is implemented like this.
    // 
    // public Vector3 min
    // {
    //     get { return this.center - this.extents; }
    //     set { this.SetMinMax(value, this.max); }
    // }
    // public void SetMinMax(Vector3 min, Vector3 max)
    // {
    //     this.extents = (max - min) * 0.5f;
    //     this.center = min + this.extents;
    // }

    #region Class

    [System.Serializable]
    public class BoundsUpdateEvent : UnityEvent<BoundsMonoBehaviour> { }

    #endregion Class

    #region Field

    public bool gizmo = true;

    public Color gizmoColor = Color.white;

    public bool isStatic;

    public BoundsUpdateEvent boundsUpdateEvent;

    #endregion Field

    #region Property

    public virtual Bounds  Bounds { get; protected set; }
    public virtual Vector3 Min    { get; protected set; }
    public virtual Vector3 Max    { get; protected set; }

    #endregion Property

    #region Method

    protected virtual void Update()
    {
        if (this.isStatic)
        {
            return;
        }

        UpdateBounds();
    }

    protected virtual void OnDrawGizmos()
    {
        if (!this.gizmo)
        {
            return;
        }

        Color previousColor = Gizmos.color;
        Gizmos.color = this.gizmoColor;
        Gizmos.DrawWireCube(this.Bounds.center, this.Bounds.size);
        Gizmos.color = previousColor;
    }

    public override bool Initialize()
    {
        if (!base.Initialize())
        {
            return false;
        }

        if (this.boundsUpdateEvent == null)
        {
            this.boundsUpdateEvent = new BoundsUpdateEvent();
        }

        UpdateBounds();

        return true;
    }

    public virtual void UpdateBounds()
    {
        if (!base.IsInitialized)
        {
            Initialize();
            return;
        }

        Bounds previousBounds = this.Bounds;

        this.Bounds = new Bounds(this.transform.position, this.transform.localScale);
        this.Min    = this.Bounds.min;
        this.Max    = this.Bounds.max;

        if (previousBounds != this.Bounds)
        {
            this.boundsUpdateEvent.Invoke(this);
        }
    }

    public bool Contains(Vector3 point)
    {
        return !(point.x < this.Min.x || point.y < this.Min.y || point.z < this.Min.z
              || this.Max.x < point.x || this.Max.y < point.y || this.Max.z < point.z);
    }

    public bool Intersects(BoundsMonoBehaviour bounds)
    {
        return Intersects(bounds.Min, bounds.Max);
    }

    public bool Intersects(Bounds bounds)
    {
        return Intersects(bounds.min, bounds.max);
    }

    public bool Intersects(Vector3 boundsMin, Vector3 boundsMax)
    {
        return this.Min.x <= boundsMax.x
            && this.Max.x >= boundsMin.x
            && this.Min.y <= boundsMax.y
            && this.Max.y >= boundsMin.y
            && this.Min.z <= boundsMax.z
            && this.Max.z >= boundsMin.z;
    }

    #endregion Method
}