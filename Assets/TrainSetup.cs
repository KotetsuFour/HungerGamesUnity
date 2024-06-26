using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSetup : MonoBehaviour
{
    [SerializeField] private Transform[] partnerPositions;
    [SerializeField] private Transform escortPos;
    [SerializeField] private Transform mentorPos;
    [SerializeField] private Transform playerPos;

    [SerializeField] private GameObject olderMalePlayer;
    [SerializeField] private GameObject olderFemalePlayer;
    [SerializeField] private GameObject youngerMalePlayer;
    [SerializeField] private GameObject youngerFemalePlayer;

    [SerializeField] private GameObject olderMaleNPC;
    [SerializeField] private GameObject olderFemaleNPC;
    [SerializeField] private GameObject youngerMaleNPC;
    [SerializeField] private GameObject youngerFemaleNPC;

    [SerializeField] private Color reapingShirt;
    [SerializeField] private Color reapingPants;

    [SerializeField] private Color escortHair;
    [SerializeField] private Color escortSkin;
    [SerializeField] private Color escortJacket;
    [SerializeField] private Color escortPants;
    [SerializeField] private Color escortShoes;

    [SerializeField] private Color defaultMentorHair;
    [SerializeField] private Color defaultMentorSkin;
    [SerializeField] private Color defaultMentorEye;
    [SerializeField] private Color mentorShirt;
    [SerializeField] private Color mentorPants;

    [SerializeField] private GameObject television;
    [SerializeField] private GameObject mirror;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject speakBox;
    [SerializeField] private GameObject fadeScreen;

    private List<StaticData.TributeData> partnersData;
    private List<GameObject> partners;
    private GameObject player;
    private GameObject escort;
    private GameObject mentor;

    private static string SITTING_IDLE = "Sitting Idle";
    private static string SITTING_TALKING = "Sitting Talking";

    // Start is called before the first frame update
    void Start()
    {
        partners = new List<GameObject>();
        partnersData = new List<StaticData.TributeData>();
        int numPerDistrict = StaticData.numTributes / 12;
        int startIdx = (StaticData.district - 1) * numPerDistrict;
        for (int q = startIdx; q < startIdx + numPerDistrict; q++)
        {
            StaticData.TributeData data = StaticData.tributesData[q];
            if (data == StaticData.playerData)
            {
                if (StaticData.gender == StaticData.Gender.FEMALE)
                {
                    if (StaticData.age >= 15)
                    {
                        player = Instantiate(olderFemalePlayer);
                    }
                    else
                    {
                        player = Instantiate(youngerFemalePlayer);
                    }
                }
                else if (StaticData.gender == StaticData.Gender.MALE)
                {
                    if (StaticData.age >= 15)
                    {
                        player = Instantiate(olderMalePlayer);
                    }
                    else
                    {
                        player = Instantiate(youngerMalePlayer);
                    }
                }
                paintReapingClothes(player, data);
                moveTo(player, playerPos);
            }
            else
            {
                partnersData.Add(data);
            }
        }
        loadPartners();

        setMentor();
        moveTo(mentor, mentorPos);
        StaticData.findDeepChild(mentor.transform, "model")
            .GetComponent<Animator>().Play(SITTING_TALKING);

        setEscort();
        moveTo(escort, escortPos);

        television.GetComponent<Television>().setPartners(partners);
        television.GetComponent<Television>().setTrainSetup(this);
        mirror.GetComponent<Mirror>().setPlayer(player, StaticData.playerData);

        mentor.AddComponent<SpecialtySelect>().setNecessities(dialogueBox, speakBox, fadeScreen);
        mentor.layer = 6;

        escort.AddComponent<Talker>().setDialogue(new List<string>(new string[] {
            "We'll arrive at the Capitol soon. You're going to love it there.",
            "In the meantime, you can freshen up in front of the mirror or familiarize yourself " +
            "with the other tributes on the television.",
            "Whenever you're done, go and speak with your mentor. I'm sure you have a lot to talk about."
        }), new List<string>(new string[] {
            "Escort"
        }), dialogueBox);
        escort.layer = 6;
        player.GetComponent<PlayerController>().quickConstruct();
        escort.GetComponent<Talker>().menu();
        player.GetComponent<PlayerController>().hijack(escort.GetComponent<Talker>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void moveTo(GameObject character, Transform pos)
    {
        character.transform.position = pos.position;
        character.transform.rotation = pos.rotation;
    }
    private void setMentor()
    {
        mentor = Instantiate(olderMaleNPC);
        StaticData.mentor = new StaticData.TributeData("Mentor");
        StaticData.mentor.gender = StaticData.Gender.MALE;
        StaticData.mentor.name = "Alcimus"; //The father of Mentor in the Odyssey
        StaticData.defaultMentorHair = defaultMentorHair;
        StaticData.defaultMentorSkin = defaultMentorSkin;
        StaticData.defaultMentorEye = defaultMentorEye;
        StaticData.defaultMentorShirt = mentorShirt;
        StaticData.defaultMentorPants = mentorPants;
        Material[] materials = StaticData.findDeepChild(mentor.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, defaultMentorHair,
            defaultMentorSkin, defaultMentorEye);
        StaticData.paintMediumSleeveShirt(materials, mentorShirt);
        StaticData.paintPants(materials, mentorPants);
        StaticData.paintShoes(materials, Color.black);
    }
    private void paintReapingClothes(GameObject tribute, StaticData.TributeData data)
    {
        Material[] materials = StaticData.findDeepChild(tribute.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, StaticData.hairColors[data.appearance[0]],
            StaticData.skinColors[data.appearance[1]], StaticData.eyeColors[data.appearance[2]]);
        StaticData.paintMediumSleeveShirt(materials, reapingShirt);
        StaticData.paintPants(materials, reapingPants);
        StaticData.paintShoes(materials, Color.black);
    }
    private void setEscort()
    {
        escort = Instantiate(olderFemaleNPC);
        Material[] materials = StaticData.findDeepChild(escort.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, escortHair,
            escortSkin, Color.blue);
        StaticData.paintPants(materials, escortShoes);
        StaticData.paintShoes(materials, escortShoes);
        StaticData.paintShorts(materials, escortPants);
        StaticData.paintLongSleeveShirt(materials, escortPants);
        StaticData.paintShortSleeveJacket(materials, escortJacket);
        StaticData.getMaterialByName(materials, "Skin").color = Color.white;
    }

    public List<GameObject> loadPartners()
    {
        foreach(GameObject p in partners)
        {
            Destroy(p);
        }
        partners.Clear();
        for (int q = 0; q < partnersData.Count; q++)
        {
            StaticData.TributeData data = partnersData[q];
            if (data.gender == StaticData.Gender.FEMALE)
            {
                if (data.age >= 15)
                {
                    partners.Add(Instantiate(olderFemaleNPC));
                }
                else
                {
                    partners.Add(Instantiate(youngerFemaleNPC));
                }
            }
            else if (data.gender == StaticData.Gender.MALE)
            {
                if (data.age >= 15)
                {
                    partners.Add(Instantiate(olderMaleNPC));
                }
                else
                {
                    partners.Add(Instantiate(youngerMaleNPC));
                }
            }
            paintReapingClothes(partners[partners.Count - 1], data);

            moveTo(partners[q], partnerPositions[q]);
            StaticData.findDeepChild(partners[q].transform, "model")
                .GetComponent<Animator>().Play(SITTING_IDLE);

            string quote = null;
            if (data.attitude == StaticData.Attitude.RUNNER)
            {
                quote = "I want to go home.";
            } else if (data.attitude == StaticData.Attitude.FIGHTER)
            {
                quote = "...";
            }
            else if (data.attitude == StaticData.Attitude.GLADIATOR)
            {
                quote = "This is my time";
            }
            else if (data.attitude == StaticData.Attitude.PROTECTOR)
            {
                quote = "I hope my family is okay.";
            }
            partners[q].AddComponent<Talker>().setDialogue(new List<string>(new string[] {
                quote
            }), new List<string>(new string[] {
                data.name
            }), dialogueBox);
            partners[q].layer = 6;
        }
        return partners;
    }
}
