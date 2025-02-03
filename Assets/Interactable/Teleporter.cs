using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Interactable
{
    [SerializeField] private string objectName;
    [SerializeField] private Transform destination;
    [SerializeField] private GameObject player;

    public override string interactNote(Tribute caller)
    {
        return objectName;
    }

    public override void menu(Tribute caller)
    {
        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation;
    }

    public override void Z()
    {
        //nothing
    }

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
}
