using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : RaycastControl {

    public Vector3 move;

    public LayerMask layer_passenger;

    public override void Start()
    {
        base.Start();

    }

    private void FixedUpdate()
    {
        UpdateRaycastOrigins();

        Vector3 velocity = move * Time.deltaTime;

        MovePassengers(velocity);
        transform.Translate(velocity);

    }

    void MovePassengers(Vector3 _velocity)
    {
        HashSet<Transform> passengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(_velocity.x);
        float directionY = Mathf.Sign(_velocity.y);

        //垂直移动带动乘客
        if (_velocity.y != 0)
        {
            //光束长度
            float rayLength = Mathf.Abs(_velocity.y) + skinWidth;
            //遍历每道光束
            for (int i = 0; i < verticalRayCount; i++)
            {
                //根据速度方向确定光束方向
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
                //光束源点偏移
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                //发出光束
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, layer_passenger);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.blue);

                if (hit)
                {
                    if (!passengers.Contains(hit.transform))
                    {
                        passengers.Add(hit.transform);

                        //如果在上升
                        float pushX = (directionY == 1) ? _velocity.x : 0;
                        float pushY = _velocity.y - (hit.distance - skinWidth) * directionY;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }
        //水平移动碰到人，推动
        if (_velocity.x != 0)
        {
            float rayLength = Mathf.Abs(_velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;

                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, layer_passenger);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit)
                {
                    if (!passengers.Contains(hit.transform))
                    {
                        passengers.Add(hit.transform);

                        //如果在上升
                        float pushX = _velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = 0;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }

        if(directionY == -1 || (_velocity.y == 0 && _velocity.x != 0))
        {
            //光束长度
            float rayLength = skinWidth * 2;
            //遍历每道光束
            for (int i = 0; i < verticalRayCount; i++)
            {
                //根据速度方向确定光束方向
                Vector2 rayOrigin = raycastOrigin.topLeft + Vector2.right * (verticalRaySpacing * i);
                //发出光束
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, layer_passenger);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.blue);

                if (hit)
                {
                    if (!passengers.Contains(hit.transform))
                    {
                        passengers.Add(hit.transform);

                        //如果在上升
                        float pushX = _velocity.x;
                        float pushY = _velocity.y;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }
    }

}
