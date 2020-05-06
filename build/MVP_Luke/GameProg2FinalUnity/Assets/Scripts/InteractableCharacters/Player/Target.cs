using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private bool tagIfObstacle;
    public bool TagIfObstacle
    {
        get { return this.tagIfObstacle; }
        set
        {
            if (this.tagIfObstacle != value)
            {
                this.tagIfObstacle = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TagIfObstacle = false;
    }

    // Update is called once per frame
    void Update()
    {
        int button = 0;

        if (Input.GetMouseButtonDown(button))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Vector3 targetPosition = hitInfo.point;
                transform.position = targetPosition;
                if (tag != null)
                {
                    if (hitInfo.collider.gameObject.tag == "Obstacle")
                    {
                        TagIfObstacle = true;
                    }
                    else
                    {
                        TagIfObstacle = false;
                    }
                }
            }
            
        }
    }
}
