using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packaging : Item
{
    public List<Item> contents;
    public int capacity;
    public string restrictedTo;
    // Start is called before the first frame update
    void Start()
    {
        contents = new List<Item>();
    }

}
