using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] private float distanceInFrontOfPoint;
    [SerializeField] private float distanceAbovePoint;
    [SerializeField] private bool lookAtPoint;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float minZ;
    private float maxZ;
    private bool quickConstructedAlready;
    // Start is called before the first frame update
    void Start()
    {
        if (!quickConstructedAlready)
        {
            quickConstruct();
        }
    }
    public void quickConstruct()
    {
        quickConstructedAlready = true;
        minX = float.MinValue;
        maxX = float.MaxValue;
        minY = float.MinValue;
        maxY = float.MaxValue;
        minZ = float.MinValue;
        maxZ = float.MaxValue;
    }
    public void updatePosition(Transform point)
    {
        Vector3 expectedPos = point.forward * distanceInFrontOfPoint;
        expectedPos += (point.up * distanceAbovePoint);
        expectedPos += point.position;
        Vector3 actualPos = new Vector3(Mathf.Clamp(expectedPos.x, minX, maxX),
            Mathf.Clamp(expectedPos.y, minY, maxY), Mathf.Clamp(expectedPos.z, minZ, maxZ));
        transform.position = actualPos;
        transform.rotation = point.rotation;
        if (distanceInFrontOfPoint > 0)
        {
            transform.Rotate(new Vector3(0, 180, 0));
        }
        /*
        if (lookAtPoint)
        {
            transform.rotation = Quaternion.LookRotation(point.position - transform.position);
        }
        */
    }
    public void setBoundaries(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        this.minZ = minZ;
        this.maxZ = maxZ;
    }
}
