using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestCode : MonoBehaviour
{
    private Transform startPos, endPos;
    public Node StartNode { get; set; }
    public Node GoalNode { get; set; }

    public List<Node> PathArray;

    GameObject ObjStartCube, ObjEndCube;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding

    // Use this for initialization
    void Start()
    {
        ObjStartCube = GameObject.FindGameObjectWithTag("Start");
        ObjEndCube = GameObject.FindGameObjectWithTag("End");

        //AStar Calculated Path
        PathArray = new List<Node>();
        FindPath();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }
    }

    void FindPath()
    {
        startPos = ObjStartCube.transform;
        endPos = ObjEndCube.transform;

        //Assign StartNode and Goal Node
        StartNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        GoalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));

        PathArray = AStar.FindPath(StartNode, GoalNode);
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
}