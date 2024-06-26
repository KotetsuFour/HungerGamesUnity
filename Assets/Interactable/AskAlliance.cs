using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AskAlliance : Interactable
{
    [SerializeField] private GameObject picker;
    private List<StaticData.TributeData> askers;
    private int askerIdx;
    private float timer;
    public override void menu()
    {
        stillInteracting = true;
        askerIdx = 0;
        if (askerIdx < askers.Count)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            prompt();
        } else
        {
            stillInteracting = false;
        }
    }
    public override void Z()
    {
        //nothing
    }
    public void setAskers(List<StaticData.TributeData> askers)
    {
        this.askers = askers;
    }
    public void prompt()
    {
        StaticData.TributeData data = askers[askerIdx];
        currentMenu = Instantiate(picker);
        StaticData.findDeepChild(currentMenu.transform, "Canvas").GetComponent<CanvasGroup>()
            .alpha = 0;
        timer = 1;
        int allies = data.alliance.members.Count - 1;
        string text = data.name + " ";
        if (allies > 0)
        {
            text += "and " + StaticData.theirPronoun[(int)data.gender].ToLower() + " " + allies + " ";
            if (allies == 1)
            {
                text += " ally want ";
            }
            else
            {
                text += " allies want ";
            }
        }
        else
        {
            text += " wants ";
        }
        text += "to join you.";
        StaticData.findDeepChild(currentMenu.transform, "Buff").GetComponent<TextMeshProUGUI>()
            .text = text;
        StaticData.findDeepChild(currentMenu.transform, "SelectText").GetComponent<TextMeshProUGUI>()
            .text = "Join";
        StaticData.findDeepChild(currentMenu.transform, "LeaveText").GetComponent<TextMeshProUGUI>()
            .text = "Decline";
        Button.ButtonClickedEvent pickJoin = new Button.ButtonClickedEvent();
        pickJoin.AddListener(join);
        StaticData.findDeepChild(currentMenu.transform, "Choose").GetComponent<Button>()
            .onClick = pickJoin;
        Button.ButtonClickedEvent pickDeny = new Button.ButtonClickedEvent();
        pickDeny.AddListener(nextAsker);
        StaticData.findDeepChild(currentMenu.transform, "MoveOn").GetComponent<Button>()
            .onClick = pickDeny;
    }
    public void join()
    {
        StaticData.playerData.alliance.join(askers[askerIdx]);
        nextAsker();
    }
    public void nextAsker()
    {
        Destroy(currentMenu);
        askerIdx++;
        if (askerIdx < askers.Count)
        {
            while (askerIdx < askers.Count && 
                (askers[askerIdx].alliance.members.Contains(StaticData.playerData)
                || askers[askerIdx].alliance.members.Count + StaticData.playerData.alliance.members.Count > StaticData.numTributes / 4))
            {
                askerIdx++;
            }
            if (askerIdx < askers.Count)
            {
                prompt();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                stillInteracting = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            stillInteracting = false;

            if (StaticData.trainingSequenceNum <= 0)
            {
                GetComponent<TrainingSetup>().printAlliances();
                GetComponent<TrainingSetup>().setupPresentSkills();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (currentMenu != null)
        {
            StaticData.findDeepChild(currentMenu.transform, "Canvas").GetComponent<CanvasGroup>()
                .alpha = Mathf.Max(0, 1 - timer);
        }
    }
}
