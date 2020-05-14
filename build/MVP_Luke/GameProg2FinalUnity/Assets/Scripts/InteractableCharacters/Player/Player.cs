using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityCommand;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Caster, ICommandComponent
{
    private Vector3 rayDirection;

    public UnityPlayerSubject UnityPlayerSub { get; private set; }

    public int HitDistance = 15;

    public float Mass = 5.0f;
    private Vector3 velocity;
    public float Radius = 0.001f;
    private int curPathIndex;
    private bool isEnd;

    private Transform startPos, endPos;
    public Node StartNode { get; set; }
    public Node GoalNode { get; set; }

    public List<Node> PathArray;

    private float elapsedTime = 0.0f;
    public float IntervalTime = 5.0f; //Interval time between path finding


    public enum PlayerStates { NoAction, Moving, AStar, OnlyAiming, Attack}

    //public SpellSpawner SpellSpawnerReference;

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private ManaBar manaBar;
    
    //public GameObject Projectile;
    [SerializeField]
    public PlayerStates PlayerState;
    public Transform TargetTransform;

    [SerializeField]
    private float rotationSpeed = 15f;

    void Awake()
    {
        UnityPlayerSub = new UnityPlayerSubject(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxHP = 150f;
        CurrentHP = MaxHP;
        MaxManaPool = 200f;
        CurrentManaPool = MaxManaPool;
        PlayerState = PlayerStates.NoAction;
        OwnerOfShot = PlayerOrEnemyShot.Player;
        this.UserCondition = Conditions.NoCondition;

        velocity = transform.forward;

        isEnd = false;
        //AStar Calculated Path
        PathArray = new List<Node>();
    }

    public void PassiveManaRestore()
    {
        if(elapsedTime >= IntervalTime)
        {
            this.CurrentManaPool += 4f;
            this.manaBar.RestoreMana(4);
            elapsedTime = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (this.CurrentManaPool <= this.MaxManaPool)
        {
            PassiveManaRestore();

        }
        if(this.CurrentManaPool > this.MaxManaPool)
        {
            this.CurrentManaPool = this.MaxManaPool;
        }
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
        PlayerDead();
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
        StartNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        GoalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));
        PathArray = AStar.FindPath(StartNode, GoalNode);
        curPathIndex = 0;

    }

    void OnDrawGizmos()
    {
        if (PathArray == null)
            return;

        if (PathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in PathArray)
            {
                if (index < PathArray.Count)
                {
                    Node nextNode = (Node)PathArray[index];
                    Debug.DrawLine(node.Position, nextNode.Position, Color.green);
                    index++;
                }
            };
        }
    }
    public void MovingToDestination()
    {
        if (DetectObstacle())
        {
            FindPath();
            this.PlayerState = PlayerStates.AStar;
            return;
        }

        Vector3 tarPos = TargetTransform.position;
        tarPos.y = 0.5f;
        Vector3 dirRot = tarPos - transform.position;

        if (Vector3.Distance(transform.position, TargetTransform.position) < 2.5f)
            return;

        Quaternion tarRotation = Quaternion.LookRotation(dirRot);
        tarRotation.eulerAngles = new Vector3(0, tarRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
        
    }

    public void AStarPathing()
    {
        float curSpeed = movementSpeed * Time.deltaTime;
        Vector3 tarPos = PathArray[curPathIndex].Position;

        //tarPos.y = 0.5f;
        if (PathArray.Count > 0)
        {

            if (Vector3.Distance(transform.position, PathArray[curPathIndex].Position) < Radius)
            {
                if (curPathIndex < PathArray.Count -1)
                    curPathIndex++;
                if (isEnd)
                    curPathIndex = 0;
                else
                    return;
            }
            if (curPathIndex >= PathArray.Count)
            {

                isEnd = true;
                return;
            }


            if (curPathIndex >= PathArray.Count - 1 && !isEnd)
                velocity += Steer(PathArray[curPathIndex].Position, curSpeed, true);
            else
                velocity += Steer(PathArray[curPathIndex].Position, curSpeed);
            Vector3 dirRot = PathArray[curPathIndex].Position - transform.position;
            Quaternion tarRotation = Quaternion.LookRotation(dirRot);
            tarRotation.eulerAngles = new Vector3(0, tarRotation.eulerAngles.y, 0);
            transform.position += velocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);
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
        if (DetectObstacle() && PathArray.Count > 0 && TargetTransform.GetComponent<Target>().TagIfObstacle == false)
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
        if(CurrentManaPool >= 8f)
        {
        this.GetComponent<ArcaneBoltSpawner>().SpawnTheObject(this.gameObject);
        CastSpell(8f);

        }
        //This should pass through the reference of ArcaneBolt set in SpawnedObject to the SetupSpawnObject function 
    }

    public void CastFireBolt()
    {
        this.PlayerState = PlayerStates.Attack;
        if(CurrentManaPool >= 15f)
        {

        this.GetComponent<FireBoltSpawner>().SpawnTheObject(this.gameObject);
        CastSpell(15f);
        }
    }

    public void CastIceBolt()
    {
        this.PlayerState = PlayerStates.Attack;
        if(CurrentManaPool >= 5f)
        {

        this.GetComponent<IceBoltSpawner>().SpawnTheObject(this.gameObject);
        CastSpell(5f);
        }
    }

    public override void CastSpell(float ManaUsage)
    {
        Debug.Log("HELLO");
        if(manaBar != null)
        {
            manaBar.Decrease(ManaUsage);
            this.CurrentManaPool -= ManaUsage;
        }        
    }

    public override void TakeDamage(float DamageAmount)
    {
        if(healthBar != null)
        {
            healthBar.Damage(DamageAmount);
            this.CurrentHP -= DamageAmount;
        }
    }

    public void PlayerDead()
    {
        if(this.CurrentHP <= 0)
        {
            UnityPlayerSub.Notify("PlayerDead");
        }
    }

    public void RestartGame()
    {
        UnityPlayerSub.Notify("RestartGame");
    }

    public void ExitGame()
    {
        UnityPlayerSub.Notify("ExitGame");
    }
}
