using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Mirror : Interactable
{
    [SerializeField] private GameObject appearanceCustomizer;
    [SerializeField] private Material mirrorMaterial;
    [SerializeField] private GameObject olderMale;
    [SerializeField] private GameObject olderFemale;
    [SerializeField] private GameObject youngerMale;
    [SerializeField] private GameObject youngerFemale;

    private GameObject player;
    private StaticData.TributeData playerData;

    public override string interactNote(Tribute caller)
    {
        return "Mirror";
    }

    public override void menu(Tribute caller)
    {
        stillInteracting = true;

        currentMenu = Instantiate(appearanceCustomizer);
        StaticData.findDeepChild(currentMenu.transform, "Portrait").GetComponent<Image>()
            .material = mirrorMaterial;
        configureMenu();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public override void Z()
    {
        //nothing
    }
    private void configureMenu()
    {
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
        hair.value = playerData.appearance[0];
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
        skin.value = playerData.appearance[1];
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
        eye.value = playerData.appearance[2];
        TMP_Dropdown.DropdownEvent pickEye = new TMP_Dropdown.DropdownEvent();
        pickEye.AddListener(setEye);
        eye.onValueChanged = pickEye;

        Button.ButtonClickedEvent goBack = new Button.ButtonClickedEvent();
        goBack.AddListener(close);
        StaticData.findDeepChild(currentMenu.transform, "Close")
            .GetComponent<Button>().onClick = goBack;
    }
    public void setHair(int num)
    {
        playerData.appearance[0] = num;
        paintReapingClothes(player, playerData);
    }
    public void setSkin(int num)
    {
        playerData.appearance[1] = num;
        paintReapingClothes(player, playerData);
    }
    public void setEye(int num)
    {
        playerData.appearance[2] = num;
        paintReapingClothes(player, playerData);
    }
    public void close()
    {
        Destroy(currentMenu);
        currentMenu = null;
        stillInteracting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void setPlayer(GameObject player, StaticData.TributeData playerData)
    {
        this.player = player;
        this.playerData = playerData;
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


}
