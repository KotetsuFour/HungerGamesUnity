using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTile : MonoBehaviour
{
    public Material environment;
    public int water; //0 is not water, 1 is potable water, 2 is non-potable water

    public void setWater(int waterVal)
    {
        water = waterVal;
        if (water > 0)
        {
            gameObject.layer = 4;
        }
    }
}
