using UnityEngine;
using System.Collections.Generic;
using System;

public class NavGrid : MonoBehaviour {

    public class Node
    {
        public bool Walkable;
        public Vector3 WorldPosition;
        public int X;
        public int Y;
		public string Tag;
    }

    public Vector3 BottomLeft;

    public int Width;
    public int Height;

    private Node[,] grid;

	// Use this for initialization
	void Start () {
        grid = new Node[Width, Height];
        var center = transform.position;

        for (int x = Width/2*-1; x < Width/2; x++)
        {
            for (int y = Height/2*-1; y < Height/2; y++)
            {
                Vector3 worldPoint = new Vector3(center.x + x + 0.5f, center.y + y + 0.5f, 1);
                bool walkable = !(Physics2D.CircleCast(worldPoint, 0.1f, Vector3.zero));

                grid[x + Width / 2, y + Height / 2] = new Node
                {
                    Walkable = walkable,
                    WorldPosition = worldPoint,
                    X = x + Width / 2,
                    Y = y + Height / 2,
                };
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0
                    || x == -1 && y == -1
                    || x == 1 && y == 1
                    || x == 1 && y == -1
                    || x == -1 && y == 1)
                    continue;

                int checkX = node.X + x;
                int checkY = node.Y + y;

                if (checkX >= 0 && checkX < Width && checkY >= 0 && checkY < Height)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = (int)Math.Round(worldPosition.x - BottomLeft.x);
        int y = (int)Math.Round(worldPosition.y - BottomLeft.y);

		return grid [x, y];
    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        var center = transform.position;

        Gizmos.DrawWireCube(center, new Vector3(Width, Height, 1));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.WorldPosition, Vector3.one);
            }
        }
    }
}
