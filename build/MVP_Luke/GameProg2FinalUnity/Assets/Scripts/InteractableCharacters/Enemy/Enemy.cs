using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Caster
{
    public float DistanceForTouch;

    private Vector3 tarPos;

    [SerializeField]
    private float rotationSpeed = 2.0f;
    private float minX, maxX, minZ, maxZ;

    public Player PlayerReference;
    public Sight ReferenceToSight;

    public float SpawnTime;
    protected float lastSpawnTime;

    public enum EnemyStates { Patrol, SeenPlayer, Chase, Attack, NoAction}

    EnemyStates enemyState;

    // Start is called before the first frame update
    void Start()
    {
        this.movementSpeed = this.MaxMovementSpeed = 3.0f;
        this.OwnerOfShot = PlayerOrEnemyShot.Enemy;
        lastSpawnTime = SpawnTime;

        enemyState = EnemyStates.Patrol;
        this.MaxHP = 30f;
        this.CurrentHP = this.MaxHP;

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
        if (this.ReferenceToSight.DetectAspect())
            this.enemyState = EnemyStates.Attack;
        if((this.ReferenceToSight.DetectAspect() == false) && Vector3.Distance(this.transform.position, PlayerReference.transform.position) <= DistanceForTouch)
        {
            this.enemyState = EnemyStates.Patrol;
        }
       
        switch (enemyState)
        {
            case EnemyStates.Patrol:
                Patrolling();
                break;
            case EnemyStates.Chase:
                Chasing();
                break;
            case EnemyStates.Attack:
                lastSpawnTime += Time.deltaTime;
                Attacking();
                break;
            case EnemyStates.NoAction:
                break;
        }
        switch (UserCondition)
        {
            case Conditions.NoCondition:
                break;
            case Conditions.Burning:
                this.tickForDamageOverTime += Time.deltaTime;
                timerToMeetConditionLength += Time.deltaTime;
                Burning();
                break;
            case Conditions.Chilled:
                this.tickForDamageOverTime += Time.deltaTime;
                timerToMeetConditionLength += Time.deltaTime;
                Chilled();
                break;
        }


        if (this.CurrentHP <= 0)
        {
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }

    public void Patrolling()
    {
        if (Vector3.Distance(tarPos, transform.position) <= 5.0f)
        {
            GetNextPosition();
        }

        Quaternion tarRotation = Quaternion.LookRotation(tarPos - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }

    public void Chasing()
    {
       
            this.enemyState = EnemyStates.Attack;
        
    }

    public void Attacking()
    {
        Quaternion tarRotation = Quaternion.LookRotation(PlayerReference.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);
        if((lastSpawnTime > SpawnTime))
        {
            this.GetComponent<FireBoltSpawner>().SpawnTheObject();
            lastSpawnTime = 0f;
        }
    }

    public override void TakeDamage(float DamageAmount)
    {
        this.CurrentHP -= DamageAmount;
    }

    public override void SetUpCharacterStats()
    {
        base.SetUpCharacterStats();
        
    }
}
