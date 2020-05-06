using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Attacker
{
    [SerializeField]
    protected EnemyStates enemyState;
    public float DistanceForTouch;

    private Vector3 tarPos;

    [SerializeField]
    private float rotationSpeed = 2.0f;
    private float minX, maxX, minZ, maxZ;

    public Player PlayerReference;
    public Sight ReferenceToSight;


    // Start is called before the first frame update
    void Start()
    {
        this.CurrentHP = this.MaxHP = 45f;

        this.enemyState = EnemyStates.Attack;
        minX = -15.0f;
        maxX = 15.0f;

        minZ = -15.0f;
        maxZ = 15.0f;

        GetNextPosition();
    }

    void GetNextPosition()
    {
        tarPos = new Vector3(this.transform.position.x + Random.Range(minX, maxX), 0.5f, this.transform.position.z + Random.Range(minZ, maxZ));
    }

    void Update()
    {
        if (this.ReferenceToSight.DetectAspect())
            this.enemyState = EnemyStates.Chase;
        if ((this.ReferenceToSight.DetectAspect() == false) && Vector3.Distance(this.transform.position, PlayerReference.transform.position) <= DistanceForTouch)
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
                AttackingMelee();
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

    public void AttackingMelee()
    {
        Quaternion tarRotation = Quaternion.LookRotation(PlayerReference.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);
        
        if ((lastSpawnTime > SpawnTime))
        {
            this.GetComponentInChildren<ClawWeapon>().enabled = true;
            lastSpawnTime = 0f;
        }
    }

    public override void TakeDamage(float DamageAmount)
    {
        this.CurrentHP -= DamageAmount;
    }
}
