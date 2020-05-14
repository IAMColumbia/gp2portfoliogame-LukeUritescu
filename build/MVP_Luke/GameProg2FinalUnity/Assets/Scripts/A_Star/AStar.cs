using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar
{
    #region List fields

    public static PriorityQueue OpenList;
    public static HashSet<Node> ClosedList;

    #endregion

    /// <summary>
    /// Calculate the final path in the path finding
    /// </summary>
    private static List<Node> CalculatePath(Node node)
    {
        List<Node> list = new List<Node>();
        while (node != null)
        {
            list.Add(node);
            node = node.Parent;
        }
        list.Reverse();
        return list;
    }

    /// <summary>
    /// Calculate the estimated Heuristic cost to the goal
    /// </summary>
    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        Vector3 vecCost = curNode.Position - goalNode.Position;
        return vecCost.magnitude;
    }

    /// <summary>
    /// Find the path between start node and goal node using AStar Algorithm
    /// </summary>
    public static List<Node> FindPath(Node start, Node goal)
    {
        //Start Finding the path
        OpenList = new PriorityQueue();
        OpenList.Push(start);
        start.NodeTotalCost = 0.0f;
        start.EstimatedCost = HeuristicEstimateCost(start, goal);

        ClosedList = new HashSet<Node>();
        Node node = null;

        while (OpenList.Length != 0)
        {
            node = OpenList.First();

            if (node.Position == goal.Position)
            {
                return CalculatePath(node);
            }

            List<Node> neighbours = new List<Node>();
            GridManager.instance.GetNeighbours(node, neighbours);

            #region CheckNeighbours

            //Get the Neighbours
            for (int i = 0; i < neighbours.Count; i++)
            {
                //Cost between neighbour nodes
                Node neighbourNode = (Node)neighbours[i];

                if (!ClosedList.Contains(neighbourNode))
                {
                    //Cost from current node to this neighbour node
                    float cost = HeuristicEstimateCost(node, neighbourNode);

                    //Total Cost So Far from start to this neighbour node
                    float totalCost = node.NodeTotalCost + cost;

                    //Estimated cost for neighbour node to the goal
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    //Assign neighbour node properties
                    neighbourNode.NodeTotalCost = totalCost;
                    neighbourNode.Parent = node;
                    neighbourNode.EstimatedCost = totalCost + neighbourNodeEstCost;

                    //Add the neighbour node to the list if not already existed in the list
                    if (!OpenList.Contains(neighbourNode))
                    {
                        OpenList.Push(neighbourNode);
                    }
                }
            }

            #endregion

            ClosedList.Add(node);
            OpenList.Remove(node);
        }

        //If finished looping and cannot find the goal then return null
        if (node.Position != goal.Position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }

        //Calculate the path based on the final node
        return CalculatePath(node);
    }
}
