using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNavMesh : MonoBehaviour
{
    [SerializeField] private float maxX;
    [SerializeField] private float minX;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;
    [SerializeField] private float maxZ;
    [SerializeField] private float minZ;
    [SerializeField] private float tileSize;
    [SerializeField] private LayerMask land;
    [SerializeField] private LayerMask water;
    [SerializeField] private LayerMask deco;
    [SerializeField] private LayerMask landAndWater;

    [SerializeField] private TileNode tile;

    // Start is called before the first frame update
    void Start()
    {
        bake(minX, maxX, minY, maxY, minZ, maxZ);
    }

    public void bake(float startX, float endX, float startY, float endY, float startZ, float endZ)
    {
        for (float x = startX; x <= endX; x += tileSize)
        {
            for (float z = startZ; z <= endZ; z += tileSize)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, endY, z), Vector3.down, out hit, endY - startY, land))
                {
                    TileNode node = Instantiate(tile, hit.point, Quaternion.identity);
                    node.setType(TileNode.LAND);
                    node.setLength(tileSize);
                    node.connect(tileSize * 1.5f);
                    node.transform.SetParent(transform);
                }
                if (Physics.Raycast(new Vector3(x, endY, z), Vector3.down, out hit, endY - startY, water))
                {
                    TileNode node = Instantiate(tile, hit.point, Quaternion.identity);
                    node.setType(TileNode.WATER);
                    node.setLength(tileSize);
                    node.connect(tileSize * 1.5f);
                    node.transform.SetParent(transform);
                }
                float decoHeight = endY;
                while (Physics.Raycast(new Vector3(x, decoHeight, z), Vector3.down, out hit, decoHeight - startY, deco))
                {
                    TileNode node = Instantiate(tile, hit.point, Quaternion.identity);
                    node.setType(TileNode.DECO);
                    node.setLength(tileSize);
                    node.connect(tileSize * 1.5f);
                    node.transform.SetParent(transform);
                    decoHeight = node.transform.position.y - tileSize;
                    Debug.Log(node.transform.position);
                }
            }
        }
    }
}
