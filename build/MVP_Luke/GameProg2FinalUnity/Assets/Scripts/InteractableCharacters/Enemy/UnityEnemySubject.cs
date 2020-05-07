using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEnemySubject : EnemySub
{

    private GameObject _gameObject;

    public UnityEnemySubject(GameObject g) : base()
    {
        _gameObject = g;
    }

    public override void Log(string s)
    {
        Debug.Log(s);
    }
}
