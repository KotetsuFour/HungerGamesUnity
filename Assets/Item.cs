using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : Interactable
{
    [SerializeField] private string itemName;
    [SerializeField] private GameObject handsFull;
    public bool packable;
    public int handsNeeded;
    public bool strapped;

    public float timer;

    public string getName()
    {
        return itemName;
    }
    public override string interactNote(Tribute caller)
    {
        return $"Pick Up {getName()}";
    }

    public override void menu(Tribute caller)
    {
        if (caller.takeItem(this))
        {
            gameObject.layer = 0;
        }
        else
        {
            timer = 2;
            currentMenu = Instantiate(handsFull);
            StaticData.findDeepChild(currentMenu.transform, "Note")
                .GetComponent<TextMeshProUGUI>().text = "This item requires " + handsNeeded
                 + " free hand(s)";
        }
    }
    public override void Z()
    {
        //TODO
    }
}
