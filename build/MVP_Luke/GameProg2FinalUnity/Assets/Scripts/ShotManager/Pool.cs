using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public GameObject ShotPrefab;

    public int InitialPoolSize = 1;
    public int MaxPoolSize = 5;

    private void Awake()
    {
    }

    void Start()
    {       
        CreateObjectPools();
    }

    private void CreateObjectPools()
    {
        ObjectPoolManager.Instance.CreatePool(ShotPrefab, this.InitialPoolSize, this.MaxPoolSize, false);
        ObjectPoolManager.Instance.PoolGameObject = this.gameObject;
    }

    private void FixedUpdate()
    {
        
    }
}
