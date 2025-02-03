using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StyleChoice : Interactable
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject pickOutfit;
    [SerializeField] private GameObject parade;
    [SerializeField] private List<string> sayings;
    [SerializeField] private List<string> speakers;
    private int sayingIdx;

    private SelectionMode selectionMode;

    private List<Color>[] colorSchemes;
    private int modelNum;
    private string buffStatement;
    private List<int> outfitChoices;

    private float initialTime;
    private float timer;

    public AudioSource music;
    private float timeFade;
    private float timeLinger;
    private int tributeIdx;

    public GameObject olderMale;
    public GameObject olderFemale;
    public GameObject youngerMale;
    public GameObject youngerFemale;
    public Transform chariotPosition;
    public Transform snowPosition;
    private GameObject currentTribute;
    public List<GameObject> props;

    public override string interactNote(Tribute caller)
    {
        return "Stylist";
    }
    public override void menu(Tribute caller)
    {
        stillInteracting = true;

        currentMenu = Instantiate(dialogueBox);
        sayingIdx = 0;
        selectionMode = SelectionMode.DESCRIPTION;
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
        else if (selectionMode == SelectionMode.DESCRIPTION)
        {
            Destroy(currentMenu);
            currentMenu = Instantiate(pickOutfit);
            Button.ButtonClickedEvent pickIt = new Button.ButtonClickedEvent();
            pickIt.AddListener(select);
            StaticData.findDeepChild(currentMenu.transform, "Choose").GetComponent<Button>()
                .onClick = pickIt;
            Button.ButtonClickedEvent leave = new Button.ButtonClickedEvent();
            leave.AddListener(moveOn);
            StaticData.findDeepChild(currentMenu.transform, "MoveOn").GetComponent<Button>()
                .onClick = leave;
            StaticData.findDeepChild(currentMenu.transform, "Buff").GetComponent<TextMeshProUGUI>()
                .text = buffStatement;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            selectionMode = SelectionMode.PICK;
        } else if (selectionMode == SelectionMode.SELECTION_MADE)
        {
            Destroy(currentMenu);
            currentMenu = Instantiate(parade);
            Button.ButtonClickedEvent skip = new Button.ButtonClickedEvent();
            skip.AddListener(endParade);
            StaticData.findDeepChild(currentMenu.transform, "Skip").GetComponent<Button>()
                .onClick = skip;
            setTimer(3);

            selectionMode = SelectionMode.FADEOUT;
        } else if (selectionMode == SelectionMode.PAUSE_SNOW)
        {
            endParade();
        }
    }

    public void setDialogue(List<string> sayings, List<string> speakers,
        GameObject dialogueBox, GameObject pickOutfit, GameObject parade)
    {
        this.sayings = sayings;
        this.speakers = speakers;
        this.dialogueBox = dialogueBox;
        this.pickOutfit = pickOutfit;
        this.parade = parade;
    }
    public void setColorSchemes(List<Color>[] colorSchemes)
    {
        this.colorSchemes = colorSchemes;
    }
    public void setupModel1()
    {
        modelNum = 1;
        buffStatement = "Strength + 1";
        Material[] materials = StaticData.findDeepChild(transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, Color.white, Color.white, Color.white);
        outfit1(materials, StaticData.district);
    }
    public void setupModel2()
    {
        modelNum = 2;
        buffStatement = "Accuracy + 5%";
        Material[] materials = StaticData.findDeepChild(transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, Color.white, Color.white, Color.white);
        outfit2(materials, StaticData.district);
    }
    public void setupModel3()
    {
        modelNum = 3;
        buffStatement = "Likability + 3";
        Material[] materials = StaticData.findDeepChild(transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, Color.white, Color.white, Color.white);
        outfit3(materials, StaticData.district);
    }
    private void outfit1(Material[] materials, int district)
    {
        StaticData.paintLongSleeveShirt(materials, colorSchemes[district - 1][0]);
        StaticData.paintPants(materials, colorSchemes[district - 1][0]);
        StaticData.paintMediumSleeveShirt(materials, colorSchemes[district - 1][1]);
        StaticData.paintShorts(materials, colorSchemes[district - 1][1]);
        StaticData.paintBra(materials, colorSchemes[district - 1][2]);
        StaticData.paintShoes(materials, colorSchemes[district - 1][2]);
    }
    private void outfit2(Material[] materials, int district)
    {
        StaticData.paintLongSleeveShirt(materials, colorSchemes[district - 1][0]);
        StaticData.paintPants(materials, colorSchemes[district - 1][1]);
        StaticData.paintShoes(materials, colorSchemes[district - 1][2]);
    }
    private void outfit3(Material[] materials, int district)
    {
        StaticData.paintLongSleeveShirt(materials, colorSchemes[district - 1][2]);
        StaticData.paintPants(materials, colorSchemes[district - 1][2]);
        StaticData.paintShoes(materials, colorSchemes[district - 1][2]);
        StaticData.paintMediumSleeveShirt(materials, colorSchemes[district - 1][1]);
        StaticData.paintShorts(materials, colorSchemes[district - 1][1]);
        StaticData.paintShortSleeveShirt(materials, colorSchemes[district - 1][0]);
        StaticData.paintUnderwear(materials, colorSchemes[district - 1][0]);
        StaticData.paintSleevelessJacket(materials, colorSchemes[district - 1][2]);
    }
    public void select()
    {
        StaticData.oufitChoice = modelNum - 1;

        outfitChoices = new List<int>();
        for (int q = 0; q < 12; q++)
        {
            outfitChoices.Add(Random.Range(1, 4));
        }
        outfitChoices[StaticData.district - 1] = modelNum;
        foreach (StaticData.TributeData data in StaticData.tributesData)
        {
            if (outfitChoices[data.district - 1] == 1)
            {
                data.strength++;
            } else if (outfitChoices[data.district - 1] == 2)
            {
                data.accuracy += 5;
            } else if (outfitChoices[data.district - 1] == 3)
            {
                data.likability += 3;
            }
        }

        Destroy(currentMenu);
        setDialogue(new List<string>(new string[] { "An excellent choice. This is what you shall " +
            "wear tonight." }),
            new List<string>(new string[] { "Stylist" }),
            dialogueBox, pickOutfit, parade);
        currentMenu = Instantiate(dialogueBox);
        sayingIdx = 0;
        selectionMode = SelectionMode.SELECTION_MADE;
        nextSaying();
    }
    public void moveOn()
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
    private void nextTribute()
    {
        Destroy(currentTribute);
        StaticData.TributeData data = StaticData.tributesData[tributeIdx];
        if (data.gender == StaticData.Gender.FEMALE)
        {
            if (data.age >= 15)
            {
                currentTribute = Instantiate(olderFemale);
            }
            else
            {
                currentTribute = Instantiate(youngerFemale);
            }
        } else if (data.gender == StaticData.Gender.MALE)
        {
            if (data.age >= 15)
            {
                currentTribute = Instantiate(olderMale);
            }
            else
            {
                currentTribute = Instantiate(youngerMale);
            }
        }
        currentTribute.transform.position = chariotPosition.position;
        currentTribute.transform.rotation = chariotPosition.rotation;
        Material[] materials = StaticData.findDeepChild(currentTribute.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, StaticData.hairColors[data.appearance[0]],
            StaticData.skinColors[data.appearance[1]], StaticData.eyeColors[data.appearance[2]]);
        if (outfitChoices[data.district - 1] == 1)
        {
            outfit1(materials, data.district);
        } else if (outfitChoices[data.district - 1] == 2)
        {
            outfit2(materials, data.district);
            GameObject prop = Instantiate(props[data.district - 1]);
            Transform hand = StaticData.findDeepChild(currentTribute.transform, "RightHand");
            prop.transform.SetParent(hand);
            prop.transform.localPosition = hand.localPosition;
            prop.transform.rotation = hand.rotation;
        }
        else if (outfitChoices[data.district - 1] == 3)
        {
            outfit3(materials, data.district);
        }
        StaticData.findDeepChild(currentMenu.transform, "Tribute")
            .GetComponent<TextMeshProUGUI>().text = data.name + " - District " + data.district;
    }
    private void setSnow()
    {
        GameObject snow = Instantiate(olderMale);
        Material[] materials = StaticData.findDeepChild(snow.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, Color.white,
            new Color(240/255f, 199/255f, 177/255f), Color.blue);
        StaticData.paintSleevelessShirt(materials, Color.white);
        StaticData.paintLongSleeveJacket(materials, new Color(139/255f, 0, 90/255f));
        StaticData.paintPants(materials, new Color(139 / 255f, 0, 90 / 255f));
        StaticData.paintShoes(materials, Color.black);
        StaticData.getMaterialByName(materials, "Tie").color = Color.black;
        StaticData.getMaterialByName(materials, "TopTie").color = Color.black;
        StaticData.getMaterialByName(materials, "TopRightMid").color = Color.black;
        StaticData.getMaterialByName(materials, "TopLeftMid").color = Color.black;
        snow.transform.position = snowPosition.position;
        snow.transform.rotation = snowPosition.rotation;
    }
    public void endParade()
    {
        SceneManager.LoadScene("Training");
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (selectionMode == SelectionMode.FADEOUT)
        {
            StaticData.findDeepChild(currentMenu.transform, "Background").GetComponent<CanvasGroup>()
                .alpha = (initialTime - timer) / initialTime;
            /*
            StaticData.findDeepChild(currentMenu.transform, "Skip").GetComponent<CanvasGroup>()
                .alpha = (initialTime - timer) / initialTime;
            */
            if (timer <= 0)
            {
                float musicLength = music.clip.length;
                float timePerTribute = musicLength / StaticData.numTributes;
                timeFade = timePerTribute * 0.15f;
                timeLinger = timePerTribute * 0.7f;

                tributeIdx = 0;
                nextTribute();
                setTimer(timeFade);
                music.Play();

                selectionMode = SelectionMode.OPEN_TRIBUTE;
            }
        } else if (selectionMode == SelectionMode.OPEN_TRIBUTE)
        {
            StaticData.findDeepChild(currentMenu.transform, "Showcase").GetComponent<CanvasGroup>()
                .alpha = (initialTime - timer) / initialTime;
            if (timer <= 0)
            {
                setTimer(timeLinger);

                selectionMode = SelectionMode.PAUSE_TRIBUTE;
            }
        } else if (selectionMode == SelectionMode.PAUSE_TRIBUTE)
        {
            if (timer <= 0)
            {
                setTimer(timeFade);

                selectionMode = SelectionMode.CLOSE_TRIBUTE;
            }
        } else if (selectionMode == SelectionMode.CLOSE_TRIBUTE)
        {
            StaticData.findDeepChild(currentMenu.transform, "Showcase").GetComponent<CanvasGroup>()
                .alpha = timer / initialTime;
            if (timer <= 0)
            {
                tributeIdx++;
                if (tributeIdx < StaticData.numTributes)
                {
                    nextTribute();
                    selectionMode = SelectionMode.OPEN_TRIBUTE;
                } else
                {
                    StaticData.findDeepChild(currentMenu.transform, "Skip").gameObject
                        .SetActive(false);
                    StaticData.findDeepChild(currentMenu.transform, "SnowSpeech").gameObject
                        .SetActive(true);
                    setSnow();
                    setTimer(1);
                    selectionMode = SelectionMode.OPEN_SNOW;
                }
            }
        } else if (selectionMode == SelectionMode.OPEN_SNOW)
        {
            StaticData.findDeepChild(currentMenu.transform, "SnowSpeech").GetComponent<CanvasGroup>()
                .alpha = (initialTime - timer) / initialTime;
            if (timer <= 0)
            {
                selectionMode = SelectionMode.PAUSE_SNOW;
            }
        }
    }
    private enum SelectionMode
    {
        DESCRIPTION, PICK, SELECTION_MADE, FADEOUT, OPEN_TRIBUTE, PAUSE_TRIBUTE, CLOSE_TRIBUTE,
        OPEN_SNOW, PAUSE_SNOW
    }
}
