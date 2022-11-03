using Oculus.Platform.Samples.VrHoops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMove : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform rightHand;
    [SerializeField] float speed;
    Vector3 moveDirection;

    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero)
        {
            Vector2 direction = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            moveDirection = GetMoveForward(rightHand.forward, rightHand.right, direction);
            Vector3 vector3 = player.position + moveDirection * speed * Time.deltaTime;
            vector3.y = player.position.y;
            player.position = vector3;
        }
    }

    Vector3 GetMoveForward(Vector3 forward, Vector3 right, Vector2 direction)
    {
        Vector3 moveForward = Vector3.zero;

        moveForward = (forward * direction.y) + (right * direction.x);
        moveForward.Normalize();

        return moveForward;
    }
}
