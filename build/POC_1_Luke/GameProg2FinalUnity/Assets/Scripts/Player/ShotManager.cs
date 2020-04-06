using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    List<ShotSprite> Shots;
    private List<ShotSprite> shotsToRemove;
    public GameObject preFab;

    // Start is called before the first frame update
    void Start()
    {
        Shots = new List<ShotSprite>();
        shotsToRemove = new List<ShotSprite>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddShot(Vector3 Position, Vector3 Direction)
    {
        var shot = new ShotSprite() { Speed = 90f, Direction = Direction };
        Shots.Add((ShotSprite)Instantiate(shot, Position, Quaternion.identity));
        shot.State = ShotSprite.ShotState.Shooting;
        Shots.Add(shot);
    }

    private void RemoveDisabledShots()
    {
        shotsToRemove.Clear();
        foreach(ShotSprite s in Shots)
        {
            if (s.enabled == false)
                shotsToRemove.Add(s);
        }

        foreach(var shotToRemove in shotsToRemove)
        {
            this.Shots.Remove(shotToRemove);
        }
    }
}
