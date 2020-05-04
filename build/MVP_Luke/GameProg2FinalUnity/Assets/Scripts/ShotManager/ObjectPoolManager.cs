//https://blogs.msdn.microsoft.com/dave_crooks_dev_blog/2014/07/21/object-pooling-for-unity3d/
/*
 * @Author: David Crook
 *
 * Use the object pools to help reduce object instantiation time and performance
 * with objects that are frequently created and used.
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class ObjectPoolManager
{
    //variable is declared to be volatile to ensure that assignment to the instance variable completes before instance variable can be accessed

    private static volatile ObjectPoolManager instance;

    //look up list of various object pools
    private Dictionary<string, ObjectPool> objectPools;

    //object for locking
    private static object syncRoot = new System.Object();


    /// <summary>
    /// Constructor for the class
    /// </summary>
    /// 
    private ObjectPoolManager()
    {
        //Ensure object pools exists
        this.objectPools = new Dictionary<string, ObjectPool>();
    }

    /// <summary>
    /// Propery for retreiving the singleton. See MSDN documention
    /// </summary>
    /// 
    public static ObjectPoolManager Instance
    {
        get
        {
            if(instance == null)
            {
                lock (syncRoot)
                {
                    if(instance == null)
                    {
                        instance = new ObjectPoolManager();
                    }
                }
            }
            //return either the new instance or the already built one
            return instance;
        }
    }

    public GameObject PoolGameObject { get; internal set; }

    /// <summary>
    /// Create a new object pool of the objects you wish to pool
    /// </summary>
    /// <param name="objToPool">The object you wish to pool.  The name property of the object MUST be unique.</param>
    /// <param name="initialPoolSize">Number of objects you wish to instantiate initially for the pool.</param>
    /// <param name="maxPoolSize">Maximum number of objects allowed to exist in this pool.</param>
    /// <param name="shouldShrink">Should this pool shrink back down to the initial size when it receives a shrink event.</param>
    /// <returns></returns>
    public bool CreatePool(GameObject objToPool, int initialPoolSize, int maxPoolSize, bool shouldShrink)
    {
        //Check to see if the pool already exists.
        if (ObjectPoolManager.Instance.objectPools.ContainsKey(objToPool.name))
        {
            //let the caller know it already exists, just use the pool out there.
            return false;
        }
        else
        {
            //create a new pool using the properties
            ObjectPool nPool = new ObjectPool(objToPool, initialPoolSize, maxPoolSize, shouldShrink);
            //Add the pool to the dictionary of pools to manage
            //using the object name as the key and the pool as the value.
            ObjectPoolManager.Instance.objectPools.Add(objToPool.name, nPool);
            //We created a new pool!
            return true;
        }
    }

    /// <summary>
    /// Get an object from the pool.
    /// </summary>
    /// <param name="objName">String name of the object you wish to have access to.</param>
    /// <returns>A GameObject if one is available, else returns null if all are currently active and max size is reached.</returns>
    public GameObject GetObject(string objName)
    {
        //Find the right pool and ask it for an object.
        return ObjectPoolManager.Instance.objectPools[objName].GetObject();
    }

}
