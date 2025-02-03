using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TributeInteractable : Interactable
{
    private NPCTribute tribute;

    // Start is called before the first frame update
    void Start()
    {
        tribute = GetComponent<NPCTribute>();
    }

    public override string interactNote(Tribute caller)
    {
        return tribute.tributeData.name;
    }

    public override void menu(Tribute caller)
    {
        //TODO
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
