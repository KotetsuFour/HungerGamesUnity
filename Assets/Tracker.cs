using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private Transform target;
    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
        }
    }
    public void setTarget(Transform target)
    {
        this.target = target;
    }
}
