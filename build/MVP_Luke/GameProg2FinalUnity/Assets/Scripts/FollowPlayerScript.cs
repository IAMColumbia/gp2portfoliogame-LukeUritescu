using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerScript : MonoBehaviour
{
    public Transform target;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            var go = GameObject.FindWithTag("Player");
            if (go)
            {
                target = go.transform;
            }
        }

        if (target)
        {
            transform.position = new Vector3(target.position.x, target.position.y + 30, target.position.z - distance);
        }
    }
}
