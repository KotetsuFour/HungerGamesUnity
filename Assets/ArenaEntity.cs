using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArenaEntity : MonoBehaviour
{
    public GameObject model;

    public Animator animator;
    public Collider entityCollider;
    public Rigidbody rb;
    protected float attackTime;

    public abstract string getName();
    public abstract int getCurrentHP();
    public abstract float attackRange();

    public abstract int getAccuracy(float distance);
    public abstract int getAvoidance(float distance);
    /**
     * 0 for miss
     * 1 for hit
     * 2 for crit
     * 3 for lethal
     */
    public abstract void startAttack(ArenaEntity target, int style);
    public abstract void takeDamage(int damage, object damager, Weapon.WeaponSkill effect);
    public bool isAlive()
    {
        return getCurrentHP() > 0;
    }
}
