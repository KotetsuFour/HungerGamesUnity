using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrainingSetup : MonoBehaviour
{
    [SerializeField] private GameObject olderMalePlayer;
    [SerializeField] private GameObject olderFemalePlayer;
    [SerializeField] private GameObject youngerMalePlayer;
    [SerializeField] private GameObject youngerFemalePlayer;
    [SerializeField] private GameObject olderMaleNPC;
    [SerializeField] private GameObject olderFemaleNPC;
    [SerializeField] private GameObject youngerMaleNPC;
    [SerializeField] private GameObject youngerFemaleNPC;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform mentorPos;
    [SerializeField] private Transform escortPos;
    [SerializeField] private Transform avoxPos;

    [SerializeField] private Color escortHair;
    [SerializeField] private Color escortSkin;
    [SerializeField] private Color escortJacket;
    [SerializeField] private Color escortPants;
    [SerializeField] private Color escortShoes;
    [SerializeField] private Color avoxHair;
    [SerializeField] private Color avoxClothes;

    private GameObject player;
    private GameObject mentor;
    private GameObject escort;
    private GameObject avox;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject selectScreen;
    [SerializeField] private GameObject fadeScreen;

    [SerializeField] private Teleporter door;

    [SerializeField] private GameObject spear;
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject scythe;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject club;
    [SerializeField] private GameObject climbingCoach;
    [SerializeField] private GameObject camouflageCoach;
    [SerializeField] private GameObject firstAidCoach;
    [SerializeField] private GameObject plantsCoach;
    [SerializeField] private GameObject firesCoach;
    [SerializeField] private GameObject animalsCoach;
    [SerializeField] private GameObject trappingCoach;
    [SerializeField] private GameObject swimmingCoach;
    [SerializeField] private GameObject spearsCoach;
    [SerializeField] private GameObject axesCoach;
    [SerializeField] private GameObject bowsCoach;
    [SerializeField] private GameObject knivesCoach;
    [SerializeField] private GameObject farmToolsCoach;
    [SerializeField] private GameObject swordsCoach;
    [SerializeField] private GameObject clubsCoach;
    [SerializeField] private GameObject brawlingCoach;
    private GameObject[] teachers;

    [SerializeField] private GameObject pickPresentation;
    [SerializeField] private GameObject showScores;
    [SerializeField] private TextMeshProUGUI scoreLabel;

    // Start is called before the first frame update
    void Start()
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
        paintTrainingClothes(player, StaticData.playerData);
        moveTo(player, playerPos);

        setMentor();
        moveTo(mentor, mentorPos);
        //TODO add tips

        setEscort();
        moveTo(escort, escortPos);
        escort.AddComponent<Talker>().setDialogue(new List<string>(new string[] {
            "Move along now. You don't want to be late for training!"
        }), new List<string>(new string[] {
            "Escort"
        }), dialogueBox);
        escort.layer = 6;

        setAvox();
        moveTo(avox, avoxPos);
        avox.AddComponent<Talker>().setDialogue(new List<string>(new string[] {
            "..."
        }), new List<string>(new string[] {
            "Avox"
        }), dialogueBox);
        avox.layer = 6;
        avox.AddComponent<Tracker>().setTarget(player.transform);

        door.setPlayer(player);

        teachers = new GameObject[] { climbingCoach, camouflageCoach, firstAidCoach, plantsCoach,
        firesCoach, animalsCoach, trappingCoach, swimmingCoach, spearsCoach, axesCoach, bowsCoach,
        knivesCoach, farmToolsCoach, swordsCoach, clubsCoach, brawlingCoach};
        foreach(GameObject teacher in teachers)
        {
            teacher.GetComponent<Coach>().contruct();
            teacher.GetComponent<Coach>().setup = this;
            teacher.layer = 6;
            paintCoach(teacher);
        }

        arm(swordsCoach, Instantiate(sword));
        arm(spearsCoach, Instantiate(spear));
        arm(axesCoach, Instantiate(axe));
        arm(bowsCoach, Instantiate(bow));
        arm(knivesCoach, Instantiate(knife));
        arm(farmToolsCoach, Instantiate(scythe));
        arm(clubsCoach, Instantiate(club));

        setTrainees();

        StaticData.trainingSequenceNum = 9;
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
        Material[] materials = StaticData.findDeepChild(mentor.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, StaticData.defaultMentorHair,
            StaticData.defaultMentorSkin, StaticData.defaultMentorEye);
        StaticData.paintMediumSleeveShirt(materials, StaticData.defaultMentorShirt);
        StaticData.paintPants(materials, StaticData.defaultMentorPants);
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
    private void paintTrainingClothes(GameObject tribute, StaticData.TributeData data)
    {
        Material[] materials = StaticData.findDeepChild(tribute.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, StaticData.hairColors[data.appearance[0]],
            StaticData.skinColors[data.appearance[1]], StaticData.eyeColors[data.appearance[2]]);
        StaticData.paintShortSleeveShirt(materials, Color.grey);
        StaticData.paintSleevelessShirt(materials, Color.black);
        StaticData.paintPants(materials, Color.black);
        StaticData.paintShoes(materials, Color.black);
    }
    private void setAvox()
    {
        avox = Instantiate(olderFemaleNPC);
        Material[] materials = StaticData.findDeepChild(avox.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, avoxHair,
            escortSkin, Color.black);
        StaticData.paintPants(materials, avoxClothes);
        StaticData.paintShoes(materials, Color.white);
        StaticData.paintSleevelessShirt(materials, avoxClothes);
    }
    private void paintCoach(GameObject coach)
    {
        Material[] materials = StaticData.findDeepChild(coach.transform, "Mesh")
            .GetComponent<SkinnedMeshRenderer>().materials;
        StaticData.paintHairSkinEye(materials, StaticData.hairColors[Random.Range(0, StaticData.hairColors.Length)],
            StaticData.skinColors[Random.Range(0, StaticData.skinColors.Length)],
            StaticData.eyeColors[Random.Range(0, StaticData.eyeColors.Length)]);
        StaticData.paintPants(materials, new Color(138/255f, 99/255f, 36/255f));
        StaticData.paintShoes(materials, Color.black);
        StaticData.paintLongSleeveShirt(materials, new Color(138 / 255f, 99 / 255f, 36 / 255f));
    }
    private void arm(GameObject holder, GameObject weapon)
    {
        weapon.layer = 0;
        Transform hand = StaticData.findDeepChild(holder.transform, "RightHand");
        weapon.transform.SetParent(hand);
        weapon.transform.localPosition = hand.localPosition;
        weapon.transform.rotation = hand.rotation;
    }

    public void setTrainees()
    {
        foreach(GameObject teacher in teachers)
        {
            List<GameObject> trainees = teacher.GetComponent<Coach>().students;
            foreach(GameObject student in trainees)
            {
                Destroy(student);
            }
            trainees.Clear();
        }
        foreach (StaticData.TributeData data in StaticData.tributesData)
        {
            if (data != StaticData.playerData)
            {
                GameObject tribute = null;
                if (data.gender == StaticData.Gender.FEMALE)
                {
                    if (data.age >= 15)
                    {
                        tribute = Instantiate(olderFemaleNPC);
                    } else
                    {
                        tribute = Instantiate(youngerFemaleNPC);
                    }
                } else if (data.gender == StaticData.Gender.MALE)
                {
                    if (data.age >= 15)
                    {
                        tribute = Instantiate(olderMaleNPC);
                    }
                    else
                    {
                        tribute = Instantiate(youngerMaleNPC);
                    }
                }
                TrainingTribute train = tribute.AddComponent<TrainingTribute>();
                tribute.layer = 6;
                train.setup = this;
                train.interactType = Interactable.InteractType.TRIBUTE;
                train.setEssentials(selectScreen, fadeScreen, data);
                paintTrainingClothes(tribute, data);
                GameObject teacher = teachers[Random.Range(0, teachers.Length)];
                teacher.GetComponent<Coach>().addStudent(tribute);
            }
        }
    }
    public void traineesAct(List<GameObject> students)
    {
        foreach (GameObject t in teachers)
        {
            foreach (GameObject s in t.GetComponent<Coach>().students)
            {
                StaticData.TributeData sData = s.GetComponent<TrainingTribute>().data;
                if (Random.Range(0, 2) == 0) {
                    sData.increaseSkill(t.GetComponent<Coach>()
                        .taughtSkill, 1);
                }
                foreach (GameObject sec in t.GetComponent<Coach>().students)
                {
                    StaticData.TributeData secData = sec.GetComponent<TrainingTribute>().data;
                    sData.relationships[secData.dataIdx]++;
                }
            }
        }
        if (students != null)
        {
            foreach (GameObject s in students)
            {
                s.GetComponent<TrainingTribute>().data.relationships[StaticData.playerIdx]++;
            }
        }
        setTrainees();
        player.GetComponent<PlayerController>().liberate();
        StaticData.trainingSequenceNum--;
        if (StaticData.trainingSequenceNum % 3 == 0)
        {
            moveTo(player, playerPos);
            List<StaticData.TributeData> askers = new List<StaticData.TributeData>();
            foreach (StaticData.TributeData data in StaticData.tributesData)
            {
                if (data == StaticData.playerData)
                {
                    continue;
                }
                foreach (StaticData.TributeData cand in StaticData.tributesData)
                {
                    int desire = StaticData.wantJoinValue(data, cand);

                    if (desire > Random.Range(0, StaticData.allianceRequirement * 2))
                    {
                        if (StaticData.playerData.alliance.members.Contains(cand))
                        {
                            askers.Add(data);
                        }
                        else if (!data.alliance.members.Contains(cand))
                        {
                            int counterDesire = StaticData.wantJoinValue(cand, data);
                            if (counterDesire > Random.Range(0, StaticData.allianceRequirement))
                            {
                                data.alliance.join(cand);
                                break;
                            }
                        }
                    }
                }
            }
            GetComponent<AskAlliance>().setAskers(askers);
            GetComponent<AskAlliance>().menu(player.GetComponent<BetterPlayerController>());
            player.GetComponent<PlayerController>().hijack(GetComponent<AskAlliance>());

            if (StaticData.trainingSequenceNum <= 0 && askers.Count == 0)
            {
                GetComponent<TrainingSetup>().printAlliances();
                GetComponent<TrainingSetup>().setupPresentSkills();
            }
        }
        else
        {
            door.GetComponent<Teleporter>().menu(player.GetComponent<BetterPlayerController>());
        }
    }
    public void printAlliances()
    {
        List<Alliance> alliances = new List<Alliance>();
        for (int q = 0; q < StaticData.tributesData.Count; q++)
        {
            StaticData.TributeData data = StaticData.tributesData[q];
            if (!alliances.Contains(data.alliance))
            {
                alliances.Add(data.alliance);
            }
        }
        for (int q = 0; q < alliances.Count; q++)
        {
            foreach (StaticData.TributeData data in alliances[q].members)
            {
                Debug.Log(q + " - " + data.name + " - " + data.attitude);
            }
        }
    }
    public void setupPresentSkills()
    {
        player.GetComponent<PlayerController>().enabled = false;
        pickPresentation.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void presentSkill(int skill)
    {
        int[] scores = new int[StaticData.numTributes];
        for (int q = 0; q < StaticData.tributesData.Count; q++)
        {
            StaticData.TributeData data = StaticData.tributesData[q];
            StaticData.Skill presentedSkill = StaticData.Skill.CHARM;
            if (data == StaticData.playerData)
            {
                presentedSkill = (StaticData.Skill)skill;
            } else if (data.reapStatus == StaticData.Reap_Status.VOLUNTEERED)
            {
                int level = 0;
                for (int w = 0; w < data.skillsAcquired.Count; w++)
                {
                    if (data.skillLevels[w] > level)
                    {
                        presentedSkill = data.skillsAcquired[w];
                        level = data.skillLevels[w];
                    }
                }
            } else
            {
                if (data.skillsAcquired.Count != 0)
                {
                    presentedSkill = data.skillsAcquired[Random.Range(0, data.skillsAcquired.Count)];
                }
            }
            float score = data.getSkillLevel(presentedSkill);
            if (presentedSkill >= StaticData.Skill.SWORDS)
            {
                score += data.strength / 2f;
            } else if (presentedSkill > StaticData.Skill.CHARM)
            {
                score += data.intelligence / 4f;
            }
            if (score >= 9)
            {
                score -= Random.Range(0, 4);
            }

            scores[q] = Mathf.Clamp(Mathf.RoundToInt(score), 1, 11);
            if (Random.Range(0, 1800) == 0)
            {
                scores[q]++;
            }
            data.likability += scores[q] - 6;
        }
        StaticData.trainingScore = scores[StaticData.playerIdx];
        presentScores(scores, (StaticData.Skill)skill);
    }
    public void presentScores(int[] scores, StaticData.Skill skill)
    {
        pickPresentation.SetActive(false);
        showScores.SetActive(true);
        StaticData.findDeepChild(showScores.transform, "Title").GetComponent<TextMeshProUGUI>()
            .text = "You presented " + skill + ". At night, the scores are announced...";
        for (int q = 0; q < scores.Length; q++)
        {
            StaticData.TributeData data = StaticData.tributesData[q];
            TextMeshProUGUI label = Instantiate(scoreLabel,
                StaticData.findDeepChild(showScores.transform, "Content"));
            label.text = data.name + " (" + data.id + ") - District " + data.district + " - Score: " + scores[q];
        }
    }
    public void nextScene()
    {
        SceneManager.LoadScene("Interviews");
    }
}
