using UnityEngine;
using System.Collections;
using System;

public class Node : IComparable
{
    #region Fields
    public float NodeTotalCost;         //Total cost so far for the node
    public float EstimatedCost;         //Estimated cost from this node to the goal node
    public bool BObstacle;              //Does the node is an obstacle or not
    public Node Parent;                 //Parent of the node in the linked list
    public Vector3 Position;            //Position of the node
    #endregion

    /// <summary>
    //Default Constructor
    /// </summary>
    public Node()
    {
        this.EstimatedCost = 0.0f;
        this.NodeTotalCost = 1.0f;
        this.BObstacle = false;
        this.Parent = null;
    }

    /// <summary>
    //Constructor with adding position to the node creation
    /// </summary>
    public Node(Vector3 pos)
    {
        this.EstimatedCost = 0.0f;
        this.NodeTotalCost = 1.0f;
        this.BObstacle = false;
        this.Parent = null;

        this.Position = pos;
    }

    /// <summary>
    //Make the node to be noted as an obstacle
    /// </summary>
    public void MarkAsObstacle()
    {
        this.BObstacle = true;
    }

    /// <summary>
    // This CompareTo methods affect on Sort method
    // It applies when calling the Sort method from ArrayList
    // Compare using the estimated total cost between two nodes
    /// </summary>
    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (this.EstimatedCost < node.EstimatedCost)
            return -1;
        if (this.EstimatedCost > node.EstimatedCost)
            return 1;

        return 0;
    }
}


