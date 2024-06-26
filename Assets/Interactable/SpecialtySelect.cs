using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SpecialtySelect : Interactable
{
    private GameObject dialogueBox;
    private GameObject speakBox;
    private GameObject fadeScreen;
    private List<string> sayings1;
    private List<string> speakers1;
    private List<string> sayings2;
    private List<string> speakers2;
    private int sayingIdx;
    private SelectionMode selectionMode;

    private StaticData.Skill[] usingSkills;
    private float initialTime;
    private float timer;

    public override void menu()
    {
        stillInteracting = true;

        currentMenu = Instantiate(dialogueBox);
        setDialogue();
        sayingIdx = 0;
        nextSaying();
    }

    private void nextSaying()
    {
        if (selectionMode == SelectionMode.SPEAK1)
        {
            currentMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
                = speakers1[sayingIdx % speakers1.Count];
            currentMenu.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text
                = sayings1[sayingIdx];
        } else if (selectionMode == SelectionMode.SPEAK2)
        {
            currentMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
                = speakers2[sayingIdx % speakers2.Count];
            currentMenu.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text
                = sayings2[sayingIdx];
        }
    }

    public override void Z()
    {
        sayingIdx++;
        if (selectionMode == SelectionMode.SPEAK1)
        {
            if (sayingIdx < sayings1.Count)
            {
                nextSaying();
            }
            else
            {
                Destroy(currentMenu);
                currentMenu = Instantiate(speakBox);
                TMP_Dropdown drop = StaticData.findDeepChild(currentMenu.transform, "Choices")
                    .GetComponent<TMP_Dropdown>();
                if (StaticData.chooseSpecialties)
                {
                    usingSkills = (StaticData.Skill[])Enum.GetValues(typeof(StaticData.Skill));
                }
                else if (StaticData.reapStatus == StaticData.Reap_Status.REAPED)
                {
                    usingSkills = StaticData.reapedSkills;
                } else if (StaticData.reapStatus == StaticData.Reap_Status.VOLUNTEERED)
                {
                    usingSkills = StaticData.volunteeredSkills;
                }
                drop.options.Clear();
                TMP_Dropdown.OptionData blank = new TMP_Dropdown.OptionData();
                blank.text = "__________";
                drop.options.Add(blank);
                for (int q = 0; q < usingSkills.Length; q++) {
                    TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
                    opt.text = usingSkills[q] + "";
                    drop.options.Add(opt);
                }
                Button.ButtonClickedEvent confirm = new Button.ButtonClickedEvent();
                confirm.AddListener(setSpecialty);
                StaticData.findDeepChild(currentMenu.transform, "Confirm")
                    .GetComponent<Button>().onClick = confirm;

                StaticData.findDeepChild(currentMenu.transform, "Speaker")
                    .GetComponent<TextMeshProUGUI>().text = StaticData.playerName;
                StaticData.findDeepChild(currentMenu.transform, "Beginning")
                    .GetComponent<TextMeshProUGUI>().text = "I'm good with";
                StaticData.findDeepChild(currentMenu.transform, "Ending")
                    .GetComponent<TextMeshProUGUI>().text = "";

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                selectionMode = SelectionMode.CHOOSE;
            }
        } else if (selectionMode == SelectionMode.SPEAK2)
        {
            if (sayingIdx < sayings1.Count)
            {
                nextSaying();
            }
            else
            {
                Destroy(currentMenu);
                currentMenu = Instantiate(fadeScreen);
                StaticData.findDeepChild(currentMenu.transform, "Screen").GetComponent<Image>()
                    .color = new Color(0, 0, 0, 0);
                setTimer(2);
                selectionMode = SelectionMode.FADEOUT;
            }
        }
    }

    private void Update()
    {
        if (selectionMode == SelectionMode.FADEOUT)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                StaticData.findDeepChild(currentMenu.transform, "Screen").GetComponent<Image>()
                    .color = new Color(0, 0, 0, (initialTime - timer) / initialTime);
            }
            else
            {
                SceneManager.LoadScene("Parade");
            }
        }
    }
    public void setNecessities(GameObject dialogueBox, GameObject speakBox, GameObject fadeScreen)
    {
        this.dialogueBox = dialogueBox;
        this.speakBox = speakBox;
        this.fadeScreen = fadeScreen;
    }

    private void setDialogue()
    {
        sayings1 = new List<string>(new string[] { "There you are. My name's "
            + StaticData.mentor.name + ". I'll be your mentor for the Games. It's my job to " +
            "teach you how to survive.",
        "First thing's first. I need to know what I'm working with. " +
        "So tell me, what's your specialty?"
        });
        speakers1 = new List<string>(new string[] { StaticData.mentor.name });
    }
    public void setSpecialty()
    {
        TMP_Dropdown drop = StaticData.findDeepChild(currentMenu.transform, "Choices")
            .GetComponent<TMP_Dropdown>();
        if (drop.value > 0)
        {
            if (StaticData.reapStatus == StaticData.Reap_Status.REAPED || StaticData.chooseSpecialties)
            {
                StaticData.specialty = (StaticData.Skill)drop.value - 1;
            } else if (StaticData.reapStatus == StaticData.Reap_Status.VOLUNTEERED)
            {
                StaticData.specialty = (StaticData.Skill)(drop.value
                    + (int)StaticData.Skill.SWORDS) - 1;
            }
            StaticData.playerData.giveSpecialty(StaticData.specialty);

            StaticData.finalizeTributeBaseStats();

            sayings2 = new List<string>(new string[] { StaticData.specialty + ". Hmm. I can work with that.",
            "We'll talk more later. The train is about to arrive at the Capitol."
            });
            speakers2 = new List<string>(new string[] { StaticData.mentor.name });

            Destroy(currentMenu);
            currentMenu = Instantiate(dialogueBox);
            sayingIdx = 0;
            selectionMode = SelectionMode.SPEAK2;
            nextSaying();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void setTimer(float time)
    {
        initialTime = time;
        timer = initialTime;
    }
    private enum SelectionMode
    {
        SPEAK1, CHOOSE, SPEAK2, FADEOUT
    }
}
