using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Coach : Interactable
{
    [SerializeField] private GameObject select;
    [SerializeField] private GameObject fadeScreen;
    public StaticData.Skill taughtSkill;
    public int lineZDirection;
    public List<GameObject> students;
    public TrainingSetup setup;
    public float initialTime;
    public float timer;
    private SelectionMode selectionMode;
    public override void menu()
    {
        stillInteracting = true;
        currentMenu = Instantiate(select);
        Button.ButtonClickedEvent pickMeOpt = new Button.ButtonClickedEvent();
        pickMeOpt.AddListener(pickMe);
        StaticData.findDeepChild(currentMenu.transform, "Choose").GetComponent<Button>()
            .onClick = pickMeOpt;
        Button.ButtonClickedEvent leaveOpt = new Button.ButtonClickedEvent();
        leaveOpt.AddListener(leave);
        StaticData.findDeepChild(currentMenu.transform, "MoveOn").GetComponent<Button>()
            .onClick = leaveOpt;
        StaticData.findDeepChild(currentMenu.transform, "Buff").GetComponent<TextMeshProUGUI>()
            .text = "Train " + taughtSkill + "?\nCurrently "
            + StaticData.playerData.getSkillLevel(taughtSkill) + "/" + StaticData.maxSkillLevel;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        selectionMode = SelectionMode.CHOOSE;
    }

    public override void Z()
    {
        //nothing
    }

    public void setEssentials(GameObject select, GameObject fadeScreen, StaticData.TributeData data)
    {
        this.select = select;
        this.fadeScreen = fadeScreen;
    }
    public void pickMe()
    {
        StaticData.playerData.increaseSkill(taughtSkill, 1);
        Destroy(currentMenu);
        currentMenu = Instantiate(fadeScreen);
        StaticData.findDeepChild(currentMenu.transform, "Screen").GetComponent<CanvasGroup>()
            .alpha = 0;
        StaticData.findDeepChild(currentMenu.transform, "Note").GetComponent<TextMeshProUGUI>()
            .text = taughtSkill + " is now " + StaticData.playerData.getSkillLevel(taughtSkill)
            + "/" + StaticData.maxSkillLevel;
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
    public void addStudent(GameObject tribute)
    {
        tribute.transform.position = transform.position;
        tribute.transform.Translate(0, 0, (3 + (1.5f * students.Count)) * lineZDirection);
        tribute.transform.rotation = Quaternion.LookRotation(transform.position - tribute.transform.position);
        students.Add(tribute);
    }
    private void setTimer(float time)
    {
        initialTime = time;
        timer = initialTime;
    }
    public void contruct()
    {
        students = new List<GameObject>();
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
        } else if (selectionMode == SelectionMode.BLACKSCREEN)
        {
            if (timer <= 0)
            {
                Destroy(currentMenu);
                currentMenu = null;
                stillInteracting = false;
                setup.traineesAct(students);

                selectionMode = SelectionMode.CHOOSE;
            }
        }
    }
    private enum SelectionMode
    {
        CHOOSE, FADEOUT, BLACKSCREEN
    }
}
