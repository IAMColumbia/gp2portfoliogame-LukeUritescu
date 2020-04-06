﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Sense
{
    public int FieldOfView = 45;
    public int ViewDistance = 100;

    private Transform playerTransform;
    private Vector3 rayDirection;

    protected override void Initialize()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= detectionRate) DetectAspect();
    }

    void DetectAspect()
    {
        RaycastHit hit;

        rayDirection = playerTransform.position - transform.position;

        if((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            if(Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                if(aspect != null)
                {
                    if(aspect.aspectName == aspectName)
                    {
                        print("Enemy Detected");
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(!Application.isEditor || playerTransform == null)
        {
            return;
        }

        Debug.DrawLine(transform.position, playerTransform.position, Color.red);
        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        Vector3 leftRayPoint = Quaternion.Euler(0, FieldOfView * 0.5f, 0) * frontRayPoint;

        Vector3 rightRayPoint = Quaternion.Euler(0, -FieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
        Debug.DrawLine(transform.position, leftRayPoint, Color.green);
        Debug.DrawLine(transform.position, rightRayPoint, Color.green);
    }
}
