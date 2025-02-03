using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{

    public static int district;
    public static Gender gender;
    public static string playerName;
    public static Reap_Status reapStatus;
    public static int age;
    public static Attitude attitude;
    public static Skill specialty;
    public static TributeData playerData;
    public static GameObject player;
    public static int playerIdx;

    public static int OLDER = 15;

    public static TributeData mentor;
    public static Color defaultMentorHair;
    public static Color defaultMentorSkin;
    public static Color defaultMentorEye;
    public static Color defaultMentorShirt;
    public static Color defaultMentorPants;

    public static bool allReaped;
    public static bool chooseAges;
    public static bool chooseGenders;
    public static bool chooseAttitudes;
    public static bool chooseAppearances;
    public static bool chooseSpecialties;
    public static int minAge = 12;
    public static int maxAge = 18;
    public static int numTributes;
    public static bool usingVictors;
    public static bool chooseDistricts;

    public static List<TributeData> tributesData;

    public static int maxSkillLevel = 5;

    public static int trainingSequenceNum;
    public static int allianceRequirement = 100;
    public static int oufitChoice;
    public static int trainingScore;

    public static Color[] hairColors = {
        new Color(0, 0, 0),
        new Color(107/255f, 78/255f, 64/255f),
        new Color(166/255f, 132/255f, 105/255f),
        new Color(164/255f, 108/255f, 71/255f),
        new Color(84/255f, 60/255f, 50/255f),
        new Color(184/255f, 65/255f, 49/255f),
        new Color(254/255f, 246/255f, 225/255f),
        new Color(202/255f, 164/255f, 120/255f),
    };
    public static Color[] skinColors = {
        new Color(240/255f, 199/255f, 177/255f),
        new Color(243/255f, 212/255f, 207/255f),
        new Color(255/255f, 208/255f, 188/255f),
        new Color(217/255f, 184/255f, 175/255f),
        new Color(217/255f, 164/255f, 148/255f),
        new Color(233/255f, 185/255f, 149/255f),
        new Color(245/255f, 175/255f, 149/255f),
        new Color(225/255f, 158/255f, 149/255f),
        new Color(218/255f, 164/255f, 136/255f),
        new Color(242/255f, 170/255f, 146/255f),
        new Color(236/255f, 196/255f, 184/255f),
        new Color(246/255f, 228/255f, 226/255f),
        new Color(238/255f, 170/255f, 131/255f),
        new Color(205/255f, 161/255f, 132/255f),
        new Color(147/255f, 97/255f, 74/255f),
        new Color(118/255f, 70/255f, 48/255f),
        new Color(117/255f, 57/255f, 21/255f),
        new Color(88/255f, 40/255f, 18/255f),
        new Color(179/255f, 106/255f, 51/255f),
        new Color(76/255f, 45/255f, 24/255f)
    };
    public static Color[] eyeColors = {
        new Color(78/255f, 96/255f, 163/255f),
        new Color(176/255f, 185/255f, 217/255f),
        new Color(60/255f, 141/255f, 142/255f),
        new Color(102/255f, 114/255f, 78/255f),
        new Color(123/255f, 92/255f, 51/255f),
        new Color(138/255f, 180/255f, 45/255f),
        new Color(40/255f, 41/255f, 120/255f),
        new Color(77/255f, 54/255f, 35/255f),
        new Color(159/255f, 174/255f, 112/255f),
        new Color(79/255f, 170/255f, 171/255f),
    };

    public static int[] maleTributeHairStyles =
    {
        1, 2, 3, 4, 5, 7, 8, 9, 11, 14, 16, 19, 20
    };
    public static int[] femaleTributeHairStyles =
    {
        1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 18, 19, 20, 21, 23, 24
    };

    public static Skill[] reapedSkills = {
                Skill.CHARM, Skill.CLIMBING, Skill.CAMOUFLAGE,
                Skill.SWIMMING, Skill.FIRSTAID, Skill.PLANTS,
                Skill.FIRES, Skill.ANIMALS, Skill.TRAPPING
    };
    public static Skill[] volunteeredSkills = {
                Skill.SWORDS, Skill.SPEARS, Skill.AXES,
                Skill.BOWS, Skill.KNIVES, Skill.FARMTOOLS,
                Skill.CLUBS, Skill.BRAWLING
    };

    public static string[] theyPronoun = { "They", "She", "He" };
    public static string[] themPronoun = { "Them", "Her", "Him" };
    public static string[] theirPronoun = { "Their", "Her", "His" };
    public static string[] theirsPronoun = { "Theirs", "Hers", "His" };
    public static Transform findDeepChild(Transform parent, string childName)
    {
        LinkedList<Transform> kids = new LinkedList<Transform>();
        for (int q = 0; q < parent.childCount; q++)
        {
            kids.AddLast(parent.GetChild(q));
        }
        while (kids.Count > 0)
        {
            Transform current = kids.First.Value;
            kids.RemoveFirst();
            if (current.name == childName || current.name + "(Clone)" == childName)
            {
                return current;
            }
            for (int q = 0; q < current.childCount; q++)
            {
                kids.AddLast(current.GetChild(q));
            }
        }
        return null;
    }
    public static void setHairSkinEye(GameObject human, TributeData data)
    {
        //Hair
        findDeepChild(human.transform, "Hair").GetComponent<HairSelector>().setHairStyle(data.appearance[3]);
        findDeepChild(human.transform, "Hair").GetComponent<HairSelector>().setHairColor(hairColors[data.appearance[0]]);
        //Skin
        Material[] mats = findDeepChild(human.transform, "model").GetComponent<SkinnedMeshRenderer>().materials;
        getMaterialByName(mats, "Skin").color = skinColors[data.appearance[1]];
        //Eyes
        getMaterialByName(mats, "Eyes").color = eyeColors[data.appearance[2]];
    }
    public static Material getMaterialByName(Material[] materials, string matName)
    {
        foreach (Material m in materials)
        {
            if (m.name.Replace(" ", "").Replace("1", "").Replace("(Instance)", "")
                == matName.Replace(" ", "").Replace("1", "").Replace("(Instance)", ""))
            {
                return m;
            }
        }
        Debug.Log(matName);
        return null;
    }
    public static void initializeTributes()
    {
        tributesData = new List<TributeData>();
        int numPerDistrict = numTributes / 12;
        for(int q = 0; q < numTributes; q++)
        {
            int dist = (q / numPerDistrict) + 1;
            Gender g;
            if (numPerDistrict % 2 == 1 && q % numPerDistrict == numPerDistrict - 1)
            {
                g = (Gender)Random.Range(1, 3);
            } else
            {
                g = q % 2 == 0 ? Gender.FEMALE : Gender.MALE;
            }
            TributeData data = new TributeData(g + "" + dist + "-" + q);
            data.district = dist;
            data.gender = g;
            data.dataIdx = q;

            if (allReaped)
            {
                data.reapStatus = Reap_Status.REAPED;
                int[] fighterChances = {
                    90, //1
                    90, //2
                    10, //3
                    80, //4
                    10, //5
                    10, //6
                    10, //7
                    5, //8
                    5, //9
                    10, //10
                    5, //11
                    2 //12
                };
                int selectionValue = Random.Range(0, 100);
                if (selectionValue < fighterChances[dist - 1])
                {
                    data.attitude = Attitude.FIGHTER;
                    data.age = Random.Range(16, 19);
                } else
                {
                    data.attitude = Attitude.RUNNER;
                    data.age = getReapedAge();
                }
            }
            else
            {
                int[] volunteerChances = { 
                    90, //1
                    90, //2
                    10, //3
                    80, //4
                    10, //5
                    10, //6
                    10, //7
                    5, //8
                    5, //9
                    10, //10
                    5, //11
                    2 //12
                };
                int[] gladiatorChances = {
                    80, //1
                    80, //2
                    3, //3
                    60, //4
                    3, //5
                    3, //6
                    5, //7
                    1, //8
                    1, //9
                    3, //10
                    1, //11
                    1 //12
                };
                int selectionValue = Random.Range(0, 100);
                if (selectionValue < volunteerChances[dist - 1])
                {
                    data.reapStatus = Reap_Status.VOLUNTEERED;
                    if (selectionValue < gladiatorChances[dist - 1])
                    {
                        data.attitude = Attitude.GLADIATOR;
                        data.age = Random.Range(16, 19);
                    }
                    else
                    {
                        data.attitude = Attitude.PROTECTOR;
                        data.age = getReapedAge();
                    }
                } else
                {
                    data.reapStatus = Reap_Status.REAPED;
                    data.age = getReapedAge();
                    if (Random.Range(0, 2) == 0)
                    {
                        data.attitude = Attitude.RUNNER;
                    }
                    else
                    {
                        data.attitude = Attitude.FIGHTER;
                    }
                }
            }
            tributesData.Add(data);
        }

        int append = ((district - 1) * numPerDistrict)
            + (numPerDistrict > 1 && gender == Gender.MALE ? 1 : 0);
        playerData = new TributeData(gender + "" + district + "-" + append);
        playerIdx = append;
        tributesData[append] = playerData;
        playerData.dataIdx = append;
        playerData.district = district;
        playerData.gender = gender;
        playerData.name = playerName;
        playerData.reapStatus = reapStatus;
        playerData.age = age;
        playerData.attitude = attitude;

        foreach (TributeData data in tributesData)
        {
            pickColors(data);
        }
    }
    private static void applyAttitudeBonuses(TributeData data)
    {
        if (data.attitude == Attitude.RUNNER)
        {
            data.avoidance += 20;
            data.intelligence += 2;
        }
        else if (data.attitude == Attitude.FIGHTER)
        {
            data.avoidance += 20;
            data.speed++;
        }
        else if (data.attitude == Attitude.GLADIATOR)
        {
            data.strength += 4;
            data.accuracy += 30;
            data.likability += 2;
        }
        else if (data.attitude == Attitude.PROTECTOR)
        {
            data.intelligence += 2;
            data.accuracy += 30;
            data.likability += 2;
        }
    }
    private static void applyAgeBonuses(TributeData data)
    {
        data.strength += (data.age - 16);
        data.accuracy += (5 * (data.age - 16));
        data.avoidance -= (5 * (data.age - 16));
        data.speed += data.age > 16 ? 1 : 0;
        data.intelligence += (data.age - 16);
    }
    private static void pickColors(TributeData data)
    {
        data.appearance = new int[] {
            Random.Range(0, hairColors.Length),
            Random.Range(0, skinColors.Length),
            Random.Range(0, eyeColors.Length), 0
        };
        if (data.gender == Gender.MALE)
        {
            data.appearance[3] = maleTributeHairStyles[Random.Range(0, maleTributeHairStyles.Length)];
        }
        else if (data.gender == Gender.FEMALE)
        {
            data.appearance[3] = femaleTributeHairStyles[Random.Range(0, femaleTributeHairStyles.Length)];
        }
    }
    private static void applyDistrictStatBonuses(TributeData data)
    {
        if (data.district == 1)
        {
            data.intelligence -= 2;
            data.strength++;
        }
        if (data.district == 2)
        {
            data.strength += 2;
            data.accuracy += 5;
        }
        if (data.district == 3)
        {
            data.intelligence += 3;
        }
        if (data.district == 5)
        {
            data.intelligence++;
        }
        if (data.district == 12)
        {
            data.strength--;
        }
    }
    private static void applyDistrictSkillBonuses(TributeData data)
    {
        if (data.district == 1)
        {
            data.increaseSkill(Skill.CHARM, 2);
        }
        if (data.district == 2)
        {
            data.increaseSkill(Skill.SWORDS, 1);
            data.increaseSkill(Skill.SPEARS, 1);
            data.increaseSkill(Skill.KNIVES, 1);
        }
        if (data.district == 4)
        {
            data.increaseSkill(Skill.SWIMMING, 3);
        }
        if (data.district == 7)
        {
            data.increaseSkill(Skill.AXES, 2);
            data.increaseSkill(Skill.CLIMBING, 1);
        }
        if (data.district == 9)
        {
            data.increaseSkill(Skill.PLANTS, 1);
        }
        if (data.district == 10)
        {
            data.increaseSkill(Skill.ANIMALS, 2);
        }
        if (data.district == 11)
        {
            data.increaseSkill(Skill.PLANTS, 2);
            data.increaseSkill(Skill.FARMTOOLS, 1);
        }
    }

    public static void finalizeTributeBaseStats()
    {
        foreach (TributeData data in tributesData)
        {
            if (data != playerData && data.skillsAcquired.Count == 0)
            {
                if (data.reapStatus == Reap_Status.REAPED)
                {
                    data.giveSpecialty(reapedSkills[Random.Range(0, reapedSkills.Length)]);
                } else if (data.reapStatus == Reap_Status.VOLUNTEERED)
                {
                    data.giveSpecialty(volunteeredSkills[Random.Range(0, volunteeredSkills.Length)]);
                }
            }
            applyAgeBonuses(data);
            applyAttitudeBonuses(data);
            applyDistrictStatBonuses(data);
            applyDistrictSkillBonuses(data);
        }
    }
    public static int wantJoinValue(TributeData seeker, TributeData candidate)
    {
        if (seeker.alliance.members.Contains(candidate)
            || seeker.alliance.members.Count + candidate.alliance.members.Count > numTributes / 4)
        {
            return -1;
        }
        int desire = (candidate.getSkillLevel(Skill.CHARM) * 2)
            + seeker.relationships[candidate.dataIdx];
        desire += candidate.strength + candidate.intelligence;
        if ((seeker.attitude == Attitude.GLADIATOR && candidate.attitude == Attitude.GLADIATOR)
            || (seeker.attitude == Attitude.PROTECTOR && candidate.attitude == Attitude.RUNNER))
        {
            desire += 50;
        } else if (seeker.attitude == Attitude.RUNNER && candidate.attitude == Attitude.PROTECTOR)
        {
            desire += 25;
        }
        return desire;
    }
    public static void paintLongSleeveJacket(Material[] materials, Color color)
    {
        paintMediumSleeveJacket(materials, color);
        getMaterialByName(materials, "LongSleeveRight").color = color;
        getMaterialByName(materials, "LongSleeveLeft").color = color;
    }
    public static void paintMediumSleeveJacket(Material[] materials, Color color)
    {
        paintShortSleeveJacket(materials, color);
        getMaterialByName(materials, "MediumSleeveRight").color = color;
        getMaterialByName(materials, "MediumSleeveLeft").color = color;
    }
    public static void paintShortSleeveJacket(Material[] materials, Color color)
    {
        paintSleevelessJacket(materials, color);
        getMaterialByName(materials, "ShortSleeveRight").color = color;
        getMaterialByName(materials, "ShortSleeveLeft").color = color;
    }
    public static void paintSleevelessJacket(Material[] materials, Color color)
    {
        getMaterialByName(materials, "RightSideTop").color = color;
        getMaterialByName(materials, "RightSideMiddle").color = color;
        getMaterialByName(materials, "LeftSideTop").color = color;
        getMaterialByName(materials, "LeftSideMiddle").color = color;
        getMaterialByName(materials, "BackMidUpperRight").color = color;
        getMaterialByName(materials, "BackMidUpperLeft").color = color;
        getMaterialByName(materials, "BackLineMiddle").color = color;
        getMaterialByName(materials, "RightShoulder").color = color;
        getMaterialByName(materials, "LeftShoulder").color = color;
        getMaterialByName(materials, "RightSideBottom").color = color;
        getMaterialByName(materials, "LeftSideBottom").color = color;
        getMaterialByName(materials, "BackTopRight").color = color;
        getMaterialByName(materials, "BackTopLeft").color = color;
        getMaterialByName(materials, "BackMiddleRight").color = color;
        getMaterialByName(materials, "BackMiddleLeft").color = color;
        getMaterialByName(materials, "BackLineTop").color = color;
        getMaterialByName(materials, "BackLineBottom").color = color;

    }

    public static void paintShoes(Material[] materials, Color color)
    {
        getMaterialByName(materials, "ShoeRight").color = color;
        getMaterialByName(materials, "ShoeLeft").color = color;
    }
    public static void paintHairSkinEye(Material[] materials, Color hair, Color skin, Color eye)
    {
        foreach (Material m in materials)
        {
            m.color = skin;
        }
        getMaterialByName(materials, "Hair").color = hair;
        getMaterialByName(materials, "RightEye").color = eye;
        getMaterialByName(materials, "LeftEye").color = eye;
    }
    public static void paintPants(Material[] materials, Color color)
    {
        paintShorts(materials, color);
        getMaterialByName(materials, "PantsRight").color = color;
        getMaterialByName(materials, "PantsLeft").color = color;
    }
    public static void paintShorts(Material[] materials, Color color)
    {
        paintUnderwear(materials, color);
        getMaterialByName(materials, "ShortsRight").color = color;
        getMaterialByName(materials, "ShortsLeft").color = color;
    }
    public static void paintUnderwear(Material[] materials, Color color)
    {
        getMaterialByName(materials, "GroinRight").color = color;
        getMaterialByName(materials, "GroinLeft").color = color;
        getMaterialByName(materials, "GroinMiddle").color = color;
        getMaterialByName(materials, "ButtRight").color = color;
        getMaterialByName(materials, "ButtLeft").color = color;
        getMaterialByName(materials, "ButtMiddle").color = color;
        getMaterialByName(materials, "InBetweenLegs").color = color;
        getMaterialByName(materials, "UnderwearRight").color = color;
        getMaterialByName(materials, "UnderwearLeft").color = color;
    }
    public static void paintLongSleeveShirt(Material[] materials, Color color)
    {
        paintMediumSleeveShirt(materials, color);
        getMaterialByName(materials, "LongSleeveRight").color = color;
        getMaterialByName(materials, "LongSleeveLeft").color = color;
    }
    public static void paintMediumSleeveShirt(Material[] materials, Color color)
    {
        paintShortSleeveShirt(materials, color);
        getMaterialByName(materials, "MediumSleeveRight").color = color;
        getMaterialByName(materials, "MediumSleeveLeft").color = color;
    }
    public static void paintShortSleeveShirt(Material[] materials, Color color)
    {
        paintSleevelessShirt(materials, color);
        getMaterialByName(materials, "ShortSleeveRight").color = color;
        getMaterialByName(materials, "ShortSleeveLeft").color = color;
    }
    public static void paintSleevelessShirt(Material[] materials, Color color)
    {
        paintBra(materials, color);
        getMaterialByName(materials, "RightShoulder").color = color;
        getMaterialByName(materials, "LeftShoulder").color = color;
        getMaterialByName(materials, "TopTie").color = color;
        getMaterialByName(materials, "BellyButton").color = color;
        getMaterialByName(materials, "TopRightMid").color = color;
        getMaterialByName(materials, "TopLeftMid").color = color;
        getMaterialByName(materials, "BellyRightMid").color = color;
        getMaterialByName(materials, "BellyLeftMid").color = color;
        getMaterialByName(materials, "RightSideBottom").color = color;
        getMaterialByName(materials, "LeftSideBottom").color = color;
        getMaterialByName(materials, "BackTopRight").color = color;
        getMaterialByName(materials, "BackTopLeft").color = color;
        getMaterialByName(materials, "BackMiddleRight").color = color;
        getMaterialByName(materials, "BackMiddleLeft").color = color;
        getMaterialByName(materials, "BackLineTop").color = color;
        getMaterialByName(materials, "BackLineBottom").color = color;

    }
    public static void paintBra(Material[] materials, Color color)
    {
        getMaterialByName(materials, "Tie").color = color;
        getMaterialByName(materials, "BelowTie").color = color;
        getMaterialByName(materials, "ChestRightMid").color = color;
        getMaterialByName(materials, "ChestLeftMid").color = color;
        getMaterialByName(materials, "AbRightMid").color = color;
        getMaterialByName(materials, "AbLeftMid").color = color;
        getMaterialByName(materials, "RightSideTop").color = color;
        getMaterialByName(materials, "RightSideMiddle").color = color;
        getMaterialByName(materials, "LeftSideTop").color = color;
        getMaterialByName(materials, "LeftSideMiddle").color = color;
        getMaterialByName(materials, "BackMidUpperRight").color = color;
        getMaterialByName(materials, "BackMidUpperLeft").color = color;
        getMaterialByName(materials, "BackLineMiddle").color = color;
    }
    public class TributeData
    {
        public string id;
        public string name;
        public int dataIdx;
        public Gender gender;
        public int age;
        public int district;
        public Reap_Status reapStatus;
        public Attitude attitude;
        public int[] appearance; //Hair, skin, eye, hairstyle
        public List<Skill> skillsAcquired;
        public List<int> skillLevels;

        public int strength;
        public int accuracy;
        public int avoidance;
        public int speed;
        public int intelligence;
        public int likability;

        public Alliance alliance;
        public int[] relationships;

        public Tribute actualObject;

        public TributeData(string id)
        {
            this.id = id;
            this.name = id;

            skillsAcquired = new List<Skill>();
            skillLevels = new List<int>();

            strength = 6;
            accuracy = 120;
            avoidance = 30;
            speed = 5;
            intelligence = 6;
            likability = 6;

            alliance = new Alliance();
            alliance.members.Add(this);
            relationships = new int[numTributes];
        }
        public void giveSpecialty(Skill skill)
        {
            skillsAcquired.Clear();
            skillLevels.Clear();
            skillsAcquired.Add(skill);
            skillLevels.Add(age <= 14 ? maxSkillLevel - (15 - age) : age <= 50 ? maxSkillLevel
                : maxSkillLevel - ((age - 50) / 10));
        }
        public void increaseSkill(Skill skill, int amount)
        {
            bool found = false;
            for (int q = 0; q < skillsAcquired.Count; q++)
            {
                if (skillsAcquired[q] == skill)
                {
                    skillLevels[q] = Mathf.Min(skillLevels[q] + amount, maxSkillLevel);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                skillsAcquired.Add(skill);
                skillLevels.Add(Mathf.Min(amount, maxSkillLevel));
            }
        }
        public int getSkillLevel(Skill skill)
        {
            for (int q = 0; q < skillsAcquired.Count; q++)
            {
                if (skillsAcquired[q] == skill)
                {
                    return skillLevels[q];
                }
            }
            return 0;
        }
        public void removeSkill(Skill skill)
        {
            for (int q = 0; q < skillsAcquired.Count; q++)
            {
                if (skillsAcquired[q] == skill)
                {
                    skillsAcquired.RemoveAt(q);
                    skillLevels.RemoveAt(q);
                }
            }
        }
    }
    private static int getReapedAge()
    {
        int[] pool = { 12, 13, 13, 14, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 16,
            17, 17, 17, 17, 17, 17, 18, 18, 18, 18, 18, 18, 18};
        return pool[Random.Range(0, pool.Length)];
    }
    public enum Gender
    {
        UNSPECIFIED, FEMALE, MALE
    }
    public enum Reap_Status
    {
        UNSPECIFIED, REAPED, VOLUNTEERED
    }
    public enum Attitude
    {
        UNSPECIFIED, RUNNER, FIGHTER, GLADIATOR, PROTECTOR
    }
    public enum Skill
    {
        CHARM, CLIMBING, CAMOUFLAGE, SWIMMING, FIRSTAID,
        PLANTS, FIRES, ANIMALS, TRAPPING,
        SWORDS, SPEARS, AXES, BOWS, KNIVES, FARMTOOLS, CLUBS, BRAWLING
    }
}
