using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrip : MonoBehaviour
{
    [SerializeField] Transform rightHand;
    [SerializeField] float gripRange;

    Transform target;
    float minDistance;
    float distance;
    float originalY;
    Quaternion originalAngle;
    bool isOn;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            Collider[] colliders = Physics.OverlapSphere(rightHand.position, gripRange, 1 << LayerMask.NameToLayer("HandObject"));
            if (colliders.Length > 0)
            {
                target = null;
                minDistance = Mathf.Infinity;

                for (int i = 0; i < colliders.Length; i++)
                {
                    distance = (colliders[i].transform.position - colliders[i].transform.position).magnitude;
                    if (distance < minDistance)
                    {
                        target = colliders[i].transform;
                        minDistance = distance;
                    }
                }

                isOn = true;
            }

            if (isOn)
            {
                originalAngle = target.rotation;
                originalY = target.position.y;
                target.SetParent(rightHand);
                Transform locationInfo = target.Find("Location Info");
                target.localPosition = locationInfo.localPosition;
                target.localRotation = locationInfo.localRotation;
            }
        }

        if (isOn && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {
            target.SetParent(null);
            target.rotation = originalAngle;
            Vector3 position = target.position;
            position.y = originalY;
            target.position = position;

            isOn = false;
        }
    }
}
