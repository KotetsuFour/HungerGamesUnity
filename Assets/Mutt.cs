using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutt : ArenaEntity
{
    [SerializeField] private int hp;
    [SerializeField] private float attackRadius;
    [SerializeField] private int accuracy;
    [SerializeField] private int avoidance;
    [SerializeField] private int strength;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float[] attackAnimationTimes;
    [SerializeField] private string[] attackAnimations;
    private float despawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override string getName()
    {
        return name;
    }
    public override int getCurrentHP()
    {
        return hp;
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
        target.takeDamage(strength, this, Weapon.WeaponSkill.NONE);
    }
    public override void takeDamage(int damage, object damager, Weapon.WeaponSkill effect)
    {
        hp -= damage;
        StaticData.registerDamage(this, damage, damager, effect);
        if (effect == Weapon.WeaponSkill.HEADSHOT)
        {
            accuracy -= 2;
        }
        else if (effect == Weapon.WeaponSkill.DISMEMBER)
        {
            strength -= 2;
        }
        else if (effect == Weapon.WeaponSkill.CRUSHING_BLOW)
        {
            avoidance -= 20;
        }
        else if (effect == Weapon.WeaponSkill.IMPALE)
        {
            moveSpeed -= 2;
        }


        if (!isAlive())
        {
            animator.Play("Death");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (despawnTimer > 0)
        {
            despawnTimer -= Time.deltaTime;
            if (despawnTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
