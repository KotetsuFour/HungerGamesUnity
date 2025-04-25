using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public int lethality;
    public int meleeAccuracyPenalty;
    public int rangeAccuracyPenalty;
    public int meleeAttackRange;
    public int rangeAttackRange;
    public StaticData.Skill proficiencyType;
    public WeaponSkill proficiencySkill;
    public bool launcher;
    public bool launchable;
    public string description;
    public string[] animations;
    public float[] animationTimes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (currentMenu != null)
        {
            Destroy(currentMenu.gameObject);
            currentMenu = null;
        }
    }

    public enum WeaponSkill
    {
        NONE, HEADSHOT, DISMEMBER, CRUSHING_BLOW, IMPALE
    }
}
