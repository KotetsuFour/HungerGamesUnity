using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Television : Interactable
{
    [SerializeField] private GameObject tributeInfo;
    [SerializeField] private GameObject appearanceCustomizer;
    [SerializeField] private Transform photoPos;
    [SerializeField] private GameObject olderMale;
    [SerializeField] private GameObject olderFemale;
    [SerializeField] private GameObject youngerMale;
    [SerializeField] private GameObject youngerFemale;
    private int currentTributeIdx;
    private GameObject currentTribute;
    private bool safeChange;
    private List<GameObject> partners;
    private TrainSetup trainSetup;

    public override void menu()
    {
        stillInteracting = true;

        currentMenu = Instantiate(tributeInfo);
        configureMenu();
        currentTributeIdx = 0;
        setTribute();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public override void Z()
    {
        //nothing
    }
    private void setTribute()
    {
        StaticData.TributeData data = StaticData.tributesData[currentTributeIdx];
        StaticData.findDeepChild(currentMenu.transform, "Specific Tribute")
            .GetComponent<TMP_Dropdown>().value = currentTributeIdx;
        setModel(data);

        if (StaticData.chooseAppearances && data != StaticData.playerData)
        {
            StaticData.findDeepChild(currentMenu.transform, "Portrait")
                .GetComponent<Button>().interactable = true;
        }
        else
        {
            StaticData.findDeepChild(currentMenu.transform, "Portrait")
                .GetComponent<Button>().interactable = false;
        }
        StaticData.findDeepChild(currentMenu.transform, "Name")
            .GetComponent<TMP_InputField>().text = "";
        if (data.name != data.id)
        {
            StaticData.findDeepChild(currentMenu.transform, "Name")
                .GetComponent<TMP_InputField>().text = data.name;
        }
        if (data == StaticData.playerData)
        {
            StaticData.findDeepChild(currentMenu.transform, "Name")
                .GetComponent<TMP_InputField>().interactable = false;
        } else
        {
            StaticData.findDeepChild(currentMenu.transform, "Name")
                .GetComponent<TMP_InputField>().interactable = true;
        }
        StaticData.findDeepChild(currentMenu.transform, "District")
            .GetComponent<TMP_Dropdown>().value = data.district - 1;
        if (data == StaticData.playerData)
        {
            StaticData.findDeepChild(currentMenu.transform, "District")
                .GetComponent<TMP_Dropdown>().interactable = false;
        }
        else if (StaticData.chooseDistricts)
        {
            StaticData.findDeepChild(currentMenu.transform, "District")
                .GetComponent<TMP_Dropdown>().interactable = true;
        }
        StaticData.findDeepChild(currentMenu.transform, "Gender")
            .GetComponent<TMP_Dropdown>().value = (int)data.gender - 1;
        if (data == StaticData.playerData)
        {
            StaticData.findDeepChild(currentMenu.transform, "Gender")
                .GetComponent<TMP_Dropdown>().interactable = false;
        }
        else if (StaticData.chooseGenders)
        {
            StaticData.findDeepChild(currentMenu.transform, "Gender")
                .GetComponent<TMP_Dropdown>().interactable = true;
        }
        StaticData.findDeepChild(currentMenu.transform, "Age")
            .GetComponent<TMP_Dropdown>().value = data.age - StaticData.minAge;
        if (data == StaticData.playerData)
        {
            StaticData.findDeepChild(currentMenu.transform, "Age")
                .GetComponent<TMP_Dropdown>().interactable = false;
        }
        else if (StaticData.chooseAges)
        {
            StaticData.findDeepChild(currentMenu.transform, "Age")
                .GetComponent<TMP_Dropdown>().interactable = true;
        }
        if (StaticData.chooseAttitudes)
        {
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().interactable = true;
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().value = (int)data.attitude - 1;
        } else
        {
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().value = (int)data.reapStatus - 1;
            Debug.Log(StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().value);
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<Image>().color = Color.red;
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().interactable = false;
        }
        if (data == StaticData.playerData)
        {
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().interactable = false;
        }
        else if (StaticData.chooseAttitudes)
        {
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().interactable = true;
        }
        if (StaticData.chooseSpecialties && data != StaticData.playerData)
        {
            StaticData.findDeepChild(currentMenu.transform, "Specialty")
                .GetComponent<TMP_Dropdown>().interactable = true;
            StaticData.findDeepChild(currentMenu.transform, "Specialty")
                .GetComponent<Image>().color = Color.white;
            if (data.skillsAcquired.Count != 0)
            {
                StaticData.findDeepChild(currentMenu.transform, "Specialty")
                    .GetComponent<TMP_Dropdown>().value = (int)data.skillsAcquired[0] + 1;
            } else
            {
                StaticData.findDeepChild(currentMenu.transform, "Specialty")
                    .GetComponent<TMP_Dropdown>().value = 0;
            }
        } else
        {
            StaticData.findDeepChild(currentMenu.transform, "Specialty")
                .GetComponent<TMP_Dropdown>().interactable = false;
            StaticData.findDeepChild(currentMenu.transform, "Specialty")
                .GetComponent<Image>().color = Color.red;
            StaticData.findDeepChild(currentMenu.transform, "Specialty")
                .GetComponent<TMP_Dropdown>().value = 0;
        }
        StaticData.findDeepChild(currentMenu.transform, "ID")
            .GetComponent<TextMeshProUGUI>().text = data.id;
    }
    private void configureMenu()
    {
        Debug.Log("configure");
        if (StaticData.chooseAppearances)
        {
            StaticData.findDeepChild(currentMenu.transform, "Portrait")
                .GetComponent<Button>().interactable = true;
            Button.ButtonClickedEvent customizeEvent = new Button.ButtonClickedEvent();
            customizeEvent.AddListener(customizeTribute);
            StaticData.findDeepChild(currentMenu.transform, "Portrait")
                .GetComponent<Button>().onClick = customizeEvent;
        }
        else
        {
            StaticData.findDeepChild(currentMenu.transform, "Portrait")
                .GetComponent<Button>().interactable = false;
        }
        if (StaticData.chooseDistricts)
        {
            StaticData.findDeepChild(currentMenu.transform, "District")
                .GetComponent<TMP_Dropdown>().image.color = Color.white;
            StaticData.findDeepChild(currentMenu.transform, "District")
                .GetComponent<TMP_Dropdown>().interactable = true;
        } else
        {
            StaticData.findDeepChild(currentMenu.transform, "District")
                .GetComponent<TMP_Dropdown>().image.color = Color.red;
            StaticData.findDeepChild(currentMenu.transform, "District")
                .GetComponent<TMP_Dropdown>().interactable = false;
        }
        if (StaticData.chooseGenders)
        {
            StaticData.findDeepChild(currentMenu.transform, "Gender")
                .GetComponent<TMP_Dropdown>().image.color = Color.white;
            StaticData.findDeepChild(currentMenu.transform, "Gender")
                .GetComponent<TMP_Dropdown>().interactable = true;
        } else
        {
            StaticData.findDeepChild(currentMenu.transform, "Gender")
               .GetComponent<TMP_Dropdown>().image.color = Color.red;
            StaticData.findDeepChild(currentMenu.transform, "Gender")
                .GetComponent<TMP_Dropdown>().interactable = false;
        }
        if (StaticData.chooseAges)
        {
            StaticData.findDeepChild(currentMenu.transform, "Age")
                .GetComponent<TMP_Dropdown>().image.color = Color.white;
            StaticData.findDeepChild(currentMenu.transform, "Age")
                .GetComponent<TMP_Dropdown>().interactable = true;
        } else
        {
            StaticData.findDeepChild(currentMenu.transform, "Age")
                .GetComponent<TMP_Dropdown>().image.color = Color.red;
            StaticData.findDeepChild(currentMenu.transform, "Age")
                .GetComponent<TMP_Dropdown>().interactable = false;
        }
        StaticData.findDeepChild(currentMenu.transform, "Age")
            .GetComponent<TMP_Dropdown>().options.Clear();
        for (int q = StaticData.minAge; q < StaticData.maxAge + 1; q++)
        {
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = q + " y/o";
            StaticData.findDeepChild(currentMenu.transform, "Age")
                .GetComponent<TMP_Dropdown>().options.Add(opt);
        }
        StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
            .GetComponent<TMP_Dropdown>().options.Clear();
        if (StaticData.chooseAttitudes)
        {
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().image.color = Color.white;
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().interactable = true;
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = "RUNNER";
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().options.Add(opt);
            TMP_Dropdown.OptionData opt1 = new TMP_Dropdown.OptionData();
            opt.text = "FIGHTER";
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().options.Add(opt1);
            TMP_Dropdown.OptionData opt2 = new TMP_Dropdown.OptionData();
            opt.text = "GLADIATOR";
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().options.Add(opt2);
            TMP_Dropdown.OptionData opt3 = new TMP_Dropdown.OptionData();
            opt.text = "PROTECTOR";
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().options.Add(opt3);
        }
        else
        {
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().image.color = Color.red;
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().interactable = false;
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = "REAPED";
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().options.Add(opt);
            TMP_Dropdown.OptionData opt1 = new TMP_Dropdown.OptionData();
            opt1.text = "VOLUNTEERED";
            StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().options.Add(opt1);
        }
        StaticData.findDeepChild(currentMenu.transform, "Specialty")
            .GetComponent<TMP_Dropdown>().options.Clear();
        StaticData.Skill[] skills = (StaticData.Skill[])Enum.GetValues(typeof(StaticData.Skill));
        TMP_Dropdown.OptionData none = new TMP_Dropdown.OptionData();
        none.text = "SPECIALTY: UNSPECIFIED";
        StaticData.findDeepChild(currentMenu.transform, "Specialty")
            .GetComponent<TMP_Dropdown>().options.Add(none);
        foreach (StaticData.Skill skill in skills)
        {
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = "SPECIALTY: " + skill;
            StaticData.findDeepChild(currentMenu.transform, "Specialty")
                .GetComponent<TMP_Dropdown>().options.Add(opt);
        }
        Button.ButtonClickedEvent prev = new Button.ButtonClickedEvent();
        prev.AddListener(previousTribute);
        StaticData.findDeepChild(currentMenu.transform, "Previous")
            .GetComponent<Button>().onClick = prev;
        Button.ButtonClickedEvent next = new Button.ButtonClickedEvent();
        next.AddListener(nextTribute);
        StaticData.findDeepChild(currentMenu.transform, "Next")
            .GetComponent<Button>().onClick = next;
        TMP_Dropdown.DropdownEvent pick = new TMP_Dropdown.DropdownEvent();
        pick.AddListener(specificTribute);
        TMP_Dropdown spec = StaticData.findDeepChild(currentMenu.transform, "Specific Tribute")
            .GetComponent<TMP_Dropdown>();
        spec.onValueChanged = pick;
        spec.options.Clear();
        for (int q = 0; q < StaticData.numTributes; q++)
        {
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = q + "";
            spec.options.Add(opt);
        }
        Button.ButtonClickedEvent exit = new Button.ButtonClickedEvent();
        exit.AddListener(exitMenu);
        StaticData.findDeepChild(currentMenu.transform, "Close")
            .GetComponent<Button>().onClick = exit;
    }

    public void customizeTribute()
    {
        changeData();
        Destroy(currentMenu);
        currentMenu = Instantiate(appearanceCustomizer);

        TMP_Dropdown hair = StaticData.findDeepChild(currentMenu.transform, "HairColor")
            .GetComponent<TMP_Dropdown>();
        hair.options.Clear();
        for (int q = 0; q < StaticData.hairColors.Length; q++)
        {
            Color color = StaticData.hairColors[q];
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = "rgb(" + Mathf.RoundToInt(color.r * 255)
                + "," + Mathf.RoundToInt(color.g * 255) + "," + Mathf.RoundToInt(color.r * 255)
                + ")";
            hair.options.Add(opt);
        }
        hair.value = StaticData.tributesData[currentTributeIdx].appearance[0];
        TMP_Dropdown.DropdownEvent pickHair = new TMP_Dropdown.DropdownEvent();
        pickHair.AddListener(setHair);
        hair.onValueChanged = pickHair;

        TMP_Dropdown skin = StaticData.findDeepChild(currentMenu.transform, "SkinColor")
            .GetComponent<TMP_Dropdown>();
        skin.options.Clear();
        for (int q = 0; q < StaticData.skinColors.Length; q++)
        {
            Color color = StaticData.skinColors[q];
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = "rgb(" + Mathf.RoundToInt(color.r * 255)
                + "," + Mathf.RoundToInt(color.g * 255) + "," + Mathf.RoundToInt(color.r * 255)
                + ")";
            skin.options.Add(opt);
        }
        skin.value = StaticData.tributesData[currentTributeIdx].appearance[1];
        TMP_Dropdown.DropdownEvent pickSkin = new TMP_Dropdown.DropdownEvent();
        pickSkin.AddListener(setSkin);
        skin.onValueChanged = pickSkin;

        TMP_Dropdown eye = StaticData.findDeepChild(currentMenu.transform, "EyeColor")
            .GetComponent<TMP_Dropdown>();
        eye.options.Clear();
        for (int q = 0; q < StaticData.eyeColors.Length; q++)
        {
            Color color = StaticData.eyeColors[q];
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = "rgb(" + Mathf.RoundToInt(color.r * 255)
                + "," + Mathf.RoundToInt(color.g * 255) + "," + Mathf.RoundToInt(color.r * 255)
                + ")";
            eye.options.Add(opt);
        }
        eye.value = StaticData.tributesData[currentTributeIdx].appearance[2];
        TMP_Dropdown.DropdownEvent pickEye = new TMP_Dropdown.DropdownEvent();
        pickEye.AddListener(setEye);
        eye.onValueChanged = pickEye;

        Button.ButtonClickedEvent goBack = new Button.ButtonClickedEvent();
        goBack.AddListener(toTributeInfo);
        StaticData.findDeepChild(currentMenu.transform, "Close")
            .GetComponent<Button>().onClick = goBack;
    }
    public void toTributeInfo()
    {
        int numPerDistrict = StaticData.numTributes / 12;
        int partnerIdx = 0;
        int startIdx = (StaticData.district - 1) * numPerDistrict;
        for (int q = startIdx; q < startIdx + numPerDistrict; q++)
        {
            if (StaticData.tributesData[q] != StaticData.playerData)
            {
                paintReapingClothes(partners[partnerIdx], StaticData.tributesData[q]);
                partnerIdx++;
            }
        }
        Destroy(currentMenu);
        currentMenu = Instantiate(tributeInfo);
        configureMenu();
        safeChange = true;
        setTribute();
        safeChange = false;
    }
    public void previousTribute()
    {
        changeData();
        currentTributeIdx = Mathf.Max(0, currentTributeIdx - 1);
        safeChange = true;
        setTribute();
        safeChange = false;
    }
    public void nextTribute()
    {
        changeData();
        currentTributeIdx = Mathf.Min(StaticData.numTributes - 1, currentTributeIdx + 1);
        safeChange = true;
        setTribute();
        safeChange = false;
    }
    public void exitMenu()
    {
        changeData();
        partners = trainSetup.loadPartners(); //Not necessary to reassign the partners variable, but whatever
        Destroy(currentMenu);
        currentMenu = null;
        Destroy(currentTribute);
        currentTribute = null;
        stillInteracting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void specificTribute(int pick)
    {
        if (!safeChange)
        {
            changeData();
            currentTributeIdx = StaticData.findDeepChild(currentMenu.transform, "Specific Tribute")
                .GetComponent<TMP_Dropdown>().value;
            setTribute();
        }
    }
    public void setHair(int num)
    {
        StaticData.TributeData data = StaticData.tributesData[currentTributeIdx];
        data.appearance[0] = num;
        paintReapingClothes(currentTribute, data);
    }
    public void setSkin(int num)
    {
        StaticData.TributeData data = StaticData.tributesData[currentTributeIdx];
        data.appearance[1] = num;
        paintReapingClothes(currentTribute, data);
    }
    public void setEye(int num)
    {
        StaticData.TributeData data = StaticData.tributesData[currentTributeIdx];
        data.appearance[2] = num;
        paintReapingClothes(currentTribute, data);
    }
    private void changeData()
    {
        StaticData.TributeData data = StaticData.tributesData[currentTributeIdx];
        string tryName = StaticData.findDeepChild(currentMenu.transform, "Name")
            .GetComponent<TMP_InputField>().text;
        if (tryName != null && tryName != "")
        {
            data.name = tryName;
        } else
        {
            data.name = data.id;
        }
        int dist = StaticData.findDeepChild(currentMenu.transform, "District")
            .GetComponent<TMP_Dropdown>().value + 1;
        data.district = dist;
        StaticData.Gender gend = (StaticData.Gender) StaticData.findDeepChild(currentMenu.transform, "Gender")
            .GetComponent<TMP_Dropdown>().value + 1;
        data.gender = gend;
        int age = StaticData.findDeepChild(currentMenu.transform, "Age")
            .GetComponent<TMP_Dropdown>().value + StaticData.minAge;
        data.age = age;
        if (StaticData.chooseAttitudes) {
            StaticData.Attitude atti = (StaticData.Attitude)StaticData.findDeepChild(currentMenu.transform, "ReapedStatus")
                .GetComponent<TMP_Dropdown>().value + 1;
            data.attitude = atti;
        }
        TMP_Dropdown specDrop = StaticData.findDeepChild(currentMenu.transform, "Specialty")
                .GetComponent<TMP_Dropdown>();
        if (specDrop.value > 0)
        {
            StaticData.Skill spec = (StaticData.Skill)specDrop.value - 1;
            data.giveSpecialty(spec);
        }
    }
    private void setModel(StaticData.TributeData data)
    {
        Destroy(currentTribute);
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
        }
        else if (data.gender == StaticData.Gender.MALE)
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
        paintReapingClothes(currentTribute, data);
        currentTribute.transform.position = photoPos.position;
        currentTribute.transform.rotation = photoPos.rotation;
        if (data.age >= 75)
        {
            StaticData.findDeepChild(currentTribute.transform, "model").GetComponent<Animator>()
                .Play("Old Man Idle");
        }
        if (StaticData.usingVictors)
        {
            if (data.attitude == StaticData.Attitude.GLADIATOR)
            {
                StaticData.findDeepChild(currentTribute.transform, "model").GetComponent<Animator>()
                    .Play("Victory");
            } else if (data.attitude == StaticData.Attitude.RUNNER)
            {
                StaticData.findDeepChild(currentTribute.transform, "model").GetComponent<Animator>()
                    .Play("Agony");
            }
        }
    }
    private void paintReapingClothes(GameObject tribute, StaticData.TributeData data)
    {
        Material[] materials = StaticData.findDeepChild(tribute.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, StaticData.hairColors[data.appearance[0]],
            StaticData.skinColors[data.appearance[1]], StaticData.eyeColors[data.appearance[2]]);
        StaticData.paintMediumSleeveShirt(materials, Color.cyan);
        StaticData.paintPants(materials, Color.blue);
        StaticData.paintShoes(materials, Color.black);
    }
    public void setPartners(List<GameObject> partners)
    {
        this.partners = partners;
    }
    public void setTrainSetup(TrainSetup setup)
    {
        this.trainSetup = setup;
    }
}
