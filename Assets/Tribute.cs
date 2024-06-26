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

    public float currentStamina;
    public static float MAX_STAMINA = 10;

    public static float MAX_FOOD_FULLNESS = DayNightCycle.timeInADay * 2;
    public float currentFoodFullness;
    public static float MAX_WATER_FULLNESS = DayNightCycle.timeInADay;
    public float currentWaterFullness;
    public static float MAX_SLEEP = DayNightCycle.timeInADay * 2;
    public float currentSleep;

    public static float DEFAULT_ATTACK_RANGE = 4;
    public bool isGrounded()
    {
        float extraHeight = 0.1f;
        float height = entityCollider.bounds.extents.y + extraHeight;
        bool hit = Physics.BoxCast(entityCollider.bounds.center, entityCollider.bounds.extents, Vector3.down,
            Quaternion.Euler(new Vector3(90, 0, 0)), height);
        return hit;
    }

    //Messy, but I'm pretty sure it should work
    public Weapon getEquippedWeapon()
    {
        if (rightHand.childCount > 0)
        {
            Weapon rightHeld = rightHand.GetChild(0).GetComponent<Weapon>();
            if (rightHeld == null)
            {
                if (leftHand.childCount > 0)
                {
                    Weapon leftHeld = leftHand.GetChild(0).GetComponent<Weapon>();
                    if (leftHeld == null)
                    {
                        return null;
                    } else
                    {
                        if (leftHeld.launcher && hasLaunchable() != null)
                        {
                            return leftHeld;
                        } else
                        {
                            return null;
                        }
                    }
                }
            }
            else
            {
                if (rightHeld.launcher)
                {
                    return null;
                } else if (rightHeld.launchable)
                {
                    if (leftHand.childCount > 0)
                    {
                        Weapon leftHeld = leftHand.GetChild(0).GetComponent<Weapon>();
                        if (leftHeld == null)
                        {
                            return null;
                        }
                        else
                        {
                            if (leftHeld.launcher)
                            {
                                return leftHeld;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                else
                {
                    return rightHeld;
                }
            }
        }
        return null;
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
    public float attackRange()
    {
        return Mathf.Max(meleeRange(), distantRange());
    }

}
