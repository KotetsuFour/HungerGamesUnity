using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylistSetup : MonoBehaviour
{
    [SerializeField] private GameObject stylist;
    [SerializeField] private GameObject olderMalePlayer;
    [SerializeField] private GameObject olderFemalePlayer;
    [SerializeField] private GameObject youngerMalePlayer;
    [SerializeField] private GameObject youngerFemalePlayer;
    [SerializeField] private Transform playerPos;
    [SerializeField] private GameObject olderMaleNPC;
    [SerializeField] private GameObject olderFemaleNPC;
    [SerializeField] private GameObject youngerMaleNPC;
    [SerializeField] private GameObject youngerFemaleNPC;
    [SerializeField] private Transform modelPos1;
    [SerializeField] private Transform modelPos2;
    [SerializeField] private Transform modelPos3;
    [SerializeField] private Transform chariotPos;
    [SerializeField] private Transform snowPos;
    [SerializeField] private Color stylistHair;
    [SerializeField] private Color stylistSkin;
    [SerializeField] private Color stylistEye;
    [SerializeField] private Color scrubsColor;
    private GameObject player;
    private GameObject model1;
    private GameObject model2;
    private GameObject model3;

    [SerializeField] private List<GameObject> props;
    [SerializeField] private List<Color> colorScheme1;
    [SerializeField] private List<Color> colorScheme2;
    [SerializeField] private List<Color> colorScheme3;
    [SerializeField] private List<Color> colorScheme4;
    [SerializeField] private List<Color> colorScheme5;
    [SerializeField] private List<Color> colorScheme6;
    [SerializeField] private List<Color> colorScheme7;
    [SerializeField] private List<Color> colorScheme8;
    [SerializeField] private List<Color> colorScheme9;
    [SerializeField] private List<Color> colorScheme10;
    [SerializeField] private List<Color> colorScheme11;
    [SerializeField] private List<Color> colorScheme12;
    private List<Color>[] colorSchemes;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject pickOutfit;
    [SerializeField] private GameObject paradeScreen;

    // Start is called before the first frame update
    void Start()
    {
        setupStylist();
        if (StaticData.gender == StaticData.Gender.FEMALE)
        {
            if (StaticData.age >= 15)
            {
                player = Instantiate(olderFemalePlayer);
                model1 = Instantiate(olderFemaleNPC);
                model2 = Instantiate(olderFemaleNPC);
                model3 = Instantiate(olderFemaleNPC);
            }
            else
            {
                player = Instantiate(youngerFemalePlayer);
                model1 = Instantiate(youngerFemaleNPC);
                model2 = Instantiate(youngerFemaleNPC);
                model3 = Instantiate(youngerFemaleNPC);
            }
        } else if (StaticData.gender == StaticData.Gender.MALE)
        {
            if (StaticData.age >= 15)
            {
                player = Instantiate(olderMalePlayer);
                model1 = Instantiate(olderMaleNPC);
                model2 = Instantiate(olderMaleNPC);
                model3 = Instantiate(olderMaleNPC);
            }
            else
            {
                player = Instantiate(youngerMalePlayer);
                model1 = Instantiate(youngerMaleNPC);
                model2 = Instantiate(youngerMaleNPC);
                model3 = Instantiate(youngerMaleNPC);
            }
        }
        player.GetComponent<PlayerController>().quickConstruct();
        moveTo(player, playerPos);
        moveTo(model1, modelPos1);
        moveTo(model2, modelPos2);
        moveTo(model3, modelPos3);

        StaticData.findDeepChild(model1.transform, "model").GetComponent<Animator>()
            .Play("Runway Pose");
        StaticData.findDeepChild(model2.transform, "model").GetComponent<Animator>()
            .Play("Walking With Item Pose");
        StaticData.findDeepChild(model3.transform, "model").GetComponent<Animator>()
            .Play("Model Pose 1");
        Transform propHand = StaticData.findDeepChild(model2.transform, "RightHand");
        GameObject prop = Instantiate(props[StaticData.district - 1]);
        prop.layer = 0;
        prop.transform.SetParent(propHand);
        prop.transform.localPosition = propHand.localPosition;
        Vector3 euler = propHand.eulerAngles;
        prop.transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z);

        colorSchemes = new List<Color>[] {
            colorScheme1, colorScheme2, colorScheme3, colorScheme4,
            colorScheme5, colorScheme6, colorScheme7, colorScheme8,
            colorScheme9, colorScheme10, colorScheme11, colorScheme12
        };
        setupPlayer();

        model1.layer = 6;
        model2.layer = 6;
        model3.layer = 6;
        StyleChoice choice1 = model1.AddComponent<StyleChoice>();
        choice1.setDialogue(new List<string>(new string[] { "This outfit is designed to show the " +
            "strength of District " + StaticData.district + ". You can feel the raw power oozing from it!" }),
            new List<string>(new string[] { "Stylist" }),
            dialogueBox, pickOutfit, paradeScreen);
        choice1.setColorSchemes(colorSchemes);
        choice1.setupModel1();
        choice1.music = GetComponent<AudioSource>();
        choice1.olderMale = olderMaleNPC;
        choice1.olderFemale = olderFemaleNPC;
        choice1.youngerMale = youngerMaleNPC;
        choice1.youngerFemale = youngerFemaleNPC;
        choice1.chariotPosition = chariotPos;
        choice1.snowPosition = snowPos;
        choice1.props = props;
        choice1.interactType = Interactable.InteractType.EXAMINE;

        StyleChoice choice2 = model2.AddComponent<StyleChoice>();
        choice2.setDialogue(new List<string>(new string[] { "I quite like the accessory on this one. It " +
            "truly completes the look, don't you think?" }),
            new List<string>(new string[] { "Stylist" }),
            dialogueBox, pickOutfit, paradeScreen);
        choice2.setColorSchemes(colorSchemes);
        choice2.setupModel2();
        choice2.music = GetComponent<AudioSource>();
        choice2.olderMale = olderMaleNPC;
        choice2.olderFemale = olderFemaleNPC;
        choice2.youngerMale = youngerMaleNPC;
        choice2.youngerFemale = youngerFemaleNPC;
        choice2.chariotPosition = chariotPos;
        choice2.snowPosition = snowPos;
        choice2.props = props;
        choice2.interactType = Interactable.InteractType.EXAMINE;

        StyleChoice choice3 = model3.AddComponent<StyleChoice>();
        choice3.setDialogue(new List<string>(new string[] { "Not to influence your decision, but this " +
            "one is my personal favorite. And I think many others would agree." }),
            new List<string>(new string[] { "Stylist" }),
            dialogueBox, pickOutfit, paradeScreen);
        choice3.setColorSchemes(colorSchemes);
        choice3.setupModel3();
        choice3.music = GetComponent<AudioSource>();
        choice3.olderMale = olderMaleNPC;
        choice3.olderFemale = olderFemaleNPC;
        choice3.youngerMale = youngerMaleNPC;
        choice3.youngerFemale = youngerFemaleNPC;
        choice3.chariotPosition = chariotPos;
        choice3.snowPosition = snowPos;
        choice3.props = props;
        choice3.interactType = Interactable.InteractType.EXAMINE;

        Talker start = GetComponent<Talker>();
        start.menu();
        player.GetComponent<PlayerController>().hijack(start);
    }

    private void moveTo(GameObject character, Transform pos)
    {
        character.transform.position = pos.position;
        character.transform.rotation = pos.rotation;
    }
    private void setupStylist()
    {
        Material[] materials = StaticData.findDeepChild(stylist.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, stylistHair, stylistSkin, stylistEye);
        StaticData.paintLongSleeveShirt(materials, Color.grey);
        StaticData.paintMediumSleeveJacket(materials, Color.black);
        StaticData.paintPants(materials, Color.black);
        StaticData.paintShoes(materials, Color.black);
    }
    private void setupPlayer()
    {
        Material[] materials = StaticData.findDeepChild(player.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials,
            StaticData.hairColors[StaticData.playerData.appearance[0]],
            StaticData.skinColors[StaticData.playerData.appearance[1]],
            StaticData.eyeColors[StaticData.playerData.appearance[2]]);
        StaticData.paintShortSleeveShirt(materials, scrubsColor);
        StaticData.paintShorts(materials, scrubsColor);
    }

}
