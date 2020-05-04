using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    bool isAlive { get; set; }
    void initializePool();
}
