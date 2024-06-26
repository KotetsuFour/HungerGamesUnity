using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : Interactable
{
    [SerializeField] private GameObject handsFull;
    public bool packable;
    public int handsNeeded;
    public bool strapped;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void menu()
    {
        if (StaticData.player.GetComponent<PlayerController>().takeItem(this))
        {
            gameObject.layer = 0;
        } else
        {
            currentMenu = Instantiate(handsFull);
            StaticData.findDeepChild(currentMenu.transform, "Note")
                .GetComponent<TextMeshProUGUI>().text = "This item requires " + handsNeeded
                 + " hand(s)";
            stillInteracting = true;
        }
    }
    public override void Z()
    {
        Destroy(currentMenu);
        currentMenu = null;
        stillInteracting = false;
    }
}
