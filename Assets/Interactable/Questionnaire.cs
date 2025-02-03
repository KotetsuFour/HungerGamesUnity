using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Questionnaire : Interactable
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject speakBox;

    [SerializeField] private List<string> sayings;
    [SerializeField] private List<string> speakers;
    private int sayingIdx;

    private string answerer;
    private string start;
    private string[] options;
    private int[][] buffs;
    private string end;

    private int answerChoice;

    private SelectionMode selectionMode;

    public override string interactNote(Tribute caller)
    {
        //TODO
        return null;
    }
    public override void menu(Tribute caller)
    {
        stillInteracting = true;

        currentMenu = Instantiate(dialogueBox);
        sayingIdx = 0;
        nextSaying();

        selectionMode = SelectionMode.DIALOGUE;
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
        if (selectionMode == SelectionMode.DIALOGUE)
        {
            sayingIdx++;
            if (sayingIdx < sayings.Count)
            {
                nextSaying();
            }
            else
            {
                Destroy(currentMenu);
                currentMenu = Instantiate(speakBox);
                StaticData.findDeepChild(currentMenu.transform, "Speaker").GetComponent<TextMeshProUGUI>()
                    .text = answerer;
                StaticData.findDeepChild(currentMenu.transform, "Beginning").GetComponent<TextMeshProUGUI>()
                    .text = start;
                TMP_Dropdown opts = StaticData.findDeepChild(currentMenu.transform, "Choices").GetComponent<TMP_Dropdown>(); ;
                opts.options.Clear();
                TMP_Dropdown.OptionData blank = new TMP_Dropdown.OptionData();
                blank.text = "___________";
                opts.options.Add(blank);
                foreach (string s in options)
                {
                    TMP_Dropdown.OptionData choice = new TMP_Dropdown.OptionData();
                    choice.text = s;
                    opts.options.Add(choice);
                }
                TMP_Dropdown.DropdownEvent answerEvent = new TMP_Dropdown.DropdownEvent();
                answerEvent.AddListener(setAnswerChoice);
                opts.onValueChanged = answerEvent;
                StaticData.findDeepChild(currentMenu.transform, "Ending").GetComponent<TextMeshProUGUI>()
                    .text = end;
                Button.ButtonClickedEvent selection = new Button.ButtonClickedEvent();
                selection.AddListener(selectOption);
                StaticData.findDeepChild(currentMenu.transform, "Confirm").GetComponent<Button>()
                    .onClick = selection;

            }
        } else if (selectionMode == SelectionMode.BUFFS)
        {
            sayingIdx++;
            if (sayingIdx < sayings.Count)
            {
                nextSaying();
            } else
            {
                Destroy(currentMenu);
                currentMenu = null;
                stillInteracting = false;
            }
        }
    }
    public void setAnswerChoice(int answerChoice)
    {
        this.answerChoice = answerChoice;
    }
    public void selectOption()
    {
        if (answerChoice != 0)
        {
            int choiceIdx = answerChoice - 1;
            StaticData.playerData.strength += buffs[choiceIdx][0];
            StaticData.playerData.accuracy += buffs[choiceIdx][1];
            StaticData.playerData.avoidance += buffs[choiceIdx][2];
            StaticData.playerData.speed += buffs[choiceIdx][3];
            StaticData.playerData.intelligence += buffs[choiceIdx][4];
            StaticData.playerData.likability += buffs[choiceIdx][5];
            string[] stats = { "Strength", "Accuracy", "Avoidance",
                "Speed", "Intelligence", "Likability" };

            Destroy(currentMenu);
            currentMenu = Instantiate(dialogueBox);
            sayings.Clear();
            for (int q = 0; q < stats.Length; q++)
            {
                if (buffs[choiceIdx][q] > 0)
                {
                    sayings.Add(stats[q] + " + " + buffs[choiceIdx][q]);
                } else if (buffs[choiceIdx][q] < 0)
                {
                    sayings.Add(stats[q] + " - " + Mathf.Abs(buffs[choiceIdx][q]));
                }
            }
            speakers.Clear();
            speakers.Add("");

            selectionMode = SelectionMode.BUFFS;
            sayingIdx = 0;
            nextSaying();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setDialogue(List<string> sayings, List<string> speakers,
        GameObject dialogueBox)
    {
        this.sayings = sayings;
        this.speakers = speakers;
        this.dialogueBox = dialogueBox;
    }
    public void setQuestion(string answerer, string start, string[] options, int[][] buffs,
        string end, GameObject speakBox)
    {
        this.answerer = answerer;
        this.start = start;
        this.options = options;
        this.buffs = buffs;
        this.end = end;
        this.speakBox = speakBox;
    }

    private enum SelectionMode
    {
        DIALOGUE, CHOICE, BUFFS
    }

}
