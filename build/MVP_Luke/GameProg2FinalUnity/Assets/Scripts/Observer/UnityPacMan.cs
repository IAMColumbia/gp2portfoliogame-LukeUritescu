using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityPlayerSubject : PlayerSubject
{
    private GameObject _gameObject;

    public UnityPlayerSubject(GameObject g) : base()
    {
        _gameObject = g;
    }

    public override void Log(string s)
    {
        Debug.Log(s);
    }
}
