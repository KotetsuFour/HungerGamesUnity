using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Talker : Interactable
{
    [SerializeField] private string speakerName;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private List<string> sayings;
    [SerializeField] private List<string> speakers;
    private int sayingIdx;

    public override string interactNote(Tribute caller)
    {
        if (GetComponent<Tribute>() != null)
        {
            return GetComponent<Tribute>().tributeData.name;
        }
        return speakerName;
    }
    public override void menu(Tribute caller)
    {
        stillInteracting = true;

        currentMenu = Instantiate(dialogueBox);
        sayingIdx = 0;
        nextSaying();
    }

    private void nextSaying()
    {
        currentMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
            = speakers[sayingIdx % speakers.Count];
        currentMenu.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text
            = sayings[sayingIdx];
    }

    public override void Z()
    {
        sayingIdx++;
        if (sayingIdx < sayings.Count)
        {
            nextSaying();
        }
        else
        {
            Destroy(currentMenu);
            currentMenu = null;
            stillInteracting = false;
        }
    }

    public void setDialogue(List<string> sayings, List<string> speakers,
        GameObject dialogueBox)
    {
        this.sayings = sayings;
        this.speakers = speakers;
        this.dialogueBox = dialogueBox;
    }
}
