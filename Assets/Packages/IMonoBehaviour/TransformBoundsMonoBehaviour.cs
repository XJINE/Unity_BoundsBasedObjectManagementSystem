using UnityEngine;

public class TransformBoundsMonoBehaviour : BoundsMonoBehaviour, ITransform
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

    #region Field

    public bool isStatic;

    #endregion Field

    #region Property

    public new Transform transform { get; protected set; }

    public override Bounds Bounds { get; protected set; }

    public virtual Vector3 Min { get; protected set; }

    public virtual Vector3 Max { get; protected set; }

    public bool IsInitialized { get; protected set; }

    #endregion Property

    #region Method

    protected virtual void Awake()
    {
        this.transform = base.transform;

        UpdateBounds();
    }

    protected virtual void Update()
    {
        if (this.isStatic)
        {
            return;
        }

        UpdateBounds();
    }

    public virtual void UpdateBounds()
    {
        this.Bounds = new Bounds(this.transform.position, this.transform.localScale);
        this.Min = this.Bounds.min;
        this.Max = this.Bounds.max;
    }

    public bool Contains(Vector3 point)
    {
        return !(point.x < this.Min.x || point.y < this.Min.y || point.z < this.Min.z
              || this.Max.x < point.x || this.Max.y < point.y || this.Max.z < point.z);
    }

    public bool Intersects(TransformBoundsMonoBehaviour bounds)
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