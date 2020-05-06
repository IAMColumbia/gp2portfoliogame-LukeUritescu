using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityCommand;
using UnityEditor;
using UnityEngine;

public class Player : Caster, ICommandComponent
{
    private Vector3 rayDirection;

    public UnityPacMan PacMan { get; private set; }

    public int HitDistance = 15;

    public float Mass = 5.0f;
    private Vector3 velocity;
    public float Radius = 0.001f;
    private int curPathIndex;
    private bool isEnd;

    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public List<Node> pathArray;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding


    public enum PlayerStates { NoAction, Moving, AStar, OnlyAiming, Attack }

    //public SpellSpawner SpellSpawnerReference;

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private ManaBar manaBar;
    
    //public GameObject Projectile;
    [SerializeField]
    public PlayerStates PlayerState;
    private int viewDistance = 10;
    public Transform TargetTransform;

    [SerializeField]
    private float rotationSpeed = 15f;

    private bool lip;

    void Awake()
    {
        PacMan = new UnityPacMan(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        lip = true;
        MaxHP = 100f;
        CurrentHP = MaxHP;
        MaxManaPool = 700f;
        CurrentManaPool = MaxManaPool;
        PlayerState = PlayerStates.NoAction;
        OwnerOfShot = PlayerOrEnemyShot.Player;

        velocity = transform.forward;

        isEnd = false;
        //AStar Calculated Path
        pathArray = new List<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(this.transform.position, (transform.position + (transform.forward * HitDistance)), Color.red);
        switch (PlayerState)
        {
            case PlayerStates.NoAction:
                break;
            case PlayerStates.Moving:
                MovingToDestination();
                break;
            case PlayerStates.AStar:
                AStarPathing();
                break;
            case PlayerStates.Attack:
                break;
            case PlayerStates.OnlyAiming:
                OnlyAiming();
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
    }

    public bool DetectObstacle()
    {
        
        RaycastHit hit;
        rayDirection = this.transform.forward;
        if(Physics.Raycast(transform.position, rayDirection, out hit, HitDistance))
        {
           string tag  = hit.collider.gameObject.tag;
            if(tag != null)
            {

                if(tag == "Obstacle")
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
        endPos = TargetTransform;

        //Assign StartNode and Goal Node
        startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));
        pathArray = AStar.FindPath(startNode, goalNode);
        curPathIndex = 0;

    }

    void OnDrawGizmos()
    {
        if (pathArray == null)
            return;

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);
                    index++;
                }
            };
        }
    }
    public void MovingToDestination()
    {
        if (DetectObstacle())
        {
            this.PlayerState = PlayerStates.AStar;
            return;
        }

        Vector3 tarPos = TargetTransform.position;
        tarPos.y = 0.5f;
        Vector3 dirRot = tarPos - transform.position;

        if (Vector3.Distance(transform.position, TargetTransform.position) < 2.5f)
            return;

        Quaternion tarRotation = Quaternion.LookRotation(dirRot);


        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
        
    }

    public void AStarPathing()
    {
        float curSpeed = movementSpeed * Time.deltaTime;
        Vector3 tarPos = pathArray[curPathIndex].position;
        //tarPos.y = 0.5f;
        if (pathArray.Count > 0)
        {

            if (Vector3.Distance(transform.position, pathArray[curPathIndex].position) < Radius)
            {
                if (curPathIndex < pathArray.Count - 1)
                    curPathIndex++;
                if (isEnd)
                    curPathIndex = 0;
                else
                    return;
            }
            if (curPathIndex >= pathArray.Count - 1)
            {

                isEnd = true;
                return;
            }


            if (curPathIndex >= pathArray.Count - 1 && !isEnd)
                velocity += Steer(pathArray[curPathIndex].position, curSpeed, true);
            else
                velocity += Steer(pathArray[curPathIndex].position, curSpeed);

            transform.position += velocity;
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    public Vector3 Steer(Vector3 target, float curSpeed,bool bFinalPoint = false)    
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

    public void OnlyAiming()
    {
        Vector3 tarPos = TargetTransform.position;
        tarPos.y = 0.5f;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRotation = Quaternion.LookRotation(dirRot);

        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);
    }


    //If the direction 
    public void Moving()
    {
        FindPath();
        if (DetectObstacle() && pathArray.Count > 0 && TargetTransform.GetComponent<Target>().TagIfObstacle == false)
        {
            this.PlayerState = PlayerStates.AStar;
        }
        else
        {
            this.PlayerState = PlayerStates.Moving;
        }
            isEnd = false;
    }

    public void OnlyAim()
    {
        PlayerState = PlayerStates.OnlyAiming;
    }

    public void CastArcaneBolt()
    {
        this.PlayerState = PlayerStates.Attack;
        if(CurrentManaPool >= 10f)
        {
        this.GetComponent<ArcaneBoltSpawner>().SpawnTheObject();
        CastSpell(8f);

        }
        //This should pass through the reference of ArcaneBolt set in SpawnedObject to the SetupSpawnObject function 
    }

    public void CastFireBolt()
    {
        this.PlayerState = PlayerStates.Attack;
        if(CurrentManaPool >= 15f)
        {

        this.GetComponent<FireBoltSpawner>().SpawnTheObject();
        CastSpell(15f);
        }
    }

    public void CastIceBolt()
    {
        this.PlayerState = PlayerStates.Attack;
        if(CurrentManaPool >= 5f)
        {

        this.GetComponent<IceBoltSpawner>().SpawnTheObject();
        CastSpell(5f);
        }
    }

    public override void CastSpell(float ManaUsage)
    {
        Debug.Log("HELLO");
        if(manaBar != null)
        {
            manaBar.Decrease(ManaUsage);
            MaxManaPool -= ManaUsage;
        }
        this.CurrentManaPool -= ManaUsage;
        
    }

    public override void TakeDamage(float DamageAmount)
    {
        if(healthBar != null)
        {
            healthBar.Damage(DamageAmount);
            MaxHP -= DamageAmount;
        }
    }
}
