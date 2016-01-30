using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Vector2 Size;

    public GameObject Ground;
    public GameObject ShrineGround;
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
                var toInstantiate = Ground;

                if(w < 4 && w > -5 && h < 4 && h > -4)
                {
                    toInstantiate = ShrineGround;
                }

                var floor = (GameObject)Instantiate(toInstantiate, new Vector3(center.x + w + 0.5f, center.y + h + 0.5f, 1), new Quaternion());

                floor.transform.SetParent(transform, true);
            }
        }

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
                if(failure > 20)
                {
                    return;
                }
                ++failure;
                var posX = Random.Range(0 + baseSize.x / 2, Size.x - baseSize.x / 2);
                var posY = Random.Range(0 + baseSize.y / 2, Size.y - baseSize.y / 2);

                if (posX < 4 && posX > -5 && posY < 5 && posY > -4)
                {
                    continue;
                }

                position = new Vector3(posX - Size.x / 2, posY - Size.y / 2, 1);
            } while (Physics2D.BoxCast(position, new Vector2(baseSize.x + 2, baseSize.y + 2), 0, Vector2.zero));
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

        Destroy(shrineBox);
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
