using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastControl {

    public CollisionInfo collisions;

    //最大可攀爬角度
    public float maxClambAngle = 60;
    //最大下坡角度（超过会直接掉下来
    public float maxDescendAngle = 55;

    public override void Start()
    {
        base.Start();

    }

    //移动
    public void Move(Vector3 _velocity)
    {
        //更新四个方向的光束源点
        UpdateRaycastOrigins();
        //重置碰撞
        collisions.Reset();
        //之前速度
        collisions.velocityOld = _velocity;
        //下坡
        if (_velocity.y < 0)
            DescendSlope(ref _velocity);
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
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        _velocity = collisions.velocityOld;
                    }
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

    void ClampSlope(ref Vector3 velocity, float slopeAngle)
    {
        //在平地上应该移动的水平距离，若有障碍物则为0
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(velocity.y > climbVelocityY)
        {
            //print("Jumping");
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

    void DescendSlope(ref Vector3 _velocity)
    {
        //速度正负方向
        float directionX = Mathf.Sign(_velocity.x);

        Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomRight : raycastOrigin.bottomLeft;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

        if (hit)
        {
            //坡角度
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            //坡角在合适范围内
            if (slopeAngle != 0 && slopeAngle <= maxClambAngle)
            {
                //在下坡
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    //
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_velocity.x))
                    {
                        float moveDistance = Mathf.Abs(_velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        _velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(_velocity.x);
                        _velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }

            }
        }
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;
        public bool clambingSlope;
        public bool descendingSlope;

        public float slopeAngle, slopeAngleOld;

        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            clambingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}
