using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {
    //边缘厚度，一个很小的值
    const float skinWidth = .015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;

    RaycastOrigin raycastOrigin;
    public CollisionInfo collisions;

    public LayerMask collisionMask;

    void Start () {
        collider = GetComponent<BoxCollider2D>();

        CalculateRaySpacing();

    }
    //移动
    public void Move(Vector3 _velocity)
    {
        //更新四个方向的光束源点
        UpdateRaycastOrigins();
        //重置碰撞
        collisions.Reset();
        //水平碰撞判定
        if (_velocity.x != 0)
            HorizontalCollisions(ref _velocity);
        //垂直碰撞判定
        if (_velocity.y != 0)
            VerticalCollisions(ref _velocity);
        //移动
        transform.Translate(_velocity);
    }
    //水平碰撞判定
    void HorizontalCollisions(ref Vector3 _velocity)
    {
        //速度正负方向
        float directionX = Mathf.Sign(_velocity.x);

        float rayLength = Mathf.Abs(_velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;

            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            //碰到物体
            if (hit)
            {
                _velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                //若方向向左则为真
                collisions.left = directionX == -1;
                //向右
                collisions.right = directionX == 1;

            }
        }
    }
    //垂直碰撞判定
    void VerticalCollisions(ref Vector3 _velocity)
    {
        //速度正负方向
        float directionY = Mathf.Sign(_velocity.y);
        //光束长度
        float rayLength = Mathf.Abs(_velocity.y) + skinWidth;
        //遍历每道光束
        for (int i = 0; i < verticalRayCount; i++)
        {
            //根据速度方向确定光束方向
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
            //光束源点偏移
            rayOrigin += Vector2.right * (verticalRaySpacing * i + _velocity.x);
            //发出光束
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.blue);

            if (hit)
            {
                //当距离为0时速度也为0
                _velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                //若方向向下则为真
                collisions.below = directionY == -1;
                //向右
                collisions.above = directionY == 1;
            }
        }
    }

    //更新四个方向的光束源点
    void UpdateRaycastOrigins()
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
    void CalculateRaySpacing()
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

    struct RaycastOrigin
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

}
