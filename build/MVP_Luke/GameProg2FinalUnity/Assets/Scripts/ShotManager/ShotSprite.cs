using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public enum PlayerOrEnemyShot { Enemy, Player}
public abstract class ShotSprite : MonoBehaviour
{

    private Vector3 moveTranslation;
    public SpriteRenderer spriteRenderer;
    protected float damage { get; set; }
    protected Vector3 direction { get; set; }
    protected float speed { get; set; }


    public PlayerOrEnemyShot OwnerOfShot;
    public enum ShotState { Start, Shooting, Done};
    [SerializeField]
    public ShotState State = ShotState.Start;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetUpShot()
    {

    }

    public virtual void Shooting(PlayerOrEnemyShot owner)
    {
        this.State = ShotState.Shooting;
        OwnerOfShot = owner;
    }
    
    public virtual void SetPosition(Vector3 tran, Vector3 Direction)
    {
        this.transform.position = tran;
        this.direction = Direction;
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.State)
        {
            case ShotState.Start:
                break;
            case ShotState.Shooting:
                this.transform.position += direction * speed * Time.deltaTime;
                break;
            case ShotState.Done:
                break;

        }
    }

    public virtual void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player" && OwnerOfShot == PlayerOrEnemyShot.Enemy)
        {
            coll.GetComponent<Caster>().TakeDamage(this.damage);
            this.State = ShotState.Done;
        }
        if (coll.gameObject.tag == "Enemy" && OwnerOfShot == PlayerOrEnemyShot.Player)
        {
            coll.GetComponent<Caster>().TakeDamage(this.damage);
            this.State = ShotState.Done;
        }
        if(coll.gameObject.tag == "Obstacle")
        {
            this.State = ShotState.Done;
        }
    }
}
