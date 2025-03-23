using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArenaEntity : MonoBehaviour
{
    public GameObject model;

    public Animator animator;
    public Collider entityCollider;
    public Rigidbody rb;

    public abstract float attackRange();

    public abstract int getAccuracy(float distance);
    public abstract int getAvoidance(float distance);
}
