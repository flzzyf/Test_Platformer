﻿using System.Collections;
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
    //最大可攀爬角度
    public float maxClambAngle = 60;

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

        //print(collisions.below);
    }
    //水平碰撞判定
    void HorizontalCollisions(ref Vector3 _velocity)
    {
        //速度正负方向
        float directionX = Mathf.Sign(_velocity.x);
        //光束长度
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
                //坡角度
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxClambAngle)
                {
                    float distanceToSlopeStart = 0;
                    //进入新的坡
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        _velocity.x -= distanceToSlopeStart * directionX;
                    }
                    //爬坡
                    ClampSlope(ref _velocity, slopeAngle);
                    _velocity.x += distanceToSlopeStart * directionX;
                }
                //爬不动坡
                if(!collisions.clambingSlope || slopeAngle > maxClambAngle)
                {
                    //根据距离确定下一步移动距离
                    _velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;
                    //在爬坡
                    if (collisions.clambingSlope)
                    {
                        _velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_velocity.x);
                    }

                    //若方向向左则为真
                    collisions.left = directionX == -1;
                    //向右
                    collisions.right = directionX == 1;
                }

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
            //有障碍物
            if (hit)
            {
                //当距离为0时速度也为0
                _velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                //在爬墙
                if (collisions.clambingSlope)
                {
                    //上方有障碍限制x方向移动
                    _velocity.x = _velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(_velocity.x);
                }

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

    void ClampSlope(ref Vector3 velocity, float slopeAngle)
    {
        //在平地上应该移动的水平距离，若有障碍物则为0
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(velocity.y > climbVelocityY)
        {
            print("Jumping");
        }
        else
        {
            //计算在斜坡上的xy轴移动距离
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            //设置为接触地面
            collisions.below = true;
            collisions.clambingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    struct RaycastOrigin
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;
        public bool clambingSlope;

        public float slopeAngle, slopeAngleOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            clambingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}