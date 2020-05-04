using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    private Vector3 tarPos;
    private float movementSpeed = 10.0f;
    private float rotationSpeed = 2.0f;
    private float minX, maxX, minZ, maxZ;
    // Start is called before the first frame update
    void Start()
    {
        minX = -25.0f;
        maxX = 25.0f;

        minZ = -25.0f;
        maxZ = 25.0f;

        GetNextPosition();
    }

    void GetNextPosition()
    {
        tarPos = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(tarPos, transform.position) <= 5.0f)
        {
            GetNextPosition();
        }

        Quaternion tarRotation = Quaternion.LookRotation(tarPos - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }
}
