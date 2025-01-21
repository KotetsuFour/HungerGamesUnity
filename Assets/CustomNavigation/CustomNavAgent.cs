using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNavAgent : MonoBehaviour
{
    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private float tileSize;
    [SerializeField] private float speed;
    [SerializeField] private float acceptableDistance;
    private int shortTimeDest;
    private TileNode destination;
    private TileNode[] path;
    private bool moving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void setDestination(Vector3 dest)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1, tileLayer);
        if (hit.collider == null)
        {
            Debug.Log($"This agent is not on the map. Coords: {transform.position}");
            return;
        }
        TileNode here = hit.collider.GetComponent<TileNode>();

        RaycastHit hit2;
        Physics.Raycast(dest, Vector3.down, out hit2, 1, tileLayer);
        if (hit2.collider == null)
        {
            Debug.Log($"The destination is not on the map. Coords: {dest}");
            return;
        }
        destination = hit2.collider.GetComponent<TileNode>();

        path = getPath(here);
        shortTimeDest = 0;
    }
    public Vector3 getDestination()
    {
        return destination.transform.position;
    }

    public bool reachedDestination()
    {
        return shortTimeDest == path.Length;
    }

    public void setActive(bool active)
    {
        moving = active;
    }

    private TileNode[] getPath(TileNode startTile)
    {
        List<TileNode> open = new List<TileNode>();
        List<TileNode> closed = new List<TileNode>();
        Dictionary<TileNode, float> f = new Dictionary<TileNode, float>();
        Dictionary<TileNode, float> g = new Dictionary<TileNode, float>();
        Dictionary<TileNode, float> pos = new Dictionary<TileNode, float>();
        Dictionary<TileNode, TileNode> parent = new Dictionary<TileNode, TileNode>();

        open.Add(startTile);
        f.Add(startTile, 0);
        g.Add(startTile, 0);
        pos.Add(startTile, 0);
        while (open.Count > 0)
        {
            TileNode check = open[0];
            for (int idx = 1; idx < open.Count; idx++)
            {
                if (f[check] > f[open[idx]])
                {
                    check = open[idx];
                }
            }
            open.Remove(check);
            foreach (TileNode child in check.getConnections())
            {
                if (child == destination)
                {
                    parent.Add(child, check);
                    return interpretPath(parent, startTile);
                }
                if (closed.Contains(child)
                    || child.getType() == TileNode.IMPASS)
                {
                    continue;
                }
                addToDictionary(child, pos[check] + 1, pos);
                addToDictionary(child, g[check] + child.getType(), g);
                float h = (child.transform.position - check.transform.position).magnitude;
                float calculateF = g[child] + h;
                if (!f.ContainsKey(child) || f[child] > calculateF)
                {
                    open.Add(child);
                    addToDictionary(child, check, parent);
                    addToDictionary(child, calculateF, f);
                }
            }
            closed.Add(check);
        }
        return interpretPath(parent, startTile);
    }
    private TileNode[] interpretPath(Dictionary<TileNode, TileNode> pathData, TileNode startTile)
    {
        TileNode current = destination;
        List<TileNode> backwards = new List<TileNode>();
        while (current != startTile)
        {
            backwards.Add(current);
            current = pathData[current];
        }
        backwards.Add(startTile);
        TileNode[] ret = new TileNode[backwards.Count];
        for (int q = backwards.Count - 1; q >= 0; q--)
        {
            ret[(backwards.Count - 1) - q] = backwards[q];
        }
        return ret;
    }

    private void addToDictionary(TileNode key, TileNode value, Dictionary<TileNode, TileNode> dictionary)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
    private void addToDictionary(TileNode key, float value, Dictionary<TileNode, float> dictionary)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (moving && !reachedDestination())
        {
            if ((transform.position - path[shortTimeDest].transform.position).magnitude <= acceptableDistance)
            {
                shortTimeDest++;
            }
            if (!reachedDestination())
            {
                Vector3 direction = (path[shortTimeDest].transform.position - transform.position).normalized;
                transform.Translate(direction * speed);
                Quaternion look = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0, look.eulerAngles.y, 0);
            }
        }
    }
}
