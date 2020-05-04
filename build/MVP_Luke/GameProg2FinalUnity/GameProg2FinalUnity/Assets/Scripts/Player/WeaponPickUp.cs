using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Pickup Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnCollisionEnter3D(Collision coll)
    {
        Debug.Log("Food OnCollisionEnter3D");
        if(coll.gameObject.tag == "Player")
        {
            this.Hit(coll.gameObject);
        }
    }

    public virtual void OnTriggerEnter3D(Collider coll)
    {
        Debug.Log("Food OnTriggerEnter3D");
        if(coll.gameObject.tag == "Player")
        {
            this.Hit(coll.gameObject);
        }
    }

    public virtual void Hit(GameObject p)
    {
        Destroy(this.gameObject);
    }
}
