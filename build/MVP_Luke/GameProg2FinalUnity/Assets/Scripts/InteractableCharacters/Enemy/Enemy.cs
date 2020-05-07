using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

    public enum EnemyStates { Patrol, SeenPlayer, Chase, Attack, NoAction, Dead}
public class Enemy : Caster
{
    public Path path;

    private Vector3 rayDirection;
    public float Mass = 5.0f;
    private Vector3 velocity;
    public float Radius = 0.001f;
    private int curPathIndex;
    private bool isEnd;
    public bool isLooping = true;

    public int HitDistance = 15;
    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }
    public float DistanceToStartShooting;

    public List<Node> pathArray;

    public UnityEnemySubject UnityEnemySub { get; private set; }

    protected EnemyOb enemyOb;

    [SerializeField]
    private float setMaxHP= 100;

    public float DistanceForTouch;

    [SerializeField]
    private float rotationSpeed = 2.0f;


    //public GameObject PacMan;
    public Player PlayerReference;

    public Sight ReferenceToSight;



    EnemyStates enemyState;


    private void Awake()
    {
        this.enemyOb = new EnemyOb();
        UnityEnemySub = new UnityEnemySubject(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = transform.forward;

        isEnd = false;
        pathArray = new List<Node>();

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
        this.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);

        if (this.CurrentHP <= 0)
        {
            if(this.gameObject.name == "BossEnemy")
            {
                this.UnityEnemySub.Notify("BossDead");
                Debug.Log("BossDed");
            }
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
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
        startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));
        pathArray = AStar.FindPath(startNode, goalNode);
        curPathIndex = 0;
    }

    public void AStarPathing()
    {
        float curSpeed = movementSpeed * Time.deltaTime;
        Vector3 tarPos = pathArray[curPathIndex].position;
        if(pathArray.Count > 0)
        {
            if(Vector3.Distance(transform.position, pathArray[curPathIndex].position) < Radius)
            {
                if (curPathIndex < pathArray.Count)
                    curPathIndex++;
                if (isEnd)
                    curPathIndex = 0;
                else
                    return;
            }
            if(curPathIndex >= pathArray.Count - 1)
            {
                isEnd = true;
                return;
            }

            if (curPathIndex >= pathArray.Count - 1 && !isEnd)
                velocity += Steer(pathArray[curPathIndex].position, curSpeed, true);
            else
                velocity += Steer(pathArray[curPathIndex].position, curSpeed);

            //transform.rotation = Quaternion.LookRotation(velocity);
            Vector3 dirRot = pathArray[curPathIndex].position - transform.position;
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
            this.enemyState = EnemyStates.Chase;
            return;
        }
        float curSpeed = movementSpeed * Time.deltaTime;
        Vector3 targetPoint = path.GetPoint(curPathIndex);
        if(Vector3.Distance(transform.position, targetPoint) < path.Radius)
        {
            if (curPathIndex < path.Length - 1)
            {
                curPathIndex++;
            }
            else if (isLooping)
                curPathIndex = 0;
            else
                return;
        }

        if (curPathIndex >= path.Length)
            return;
        if (curPathIndex >= path.Length - 1 && !isLooping)
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

    public void Chasing()
    {
            if(Vector3.Distance(PlayerReference.transform.position, this.transform.position) <= DistanceToStartShooting)
            this.enemyState = EnemyStates.Attack;
        else
        {
            this.AStarPathing();
        }
        
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

    public void DetachFromPlayer()
    {
        this.enemyOb.Detach(this.PlayerReference.UnityPlayerSub);
    }
}
