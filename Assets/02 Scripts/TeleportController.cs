using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform rightHand;
    [SerializeField] float power;
    [SerializeField] float maxDistance;
    [SerializeField] float interval;
    [SerializeField] float gravity;
    [SerializeField] LineRenderer laser;
    [SerializeField] GameObject destination;

    LineRenderer laserClone;
    GameObject destinationClone;
    Vector3 direction;
    Vector3 position;
    Vector3 intervalVector;
    List<Vector3> positionList;
    RaycastHit hit;
    float distance;
    bool isOn;
    bool isCollision;

    void Awake()
    {
        positionList = new List<Vector3>();
    }

    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            laserClone = Instantiate(laser);
            laserClone.gameObject.SetActive(true);

            isOn = true;
        }

        if (isOn)
        {
            if (OVRInput.Get(OVRInput.Button.One))
            {
                direction = rightHand.forward;
                position = rightHand.position;
                distance = 0f;
                isCollision = false;

                positionList.Clear();
                positionList.Add(position);

                while (distance < maxDistance)
                {
                    intervalVector = direction * power * Time.fixedDeltaTime;
                    position += intervalVector;
                    positionList.Add(position);
                    direction.y -= gravity * interval * Time.fixedDeltaTime;
                    direction = direction.normalized;

                    distance += intervalVector.magnitude;

                    Collider[] colliders = Physics.OverlapSphere(position, 0.1f, 1 << LayerMask.NameToLayer("Plane"));
                    if (colliders.Length > 0)
                    {
                        isCollision = true;
                        break;
                    }
                }

                laserClone.positionCount = positionList.Count;
                laserClone.SetPositions(positionList.ToArray());

                if (isCollision)
                {
                    if (!destinationClone)
                        destinationClone = Instantiate(destination);
                    if (!destinationClone.activeSelf)
                        destinationClone.SetActive(true);

                    if (Physics.Raycast(positionList[positionList.Count - 1], Vector3.down, out hit))
                        destinationClone.transform.position = hit.point;
                }
            }

            if(OVRInput.GetUp(OVRInput.Button.One))
            {
                Destroy(laserClone.gameObject);

                isOn = false;

                if (isCollision)
                {
                    position = hit.point;
                    position.y = player.position.y;
                    player.position = position;

                    Destroy(destinationClone);
                    destinationClone = null;
                }
            }
        }
    }
}
