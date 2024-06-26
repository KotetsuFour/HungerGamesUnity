using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alliance
{
    public List<StaticData.TributeData> members;
    public Vector3 destination;

    public int waterSupply;
    public bool needWater;
    public int foodSupply;
    public bool needFood;
    public bool needSleep;

    public float wakingTime;

    public Alliance()
    {
        members = new List<StaticData.TributeData>();
    }

    public void join(StaticData.TributeData data)
    {
        foreach (StaticData.TributeData ally in data.alliance.members)
        {
            members.Add(ally);
            ally.alliance = this;
        }
    }
}
