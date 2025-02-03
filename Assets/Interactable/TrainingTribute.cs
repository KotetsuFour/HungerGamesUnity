using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrainingTribute : Interactable
{
    [SerializeField] private GameObject select;
    [SerializeField] private GameObject fadeScreen;
    public StaticData.TributeData data;
    public TrainingSetup setup;
    public float initialTime;
    public float timer;
    private SelectionMode selectionMode;

    public override string interactNote(Tribute caller)
    {
        return GetComponent<Tribute>().tributeData.name;
    }

    public override void menu(Tribute caller)
    {
        stillInteracting = true;
        currentMenu = Instantiate(select);

        if (data.alliance.members.Contains(StaticData.playerData))
        {
            setupOkay(data.name + " is already an ally.");
        } else if (data.alliance.members.Count + StaticData.playerData.alliance.members.Count
            > StaticData.numTributes / 4)
        {
            setupOkay(data.name + "'s alliance of " + data.alliance.members.Count + " is too " +
                "big to join your alliance of " + StaticData.playerData.alliance.members.Count + ".");
        } else
        {
            Button.ButtonClickedEvent pickMeOpt = new Button.ButtonClickedEvent();
            pickMeOpt.AddListener(pickMe);
            StaticData.findDeepChild(currentMenu.transform, "Choose").GetComponent<Button>()
                .onClick = pickMeOpt;
            Button.ButtonClickedEvent leaveOpt = new Button.ButtonClickedEvent();
            leaveOpt.AddListener(leave);
            StaticData.findDeepChild(currentMenu.transform, "MoveOn").GetComponent<Button>()
                .onClick = leaveOpt;
            StaticData.findDeepChild(currentMenu.transform, "Buff").GetComponent<TextMeshProUGUI>()
                .text = "Try to ally with " + data.name
                + (data.alliance.members.Count > 1 ? (" and " + (data.alliance.members.Count - 1) + " others")
                : "") + "?";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        selectionMode = SelectionMode.CHOOSE;
    }
    private void setupOkay(string msg)
    {
        StaticData.findDeepChild(currentMenu.transform, "Choose").gameObject
            .SetActive(false);
        Button.ButtonClickedEvent leaveOpt = new Button.ButtonClickedEvent();
        leaveOpt.AddListener(leave);
        StaticData.findDeepChild(currentMenu.transform, "MoveOn").GetComponent<Button>()
            .onClick = leaveOpt;
        StaticData.findDeepChild(currentMenu.transform, "Buff").GetComponent<TextMeshProUGUI>()
            .text = msg;
    }

    public override void Z()
    {
        //nothing
    }

    public void setEssentials(GameObject select, GameObject fadeScreen, StaticData.TributeData data)
    {
        this.select = select;
        this.fadeScreen = fadeScreen;
        this.data = data;
    }

    public void pickMe()
    {

        int wannaJoin = StaticData.wantJoinValue(data, StaticData.playerData);
        string joinStatus = "";
        if (wannaJoin > Random.Range(0, StaticData.allianceRequirement))
        {
            StaticData.playerData.alliance.join(data);
            joinStatus = data.name + " joined your party.";
        } else
        {
            joinStatus = data.name + " did not want to join you.";
        }

        Destroy(currentMenu);
        currentMenu = Instantiate(fadeScreen);
        StaticData.findDeepChild(currentMenu.transform, "Screen").GetComponent<CanvasGroup>()
            .alpha = 0;
        StaticData.findDeepChild(currentMenu.transform, "Note").GetComponent<TextMeshProUGUI>()
            .text = joinStatus;
        setTimer(1);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        selectionMode = SelectionMode.FADEOUT;
    }
    public void leave()
    {
        Destroy(currentMenu);
        currentMenu = null;
        stillInteracting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void setTimer(float time)
    {
        initialTime = time;
        timer = initialTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (selectionMode == SelectionMode.FADEOUT)
        {
            StaticData.findDeepChild(currentMenu.transform, "Screen").GetComponent<CanvasGroup>()
                .alpha = (initialTime - timer) / initialTime;
            if (timer <= 0)
            {
                setTimer(2);
                selectionMode = SelectionMode.BLACKSCREEN;
            }
        }
        else if (selectionMode == SelectionMode.BLACKSCREEN)
        {
            if (timer <= 0)
            {
                Destroy(currentMenu);
                currentMenu = null;
                stillInteracting = false;
                setup.traineesAct(null);

                selectionMode = SelectionMode.CHOOSE;
            }
        }
    }
    private enum SelectionMode
    {
        CHOOSE, FADEOUT, BLACKSCREEN
    }

}
