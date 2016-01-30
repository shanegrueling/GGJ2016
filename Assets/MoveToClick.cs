using System;
using System.Collections.Generic;
using UnityEngine;
using Node = NavGrid.Node;

public class MoveToClick : MonoBehaviour {

	public bool inConversationOrMenu = false;

	NavMeshAgent agent;
    private Vector3 destination;
    private NavGrid grid;
    private Queue<Node> _path;



    void Start()
    {
        grid = FindObjectOfType<NavGrid>();
        _path = new Queue<Node>();
    }



    void Update()
    {
		if (!inConversationOrMenu && Input.GetMouseButtonDown(0))
        {
            var mousPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousPoint.z = 0;
            FindPath(transform.position, mousPoint);
        }
    }



    void FixedUpdate()
    {
        if (_path.Count == 0)
        {
            GetComponent<Animator>().SetInteger("Direction", 0);
            return;
        }

        var target = _path.Peek();

        if (target.WorldPosition.x > Math.Round(transform.position.x*2)/2)
        {
			GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<Animator>().SetInteger("Direction", 2);
        }
        else if (target.WorldPosition.x < Math.Round(transform.position.x * 2) / 2)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Animator>().SetInteger("Direction", 4);
        }
        else if (target.WorldPosition.y > Math.Round(transform.position.y * 2) / 2)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Animator>().SetInteger("Direction", 1);
        }
        else if (target.WorldPosition.y < Math.Round(transform.position.y * 2) / 2)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Animator>().SetInteger("Direction", 3);
		}

		transform.position = Vector3.MoveTowards(transform.position, target.WorldPosition, 2f * Time.fixedDeltaTime);

        if (transform.position == target.WorldPosition)
        {
            _path.Dequeue();
        }
    }



	/**
	 * position & camera update
	 */
    public void LateUpdate()
    {
        var position = transform.position;

        position.z = Camera.main.transform.position.z;

        Camera.main.transform.position = position;
    }



    private class Waypoint
    {
        public Node Node;
        public int GCost;
        public int FCost { get { return GCost + HCost; } }
        public int HCost;

        public Waypoint Parent { get; internal set; }

        public override int GetHashCode()
        {
            return Node.GetHashCode();
        }
    }



    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Waypoint startNode = new Waypoint
        {
            Node = grid.NodeFromWorldPoint(startPos),
            GCost = 0,
        };
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (!targetNode.Walkable) return;


        List<Waypoint> openSet = new List<Waypoint>();
        HashSet<Waypoint> closedSet = new HashSet<Waypoint>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Waypoint currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {

                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost <= currentNode.HCost) {
					
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.Node == targetNode) {

                RetracePath(startNode.Node, targetNode, currentNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode.Node)) {

                var wayPoint = new Waypoint() { Node = neighbour };
                if (!neighbour.Walkable || closedSet.Contains(wayPoint)) {

					continue;
                }

                int newMovementCostToNeighbour = currentNode.GCost + 1;

				if (newMovementCostToNeighbour < wayPoint.GCost || !openSet.Contains(wayPoint)) {

                    wayPoint.GCost = newMovementCostToNeighbour;
                    wayPoint.HCost = (int)distance(neighbour.X, neighbour.Y, targetNode.X, targetNode.Y);
                    wayPoint.Parent = currentNode;

					if (!openSet.Contains (wayPoint)) {

						openSet.Add (wayPoint);
					}
                }
            }
        }
    }



    private double distance(int x1, int y1, int x2, int y2)
    {
        int dx = Math.Abs(x2 - x1);
        int dy = Math.Abs(y2 - y1);

        int min = Math.Min(dx, dy);
        int max = Math.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return Math.Sqrt(2) * diagonalSteps + straightSteps;
    }



    void RetracePath(Node startNode, Node endNode, Waypoint endWayPoint)
    {
        List<Node> path = new List<Node>();
        var currentNode = endWayPoint;

        while (currentNode.Node != startNode)
        {
            path.Add(currentNode.Node);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        grid.path = path;

        _path = new Queue<Node>(path);

    }
}
