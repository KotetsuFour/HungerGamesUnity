using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutt : ArenaEntity
{
    [SerializeField] private float attackRadius;
    [SerializeField] private int accuracy;
    [SerializeField] private int avoidance;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override float attackRange()
    {
        return attackRadius;
    }

    public override int getAccuracy(float distance)
    {
        return accuracy;
    }

    public override int getAvoidance(float distance)
    {
        return avoidance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
