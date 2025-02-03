using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSelector : MonoBehaviour
{
    public void setHairStyle(int num)
    {
        for (int q = 0; q < transform.childCount; q++)
        {
            transform.GetChild(q).gameObject.SetActive(false);
        }
        transform.GetChild(num).gameObject.SetActive(true);
    }

    public void setHairColor(Color color)
    {
        //0 is bald, so we start with 1
        for (int q = 1; q < transform.childCount; q++)
        {
            Material[] mats = transform.GetChild(q).GetComponent<MeshRenderer>().materials;
            foreach (Material mat in mats)
            {
                mat.color = color;
            }
        }
    }
}
