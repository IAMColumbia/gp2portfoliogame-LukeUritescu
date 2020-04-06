using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Projectile;

    private Transform projectileSpawnPoint;
    private float shootRate = 0.5f;
    private float elapsedTime;

    public Transform targetTransform;
    private float movementSpeed, rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {

        movementSpeed = 10.0f;
        rotationSpeed = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetTransform.position) < 2.5f)
            return;

        Vector3 tarPos = targetTransform.position;
        tarPos.y = 0.5f;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRotation = Quaternion.LookRotation(dirRot);

        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }
}
