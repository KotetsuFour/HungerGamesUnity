using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : Interactable
{
    private BetterPlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<BetterPlayerController>();
    }

    public override string interactNote(Tribute caller)
    {
        return null;
    }
    public override void menu(Tribute caller)
    {
        //TODO
        stillInteracting = false;
    }
    public override void Z()
    {
        //TODO
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
