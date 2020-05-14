﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMelee : Attacker
{
    private float elapsedTime;
    public float SetTimerForRecoveryAfterAttack = 3;


    public Path path;

    public float DistanceToDetectPlayer;


    private Vector3 rayDirection;
    public float Mass = 5.0f;
    private Vector3 velocity;
    public float Radius = 0.001f;
    private int curPathIndex;
    private int aStarPathIndex;
    private bool isEnd;
    public bool IsLooping = true;

    public int HitDistance = 15;
    private Transform startPos, endPos;
    public Node StartNode { get; set; }
    public Node GoalNode { get; set; }
    public float DistanceToStartShooting;

    public List<Node> PathArray;

    public UnityEnemySubject UnityEnemySub { get; private set; }

    protected EnemyOb enemyOb;

    [SerializeField]
    private float setMaxHP = 100;

    public float DistanceForTouch;

    [SerializeField]
    private float rotationSpeed = 2.0f;


    //public GameObject PacMan;
    public Player PlayerReference;

    public Sight ReferenceToSight;


    [SerializeField]
    protected EnemyStates enemyState;


    private void Awake()
    {
        this.enemyOb = new EnemyOb();
        UnityEnemySub = new UnityEnemySubject(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;

        aStarPathIndex = 0;
        velocity = transform.forward;

        isEnd = false;
        PathArray = new List<Node>();

        this.OwnerOfShot = PlayerOrEnemyShot.Enemy;
        lastSpawnTime = SpawnTime;

        enemyState = EnemyStates.Patrol;
        this.MaxHP = setMaxHP;
        this.CurrentHP = this.MaxHP;

    }


    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(this.transform.position, (transform.position + (transform.forward * HitDistance)), Color.red);

        //if (this.ReferenceToSight.DetectAspect())
        //    this.enemyState = EnemyStates.Attack;
        if ((this.ReferenceToSight.DetectAspect() == false) && Vector3.Distance(this.transform.position, PlayerReference.transform.position) >= DistanceForTouch)
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
            case EnemyStates.Recovery:
                elapsedTime += Time.deltaTime;
                Recover();
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
        this.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);

        if (this.CurrentHP <= 0)
        {
            if (this.gameObject.name == "BossEnemy")
            {
                this.UnityEnemySub.Notify("BossDead");
                Debug.Log("BossDed");
            }
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }

    public void Recover()
    {
        if(elapsedTime>= SetTimerForRecoveryAfterAttack)
        {
            this.enemyState = EnemyStates.Chase;
            elapsedTime = 0;
        }
    }

    public bool DetectObstacle(string whatAreYouLookingFor)
    {
        RaycastHit hit;
        rayDirection = this.transform.forward;
        if (Physics.Raycast(transform.position, rayDirection, out hit, HitDistance))
        {
            string tag = hit.collider.gameObject.tag;
            if (tag != null)
            {
                if (tag == whatAreYouLookingFor)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void FindPath()
    {
        startPos = this.transform;
        endPos = PlayerReference.transform;
        StartNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        GoalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));
        PathArray = AStar.FindPath(StartNode, GoalNode);
        aStarPathIndex = 0;
    }

    public void AStarPathing()
    {
        if (PathArray.Count > 0)
        {
            float curSpeed = movementSpeed * Time.deltaTime;
            Vector3 tarPos = PathArray[aStarPathIndex].Position;
            Debug.Log(aStarPathIndex);
            Debug.Log(PathArray.Count);
            if (Vector3.Distance(transform.position, PathArray[aStarPathIndex].Position) < Radius)
            {
                if (aStarPathIndex < PathArray.Count - 1)
                    aStarPathIndex++;
                if (isEnd)
                {
                    aStarPathIndex = 0;
                    isEnd = false;
                }
            }
            if (aStarPathIndex >= PathArray.Count - 1)
            {
                FindPath();
                isEnd = true;
                return;
            }

            if (aStarPathIndex >= PathArray.Count - 1 && !isEnd)
                velocity += Steer(PathArray[aStarPathIndex].Position, curSpeed, true);
            else
                velocity += Steer(PathArray[aStarPathIndex].Position, curSpeed);

            //transform.rotation = Quaternion.LookRotation(velocity);
            Vector3 dirRot = PathArray[aStarPathIndex].Position - transform.position;
            Quaternion tarRotation = Quaternion.LookRotation(dirRot);
            tarRotation.eulerAngles = new Vector3(0, tarRotation.eulerAngles.y, 0);

            transform.position += velocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);

        }
    }

    public Vector3 Steer(Vector3 target, float curSpeed, bool bFinalPoint = false)
    {
        Vector3 desiredVelocity = (target - transform.position);
        float dist = desiredVelocity.magnitude;

        desiredVelocity.Normalize();

        if (bFinalPoint && dist < 10.0f)
            desiredVelocity *= (curSpeed * (dist / 10.0f));
        else
            desiredVelocity *= curSpeed;

        Vector3 steeringForce = desiredVelocity - velocity;
        Vector3 acceleration = steeringForce / Mass;

        return acceleration;
    }

    public void Patrolling()
    {
        if (DetectObstacle("Player"))
        {
            FindPath();
            this.enemyState = EnemyStates.Chase;
            return;
        }
        float curSpeed = movementSpeed * Time.deltaTime;
        Vector3 targetPoint = path.GetPoint(curPathIndex);
        if (Vector3.Distance(transform.position, targetPoint) < path.Radius)
        {
            if (curPathIndex < path.Length - 1)
            {
                curPathIndex++;
            }
            else if (IsLooping)
                curPathIndex = 0;
            else
                return;
        }

        if (curPathIndex >= path.Length)
            return;
        if (curPathIndex >= path.Length - 1 && !IsLooping)
            velocity += Steer(targetPoint, curSpeed, true);
        else
            velocity += Steer(targetPoint, curSpeed);

        //transform.rotation = Quaternion.LookRotation(velocity);
        Vector3 dirRot = path.GetPoint(curPathIndex) - transform.position;
        Quaternion tarRotation = Quaternion.LookRotation(dirRot);
        tarRotation.eulerAngles = new Vector3(0, tarRotation.eulerAngles.y, 0);

        transform.position += velocity;
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);

    }

    public void CheckDistanceFromPlayer()
    {
        if (Vector3.Distance(PlayerReference.transform.position, this.transform.position) <= DistanceToDetectPlayer)
        {
            this.enemyState = EnemyStates.Chase;
        }
    }

    public void Chasing()
    {
        Debug.Log(Vector3.Distance(PlayerReference.transform.position, transform.position));
        if (Vector3.Distance(PlayerReference.transform.position, transform.position) <= this.GetComponentInChildren<MeleeWeapon>().RangeOfAttack)
        {
            Attacking();
        }
        Quaternion tarRot = Quaternion.LookRotation(PlayerReference.transform.position - this.transform.position);
        tarRot.eulerAngles = new Vector3(0, tarRot.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, 10.0f * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }

    public void Attacking()
    {
        
        //if ((lastSpawnTime > SpawnTime))
        //{
            this.PlayerReference.GetComponent<Attacker>().TakeDamage(this.GetComponentInChildren<MeleeWeapon>().DealDamage());
            lastSpawnTime = 0f;
            this.enemyState = EnemyStates.Recovery;
        //}
    }

    public override void TakeDamage(float DamageAmount)
    {
        this.CurrentHP -= DamageAmount;
    }
}
