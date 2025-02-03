using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterviewSetup : MonoBehaviour
{
    [SerializeField] private GameObject olderMaleNPC;
    [SerializeField] private GameObject olderFemaleNPC;
    [SerializeField] private GameObject youngerMaleNPC;
    [SerializeField] private GameObject youngerFemaleNPC;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform caesarPos;
    [SerializeField] private Transform mentorPos;
    private GameObject player;
    private GameObject caesar;
    private GameObject mentor;

    [SerializeField] private Color caesarHair;
    [SerializeField] private Color caesarSkin;
    [SerializeField] private Color caesarEye;
    [SerializeField] private Color caesarJacket;
    [SerializeField] private List<Color> dressColors;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject speakBox;
    [SerializeField] private GameObject fadeScreen;

    [SerializeField] private GameObject mentorCam;
    [SerializeField] private GameObject interviewCam;

    private Interactable currentMenu;

    private SelectionMode selectionMode;

    // Start is called before the first frame update
    void Start()
    {
        if (StaticData.gender == StaticData.Gender.FEMALE)
        {
            if (StaticData.age >= 15)
            {
                player = Instantiate(olderFemaleNPC);
            }
            else
            {
                player = Instantiate(youngerFemaleNPC);
            }
        }
        else if (StaticData.gender == StaticData.Gender.MALE)
        {
            if (StaticData.age >= 15)
            {
                player = Instantiate(olderMaleNPC);
            }
            else
            {
                player = Instantiate(youngerMaleNPC);
            }
        }
        paintInterviewClothes(player, StaticData.playerData);
        moveTo(player, playerPos);
        StaticData.findDeepChild(player.transform, "model").GetComponent<Animator>()
            .Play("Sitting Idle");

        setCaesar();
        moveTo(caesar, caesarPos);
        StaticData.findDeepChild(caesar.transform, "model").GetComponent<Animator>()
            .Play("Sitting Talking");

        setMentor();
        moveTo(mentor, mentorPos);

        Talker mentorText = mentor.AddComponent<Talker>();
        mentorText.setDialogue(new List<string>(new string[] { "It's time for interviews. This is your" +
            " last chance to make people like you and get sponsors. Make it count.",
        "Remember, everything you say out there will have an impact.",
        "No pressure."}),
            new List<string>(new string[] { StaticData.mentor.name }),
            dialogueBox);
        currentMenu = mentorText;
        currentMenu.menu(player.GetComponent<BetterPlayerController>());

        selectionMode = SelectionMode.MENTOR;
    }
    private void moveTo(GameObject character, Transform pos)
    {
        character.transform.position = pos.position;
        character.transform.rotation = pos.rotation;
    }

    private void setMentor()
    {
        mentor = Instantiate(olderMaleNPC);
        Material[] materials = StaticData.findDeepChild(mentor.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, StaticData.defaultMentorHair,
            StaticData.defaultMentorSkin, StaticData.defaultMentorEye);
        StaticData.paintMediumSleeveShirt(materials, StaticData.defaultMentorShirt);
        StaticData.paintPants(materials, StaticData.defaultMentorPants);
        StaticData.paintShoes(materials, Color.black);
    }

    private void setCaesar()
    {
        caesar = Instantiate(olderMaleNPC);
        Material[] materials = StaticData.findDeepChild(caesar.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, caesarHair,
            caesarSkin, caesarEye);
        StaticData.paintSleevelessShirt(materials, Color.black);
        StaticData.paintPants(materials, Color.black);
        StaticData.paintShoes(materials, Color.black);
        StaticData.paintLongSleeveJacket(materials, caesarJacket);
    }
    private void paintInterviewClothes(GameObject tribute, StaticData.TributeData data)
    {
        Material[] materials = StaticData.findDeepChild(tribute.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, StaticData.hairColors[data.appearance[0]],
            StaticData.skinColors[data.appearance[1]], StaticData.eyeColors[data.appearance[2]]);
        if (StaticData.gender == StaticData.Gender.MALE)
        {
            StaticData.paintSleevelessShirt(materials, Color.white);
            StaticData.paintPants(materials, dressColors[StaticData.district - 1]);
            StaticData.paintShoes(materials, Color.black);
            StaticData.paintLongSleeveJacket(materials, dressColors[StaticData.district - 1]);
        }
        else if (StaticData.gender == StaticData.Gender.FEMALE)
        {
            StaticData.paintSleevelessShirt(materials, dressColors[StaticData.district - 1]);
            StaticData.paintPants(materials, dressColors[StaticData.district - 1]);
            StaticData.paintShoes(materials, dressColors[StaticData.district - 1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMenu != null && currentMenu.stillInteracting && Input.GetKeyDown(KeyCode.Z))
        {
            currentMenu.Z();
        }
        if (selectionMode == SelectionMode.MENTOR && !currentMenu.stillInteracting)
        {
            Questionnaire questions = caesar.AddComponent<Questionnaire>();
            questions.setDialogue(new List<string>(new string[] { "Welcome, " + StaticData.playerName +
            "! Welcome! I must say, I have been looking forward to meeting you since the parade.",
            (StaticData.oufitChoice == 0 ? "The way you carried yourself in that armor, I think we " +
            "can all agree was quite powerful."
            : StaticData.oufitChoice == 1 ? "The way you wielded that- I assume- fake weapon " +
            "really displayed skill that I'm sure we will see more of in the Games."
            : "When you came out in that stunning outfit, my heart stopped. And I'm sure I'm not " +
            "the only one."),
            "What do you think about your stylist's choice in how he presented your district?"}),
            new List<string>(new string[] { "Caesar" }),
            dialogueBox);

            questions.setQuestion(StaticData.playerName, "I think ",
                new string[] { "Option1",
                    "Option2",
                    "Option3" },
                new int[][] { new int[] { 0, 0, 0, 0, 0, 1 },
                    new int [] { 0, 0, 0, 0, 0, 1 },
                    new int [] { 0, 0, 0, 0, 0, 1 } },
                "", speakBox);

            currentMenu = questions;
            currentMenu.menu(player.GetComponent<BetterPlayerController>());

            mentorCam.SetActive(false);
            interviewCam.SetActive(true);

            selectionMode = SelectionMode.QUESTION1;
        } else if (selectionMode == SelectionMode.QUESTION1 && !currentMenu.stillInteracting)
        {
            Questionnaire questions = caesar.GetComponent<Questionnaire>();
            questions.setDialogue(new List<string>(new string[] { "Now, let's talk about that score. A " + StaticData.trainingScore + ".",
            (StaticData.trainingScore >= 9 ? "I don't expect you to tell us how you pulled off a score that impressive," +
            " but I hope you will at least give us some idea of whether we can expect more in the Games."
            : StaticData.trainingScore <= 4 ? "It does make me a little nervous. Tell me, what can we " +
            "expect to see from you in the Games?"
            : "How accurate would you say that score is? What can we expect from you in the arena?")}),
            new List<string>(new string[] { "Caesar" }),
            dialogueBox);

            questions.setQuestion(StaticData.playerName, "You can expect ",
                new string[] { "Option1",
                    "Option2",
                    "Option3" },
                new int[][] { new int[] { 0, 0, 0, 0, 0, 1 },
                    new int [] { 0, 0, 0, 0, 0, 1 },
                    new int [] { 0, 0, 0, 0, 0, 1 } },
                "", speakBox);

            questions.menu(player.GetComponent<BetterPlayerController>());

            selectionMode = SelectionMode.QUESTION2;
        } else if (selectionMode == SelectionMode.QUESTION2 && !currentMenu.stillInteracting)
        {
            Questionnaire questions = caesar.GetComponent<Questionnaire>();
            questions.setDialogue(new List<string>(new string[] { "Now, let's talk about that score. A " + StaticData.trainingScore + ".",
            (StaticData.trainingScore >= 9 ? "I don't expect you to tell us how you pulled off a score that impressive," +
            " but I hope you will at least give us some idea of whether we can expect more in the Games."
            : StaticData.trainingScore <= 4 ? "It does make me a little nervous. Tell me, what can we " +
            "expect to see from you in the Games?"
            : "How accurate would you say that score is? What can we expect from you in the arena?")}),
            new List<string>(new string[] { "Caesar" }),
            dialogueBox);

            questions.setQuestion(StaticData.playerName, "You can expect ",
                new string[] { "Option1",
                    "Option2",
                    "Option3" },
                new int[][] { new int[] { 0, 0, 0, 0, 0, 1 },
                    new int [] { 0, 0, 0, 0, 0, 1 },
                    new int [] { 0, 0, 0, 0, 0, 1 } },
                "", speakBox);

            questions.menu(player.GetComponent<BetterPlayerController>());

            selectionMode = SelectionMode.QUESTION3;
        } else if (selectionMode == SelectionMode.QUESTION3 && !currentMenu.stillInteracting)
        {
            Talker quote = caesar.AddComponent<Talker>();
            quote.setDialogue(new List<string>(new string[] { StaticData.playerName + ", thank you for joining us " +
                "tonight. Ladies and gentlemen, " + StaticData.playerName + "!" }),
            new List<string>(new string[] { "Caesar" }),
            dialogueBox);

            currentMenu = quote;
            currentMenu.menu(player.GetComponent<BetterPlayerController>());

            selectionMode = SelectionMode.ENDING;
        } else if (selectionMode == SelectionMode.ENDING && !currentMenu.stillInteracting)
        {
            SceneManager.LoadScene("FinalNight");
        }
    }

    private enum SelectionMode
    {
        MENTOR, QUESTION1, QUESTION2, QUESTION3, ENDING
    }
}
