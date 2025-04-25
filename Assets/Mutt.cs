using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutt : ArenaEntity
{
    [SerializeField] private float attackRadius;
    [SerializeField] private int accuracy;
    [SerializeField] private int avoidance;
    [SerializeField] private int strength;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float[] attackAnimationTimes;
    [SerializeField] private string[] attackAnimations;

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
    public override void startAttack(ArenaEntity target, int style)
    {
        animator.Play(attackAnimations[style]);
        attackTime = attackAnimationTimes[style];
    }
    public override void takeDamage(int damage, Weapon.WeaponSkill effect)
    {
        //TODO
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
