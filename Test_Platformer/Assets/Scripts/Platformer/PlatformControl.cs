using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : RaycastControl {

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public float speed = 1;
    int fromWaypointIndex;
    float percentBetweenWaypoints;

    public bool cyclic;

    public LayerMask layer_passenger;

    List<PassengerMovement> passengerMovements;

    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    public float waitTime = 0.5f;
    float nextMoveTime;
    [Range(0, 2)]
    public float easeAmount = .2f;

    public override void Start()
    {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];

        for (int i = 0; i < globalWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;

        }
    }

    private void FixedUpdate()
    {
        UpdateRaycastOrigins();

        Vector3 velocity = CalculatePlatformMovement();

        CalculatePassengerMovement(velocity);

        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    float Ease(float _x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(_x, a) / (Mathf.Pow(_x, a) + Mathf.Pow(1 - _x, a));
    }

    Vector3 CalculatePlatformMovement()
    {
        if(Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;

        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if(percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            //不循环
            if (!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }

            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    void MovePassengers(bool _beforeMovePlatform)
    {
        foreach (PassengerMovement pm in passengerMovements)
        {
            if (!passengerDictionary.ContainsKey(pm.transform))
            {
                passengerDictionary.Add(pm.transform, pm.transform.GetComponent<Controller2D>());
            }

            if(pm.moveBeforePlatform == _beforeMovePlatform)
            {
                passengerDictionary[pm.transform].Move(pm.velocity, pm.standingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement(Vector3 _velocity)
    {
        passengerMovements = new List<PassengerMovement>();

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

                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
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
                    //Debug.Log("hOR");

                    if (!passengers.Contains(hit.transform))
                    {
                        passengers.Add(hit.transform);

                        float pushX = _velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));

                    }
                }
            }
        }
        //平台在向下或水平移动
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

                        passengerMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));

                    }
                }
            }
        }
    }

    struct PassengerMovement {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    private void OnDrawGizmos()
    {
        if(localWaypoints != null)
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawWireCube(globalWaypointPos, transform.localScale);
            }
        }
    }

}
