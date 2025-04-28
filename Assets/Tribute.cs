using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tribute : ArenaEntity
{
    public StaticData.TributeData tributeData;
    public Transform rightHand;
    public Transform leftHand;
    public Transform back1;
    public Transform back2;
    protected Weapon equippedWeapon;

    public float currentStamina;
    public static float MAX_STAMINA = 10;

    public bool combatMode;

    public static float MAX_FOOD_FULLNESS = DayNightCycle.timeInADay * 2;
    public float currentFoodFullness;
    public static float MAX_WATER_FULLNESS = DayNightCycle.timeInADay;
    public float currentWaterFullness;
    public static float MAX_SLEEP = DayNightCycle.timeInADay * 2;
    public float currentSleep;

    public RaycastHit gravHit;

    public static float DEFAULT_ATTACK_RANGE = 4;

    public override string getName()
    {
        return $"{tributeData.name} ({tributeData.id})";
    }
    public bool isGrounded()
    {
        float extraHeight = 0.1f;
        float height = entityCollider.bounds.extents.y + extraHeight;
        bool hit = Physics.BoxCast(entityCollider.bounds.center, entityCollider.bounds.extents, Vector3.down,
            out gravHit, Quaternion.Euler(new Vector3(90, 0, 0)), height);
        return hit;
    }

    public Weapon getEquippedWeapon()
    {
        return equippedWeapon;
    }
    public void updateEquippedWeapon()
    {
        Weapon rightHeld = null;
        Weapon leftHeld = null;
        if (rightHand.childCount > 0)
        {
            rightHeld = rightHand.GetChild(0).GetComponent<Weapon>();
        }
        if (leftHand.childCount > 0)
        {
            leftHeld = leftHand.GetChild(0).GetComponent<Weapon>();
        }
        if (rightHeld != null)
        {
            if (rightHeld.launcher)
            {
                equippedWeapon = null;
                return;
            }
            else if (rightHeld.launchable && leftHeld != null && leftHeld.launcher)
            {
                equippedWeapon = leftHeld;
                return;
            }
            else
            {
                equippedWeapon = rightHeld;
                return;
            }
        }
        if (leftHeld != null && leftHeld.launcher && hasLaunchable() != null)
        {
            equippedWeapon = leftHeld;
            return;
        }
        equippedWeapon = null;
    }

    public Item hasLaunchable()
    {
        if (back1.childCount > 0)
        {
            Packaging pack = back1.GetChild(0).GetComponent<Packaging>();
            if (pack != null)
            {
                foreach (Item launchable in pack.contents)
                {
                    if (launchable is Weapon && ((Weapon)launchable).launchable)
                    {
                        return pack;
                    }
                }
            }
        }
        if (back2.childCount > 0)
        {
            Packaging pack = back2.GetChild(0).GetComponent<Packaging>();
            if (pack != null)
            {
                foreach (Item launchable in pack.contents)
                {
                    if (launchable is Weapon && ((Weapon)launchable).launchable)
                    {
                        return pack;
                    }
                }
            }
        }
        return null;
    }
    public float meleeRange()
    {
        Weapon wep = getEquippedWeapon();
        if (wep == null)
        {
            return DEFAULT_ATTACK_RANGE;
        }
        return wep.meleeAttackRange;
    }
    public float distantRange()
    {
        Weapon wep = getEquippedWeapon();
        if (wep == null)
        {
            return 0;
        }
        return wep.rangeAttackRange;
    }
    public override float attackRange()
    {
        return Mathf.Max(meleeRange(), distantRange());
    }
    public override void startAttack(ArenaEntity target, int style)
    {
        Weapon wep = getEquippedWeapon();
        if (wep == null)
        {
            //TODO barehand attacks
        }
        else
        {
            animator.Play(wep.animations[style]);
            attackTime = wep.animationTimes[style];
        }
        if (style == 1)
        {
            target.takeDamage(tributeData.strength + (wep == null ? 0 : wep.lethality), this,
                Weapon.WeaponSkill.NONE);
        }
        else if (style == 2)
        {
            target.takeDamage(tributeData.strength + (wep == null ? 0 : (wep.lethality * 2))
                + ((wep != null && Random.Range(0, StaticData.maxSkillLevel) < tributeData.getSkillLevel(wep.proficiencyType))
                || (wep == null && Random.Range(0, StaticData.maxSkillLevel) < tributeData.getSkillLevel(StaticData.Skill.BRAWLING))
                ? tributeData.strength / 2 : 0),
                this,
                Weapon.WeaponSkill.NONE);
        }
        else if (style == 3)
        {
            target.takeDamage((tributeData.strength * 3 / 2) + (wep.lethality * 2),
                this,
                wep != null && Random.Range(0, StaticData.maxSkillLevel) < tributeData.getSkillLevel(wep.proficiencyType)
                ? wep.proficiencySkill : Weapon.WeaponSkill.NONE);
        }
    }
    public override int getAccuracy(float distance)
    {
        Weapon wep = getEquippedWeapon();
        if (wep == null)
        {
            return tributeData.accuracy;
        }
        int ret = tributeData.accuracy + (tributeData.getSkillLevel(wep.proficiencyType) * 5);
        if (distance <= wep.meleeAttackRange)
        {
            ret -= wep.meleeAccuracyPenalty;
        }
        else if (distance <= wep.rangeAttackRange)
        {
            ret -= wep.rangeAccuracyPenalty;
        }
        else
        {
            return 0;
        }
        return ret;
    }

    public override int getAvoidance(float distance)
    {
        return tributeData.avoidance;
    }
    public override void takeDamage(int damage, object damager, Weapon.WeaponSkill effect)
    {
        tributeData.currentHP -= damage;
        StaticData.registerDamage(this, damage, damager, effect);
        if (effect == Weapon.WeaponSkill.HEADSHOT)
        {
            tributeData.accuracy -= 2;
        }
        else if (effect == Weapon.WeaponSkill.DISMEMBER)
        {
            tributeData.strength -= 2;
        }
        else if (effect == Weapon.WeaponSkill.CRUSHING_BLOW)
        {
            tributeData.avoidance -= 20;
        }
        else if (effect == Weapon.WeaponSkill.IMPALE)
        {
            tributeData.speed -= 2;
        }

        if (!isAlive())
        {
            animator.Play("Death");
        }
    }
    public override int getCurrentHP()
    {
        return tributeData.currentHP;
    }

    public void setData(StaticData.TributeData data)
    {
        tributeData = data;
        data.actualObject = this;
        StaticData.setHairSkinEye(gameObject, data);
    }

    public bool takeItem(Item item)
    {
        int handsNeeded = item.handsNeeded;
        if (handsNeeded <= 1)
        {
            if (rightHandCapacity() >= handsNeeded)
            {
                putInInventoryPosition(rightHand, item.transform);
                return true;
            }
            else if (leftHandCapacity() >= handsNeeded)
            {
                putInInventoryPosition(leftHand, item.transform);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (handsNeeded <= 2)
        {
            if (rightHandCapacity() + leftHandCapacity() >= handsNeeded)
            {
                putInInventoryPosition(rightHand, item.transform);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    private float rightHandCapacity()
    {
        float capacity = 1;
        for (int q = 0; q < rightHand.transform.childCount; q++)
        {
            capacity -= rightHand.GetChild(q).GetComponent<Item>().handsNeeded;
        }
        return capacity;
    }
    private float leftHandCapacity()
    {
        float capacity = 1;
        for (int q = 0; q < leftHand.transform.childCount; q++)
        {
            capacity -= leftHand.transform.GetChild(q).GetComponent<Item>().handsNeeded;
        }
        return capacity;
    }

    private void putInInventoryPosition(Transform destination, Transform item)
    {
        item.transform.SetParent(destination);
        item.transform.localPosition = destination.localPosition;
        Vector3 euler = destination.eulerAngles;
        item.transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z);
        updateEquippedWeapon();
    }

    public void drop()
    {
        Item rightHeld = null;
        Item leftHeld = null;
        if (rightHand.childCount > 0)
        {
            rightHeld = rightHand.GetChild(0).GetComponent<Item>();
        }
        if (leftHand.childCount > 0)
        {
            leftHeld = leftHand.GetChild(0).GetComponent<Item>();
        }
        if (leftHeld != null && (rightHeld == null || leftHeld != getEquippedWeapon()))
        {
            removeFromHand(leftHeld);
        }
        else if (rightHeld != null)
        {
            removeFromHand(rightHeld);
        }
        updateEquippedWeapon();
    }
    private void removeFromHand(Item item)
    {
        item.transform.SetParent(null);
        item.transform.position = transform.position;
        item.transform.rotation = Quaternion.identity;
        item.gameObject.layer = 6;
        updateEquippedWeapon();
    }


}
