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

    public bool canAccept(Item item)
    {
        return contents.Count < capacity && item.packable;
    }

    public void pack(Item item)
    {
        contents.Add(item);
    }

    public Item unpack(int idx)
    {
        Item ret = contents[idx];
        contents.RemoveAt(idx);
        return ret;
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
}
