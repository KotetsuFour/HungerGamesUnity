using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public InteractType interactType;

    public bool stillInteracting;
    public GameObject currentMenu;

    public abstract string interactNote(Tribute caller);
    public abstract void menu(Tribute caller);
    public abstract void Z();

    public enum InteractType
    {
        TALK, USE, READ, BED, TAKE, OPEN, WAKE, EXAMINE, EXIT, COACH, TRIBUTE
    }
}
