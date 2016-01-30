using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    public Vector2 Size;

    public GameObject Ground;
    public GameObject Shrine;
    public GameObject[] Objects;
    
	// Use this for initialization
	void Awake () {
        Generate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Generate()
    {
        var center = transform.position;

        center.z = 1;

        for (var w = Size.x / 2 * -1; w < Size.x / 2; ++w)
        {
            for (var h = Size.y / 2 * -1; h < Size.y / 2; ++h)
            {
                var floor = (GameObject)Instantiate(Ground, new Vector3(center.x + w + 0.5f, center.y + h + 0.5f, 1), new Quaternion());

                //floor.transform.SetParent(transform);
            }
        }

        //Place Shrine
        var shrine = (GameObject)Instantiate(Shrine, center, new Quaternion());
    }

    void OnDrawGizmos()
    {
        return;

        var center = transform.position;
        
        Gizmos.DrawWireCube(center, new Vector3(Size.x, Size.y, 1));

        for(var w = Size.x/2*-1; w < Size.x/2; ++w)
        {
            for(var h = Size.y/2*-1; h < Size.y/2; ++h)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawCube(new Vector3(center.x + w +0.5f, center.y + h + 0.5f, 1) , Vector3.one);
            }
        }

        //Place Shrine
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(center, new Vector3(5, 6, 1));

        //Place Stuff


        /*if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.WorldPosition, Vector3.one);
            }
        }*/
    }
}
