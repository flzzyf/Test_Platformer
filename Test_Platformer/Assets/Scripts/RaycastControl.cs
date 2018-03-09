using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastControl : MonoBehaviour {

    //边缘厚度，一个很小的值
    public const float skinWidth = .015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    new BoxCollider2D collider;
    public LayerMask collisionMask;

    protected RaycastOrigin raycastOrigin;

    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();

        CalculateRaySpacing();

    }

    //更新四个方向的光束源点
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        //边缘缩进一定的厚度
        bounds.Expand(skinWidth * -2);

        raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }
    //计算光束间距
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        //边缘缩进一定的厚度
        bounds.Expand(skinWidth * -2);
        //至少两道光
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        //计算水平和垂直方向上光束的间距
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    protected struct RaycastOrigin
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
