using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Vector2 Size;

    public GameObject Ground;
    public GameObject Way;
    public GameObject ShrineGround;
    public GameObject Shrine;
    public GameObject[] Objects;

    public GameObject[,,] Level;

    public IList<BoxCollider2D> WayBoxes;
    
	// Use this for initialization
	void Awake () {
        Generate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Generate()
    {
        WayBoxes = new List<BoxCollider2D>();

        Level = new GameObject[2,(int)Size.x, (int)Size.y];
        var center = transform.position;

        center.z = 1;

        for (var w = Size.x / 2 * -1; w < Size.x / 2; ++w)
        {
            for (var h = Size.y / 2 * -1; h < Size.y / 2; ++h)
            {
                var toInstantiate = Ground;

                if(w < 4 && w > -5 && h < 4 && h > -4)
                {
                    toInstantiate = ShrineGround;
                }

                var floor = (GameObject)Instantiate(toInstantiate, new Vector3(center.x + w + 0.5f, center.y + h + 0.5f, 1), new Quaternion());

                floor.transform.SetParent(transform, true);
                Level[0, (int)(w + Size.x /2), (int)(h + Size.y /2)] = floor;
            }
        }

        //Generate ways
        GenerateWays(Vector2.up, new Vector2(Size.x/2, Size.y/2 + 4));
        GenerateWays(Vector2.right, new Vector2(Size.x / 2 + 4, Size.y / 2));
        GenerateWays(Vector2.down, new Vector2(Size.x / 2, Size.y / 2 - 4));
        GenerateWays(Vector2.left, new Vector2(Size.x / 2 - 5, Size.y / 2));

        //Place Shrine
        var shrine = (GameObject)Instantiate(Shrine, center, new Quaternion());
        shrine.transform.SetParent(transform, true);

        //Place BoundingBox for shrine
        var shrineBox = new GameObject();

        var collider = shrineBox.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(8, 7);

        shrineBox.transform.position = new Vector3(0f, 0.5f, 0);
        shrineBox.transform.SetParent(transform);

        //Place objects
        var failure = 0;
        var i = 0;
        while (i < 6)
        {

            var home = Objects[0];

            var position = Vector3.zero;

            var baseSize = home.GetComponentInChildren<BoxCollider2D>().size;

            do
            {
                ++failure;
                var posX = Random.Range(0 + baseSize.x / 2, Size.x - baseSize.x / 2);
                var posY = Random.Range(0 + baseSize.y / 2, Size.y - baseSize.y / 2);

                if (posX < 4 && posX > -5 && posY < 5 && posY > -4)
                {
                    continue;
                }

                position = new Vector3(posX - Size.x / 2, posY - Size.y / 2, 1);
            } while (failure < 20 && Physics2D.BoxCast(position, new Vector2(baseSize.x + 2, baseSize.y + 10), 0, Vector2.zero));
            if(failure >= 20)
            {
                break;
            }
            failure = 0;
            ++i;
            var obj = (GameObject)Instantiate(home, position, new Quaternion());

            obj.transform.FindChild("Top").gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)position.y * -1;

            obj.transform.SetParent(transform, true);
        }

        failure = 0;
        i = 0;
        while (i < 15)
        {

            var home = Objects[1];

            var position = Vector3.zero;

            var baseSize = home.GetComponentInChildren<BoxCollider2D>().size;

            do
            {
                if (failure > 20)
                {
                    return;
                }
                ++failure;
                var posX = Random.Range(0 + baseSize.x / 2, Size.x - baseSize.x / 2);
                var posY = Random.Range(0 + baseSize.y / 2, Size.y - baseSize.y / 2);

                position = new Vector3(posX - Size.x / 2, posY - Size.y / 2, 1);
            } while (Physics2D.BoxCast(position, new Vector2(baseSize.x + 1, baseSize.y + 1), 0, Vector2.zero));
            failure = 0;
            ++i;
            var obj = (GameObject)Instantiate(home, position, new Quaternion());

            obj.transform.FindChild("Top").gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)position.y * -1;

            obj.transform.SetParent(transform, true);
        }


        foreach(var box in WayBoxes)
        {
            Destroy(box);
        }
        Destroy(shrineBox);
    }

    private void GenerateWays(Vector2 direction, Vector2 startPoint, Vector2 startDirection = default(Vector2), int length = -1, int thickness = -1, bool goFurther = true)
    {
        length = length == -1?Random.Range(8, 15):length;;

        var left = new Vector2(-direction.y, -direction.x);
        var right = new Vector2(direction.y, direction.x);
        var currentPoint = startPoint;

        thickness = thickness == -1?3:thickness;

        for(var i = 0; i < length; ++i )
        {
            if (currentPoint.x >= Size.x || currentPoint.y >= Size.y || currentPoint.x < 0 || currentPoint.y < 0)
            {
                goFurther = false;
                break;
            }

            if (currentPoint.x < Size.x /2 + 4 && currentPoint.x > Size.x / 2 - 5 && currentPoint.y < Size.y / 2 + 5 && currentPoint.y > Size.y /2 -4)
            { 
                currentPoint += direction;
                continue;
            }

            var oldGO = Level[0, (int)currentPoint.x, (int)currentPoint.y];

            if(thickness > 1 && oldGO.name == string.Format("{0}(Clone)", Way.name))
            {
                currentPoint += direction;
                length = i + 1;
                goFurther = false;
                break;
            }

            var newGO = (GameObject)Instantiate(Way, oldGO.transform.position, new Quaternion());
            var box = newGO.AddComponent<BoxCollider2D>();

            box.size = new Vector2(1, 1);

            WayBoxes.Add(box);

            Destroy(oldGO);

            Level[0, (int)currentPoint.x, (int)currentPoint.y] = newGO;

            currentPoint += direction;
        }

        if(thickness > 1)
        {
            for(var i = 0; i < thickness /2; ++i)
            {
                GenerateWays(direction, startPoint + (left * (i +1)), startDirection, length, 1, false);
            }
        }
        if(thickness > 2)
        {
            for (var i = 0; i < thickness / 2; ++i)
            {
                GenerateWays(direction, startPoint + (right * (i + 1)), startDirection, length, 1, false);
            }
        }

        if (!goFurther) return;
        if(startDirection == default(Vector2))
        {
            startDirection = direction;
        }
        var oldDirection = direction;
        var counter = 0;
        do
        {
            var r = Random.Range(0, 4);
            switch (r)
            {
                case 0:
                    direction = Vector2.up;
                    break;
                case 1:
                    direction = Vector2.right;
                    break;
                case 2:
                    direction = Vector2.down;
                    break;
                case 3:
                    direction = Vector2.left;
                    break;
            }

            counter++;
            if(counter > 500)
            {
                throw new System.Exception("ASDHASDKJASD");
            }
        } while ((oldDirection + direction) == Vector2.zero || startDirection + direction == Vector2.zero);
        
        GenerateWays(direction, currentPoint, startDirection);
        if(Random.Range(0,3) == 0 && direction + oldDirection != Vector2.zero)
        {
            GenerateWays(direction * -1, currentPoint + direction *-1, startDirection);
        }

        if (Random.Range(0, 50) == 0 && direction * -1 + oldDirection != Vector2.zero)
        {
            GenerateWays(oldDirection, currentPoint, startDirection);
        }
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
