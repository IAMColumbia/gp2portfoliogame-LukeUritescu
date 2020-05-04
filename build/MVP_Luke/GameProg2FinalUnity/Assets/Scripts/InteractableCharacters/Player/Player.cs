using System.Collections;
using System.Collections.Generic;
using UnityCommand;
using UnityEngine;

public class Player : Caster, ICommandComponent
{
    public enum PlayerStates { NoAction, Moving, OnlyAiming, Attack }

    //public SpellSpawner SpellSpawnerReference;

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private ManaBar manaBar;
    
    //public GameObject Projectile;
    [SerializeField]
    public PlayerStates PlayerState;
    private Transform projectileSpawnPoint;
    private int viewDistance = 10;
    public Transform TargetTransform;

    [SerializeField]
    private float rotationSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        this.movementSpeed = this.MaxMovementSpeed = 10.0f;
        MaxHP = 100f;
        CurrentHP = MaxHP;
        MaxManaPool = 300f;
        CurrentManaPool = MaxManaPool;
        PlayerState = PlayerStates.NoAction;
        OwnerOfShot = PlayerOrEnemyShot.Player;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(this.transform.position, (transform.position + (transform.forward * viewDistance)), Color.red);
        switch (PlayerState)
        {
            case PlayerStates.NoAction:
                break;
            case PlayerStates.Moving:
                //if (this.movementSpeed != movementSpeedHolder)
                //    movementSpeed = movementSpeedHolder;
                MovingToDestination();
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
    }

    public void MovingToDestination()
    {
        Vector3 tarPos = TargetTransform.position;
        tarPos.y = 0.5f;
        Vector3 dirRot = tarPos - transform.position;

        if (Vector3.Distance(transform.position, TargetTransform.position) < 2.5f)
            return;

        Quaternion tarRotation = Quaternion.LookRotation(dirRot);


        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }

    public void OnlyAiming()
    {
        Vector3 tarPos = TargetTransform.position;
        tarPos.y = 0.5f;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRotation = Quaternion.LookRotation(dirRot);

        transform.rotation = Quaternion.Slerp(transform.rotation, tarRotation, rotationSpeed * Time.deltaTime);
    }

    public void Moving()
    {
        PlayerState = PlayerStates.Moving;

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
