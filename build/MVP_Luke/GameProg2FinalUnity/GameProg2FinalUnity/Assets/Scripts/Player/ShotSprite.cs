using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSprite : MonoBehaviour
{

    private Vector3 moveTranslation;
    SpriteRenderer spriteRenderer;

    public Vector3 Direction;
    public float Speed;

    public enum ShotState { Start, Shooting, Collided, Done};
    public ShotState State;

    // Start is called before the first frame update
    void Start()
    {
        SetupShot();
    }

    private void SetupShot()
    {
        this.State = ShotState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.State == ShotState.Shooting)
        {
            this.moveTranslation = new Vector3(this.Direction.x, this.Direction.z) * Time.deltaTime * this.Speed;
            this.transform.position = new Vector3(this.transform.position.x + this.moveTranslation.x, this.transform.position.z + this.moveTranslation.z);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {

        }
        if(coll.gameObject.tag == "Enemy")
        {

        }
    }
}
