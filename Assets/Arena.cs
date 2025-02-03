using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private Terrain map;
    [SerializeField] private float tileSize;
    [SerializeField] private GameObject pedestal;
    [SerializeField] private float averageTemperatureDegrees;
    [SerializeField] private int pedestalArrangement;
    private List<GameObject> pedestals;
    [SerializeField] private GameObject cornucopia;
    [SerializeField] private float feastProbability;
    [SerializeField] private LayerMask traversable;

    private Vector3 centerSky;
    private Vector3 center;

    [SerializeField] private NPCTribute olderMaleNPC;
    [SerializeField] private NPCTribute olderFemaleNPC;
    [SerializeField] private NPCTribute youngerMaleNPC;
    [SerializeField] private NPCTribute youngerFemaleNPC;
    [SerializeField] private BetterPlayerController olderMalePlayer;
    [SerializeField] private BetterPlayerController olderFemalePlayer;
    [SerializeField] private BetterPlayerController youngerMalePlayer;
    [SerializeField] private BetterPlayerController youngerFemalePlayer;

    public static float arenaTimer;
    public static float timePassed;
    public static bool timerPaused;
    public static Battle currentBattle;

    void Start()
    {
        //TODO cinematic opening
        construct();
    }

    public void construct()
    {
        pedestals = new List<GameObject>();
        centerSky = new Vector3(map.terrainData.size.x / 2, map.terrainData.size.y, map.terrainData.size.z / 2);
        RaycastHit hit;
        Physics.Raycast(centerSky, Vector3.down, out hit, float.MaxValue, traversable);
        center = hit.point;

        float distanceFromCorn = 60 + StaticData.numTributes - 24;
        if (pedestalArrangement == 0)
        {
            semiCirclePedestals(distanceFromCorn);
        } else if (pedestalArrangement == 1)
        {
            fullCirclePedestals(distanceFromCorn);
        } else if (pedestalArrangement == 2)
        {
            separatedCirclePedestals(distanceFromCorn);
        }

        int[] order = new int[StaticData.numTributes];
        //Randomize
        for (int q = 0; q < order.Length; q++)
        {
            order[q] = q;
        }
        for (int q = 0; q < order.Length; q++)
        {
            int replace = Random.Range(0, order.Length);
            int temp = order[q];
            order[q] = order[replace];
            order[replace] = temp;
        }
        for (int q = 0; q < pedestals.Count; q++)
        {
            StaticData.TributeData data = StaticData.tributesData[order[q]];
            if (data == StaticData.playerData)
            {
                if (data.gender == StaticData.Gender.MALE)
                {
                    if (data.age >= StaticData.OLDER)
                    {
                        setupTribute(olderMalePlayer, q).setData(data);
                    }
                    else
                    {
                        setupTribute(youngerMalePlayer, q).setData(data);
                    }
                }
                else if (data.gender == StaticData.Gender.FEMALE)
                {
                    if (data.age >= StaticData.OLDER)
                    {
                        setupTribute(olderFemalePlayer, q).setData(data);
                    }
                    else
                    {
                        setupTribute(youngerFemalePlayer, q).setData(data);
                    }
                }
            }
            else
            {
                if (data.gender == StaticData.Gender.MALE)
                {
                    if (data.age >= StaticData.OLDER)
                    {
                        setupTribute(olderMaleNPC, q).setData(data);
                    }
                    else
                    {
                        setupTribute(youngerMaleNPC, q).setData(data);
                    }
                }
                else if (data.gender == StaticData.Gender.FEMALE)
                {
                    if (data.age >= StaticData.OLDER)
                    {
                        setupTribute(olderFemaleNPC, q).setData(data);
                    }
                    else
                    {
                        setupTribute(youngerFemaleNPC, q).setData(data);
                    }
                }
            }
        }
        if (StaticData.playerData != null)
        {
            StaticData.playerData.actualObject.GetComponent<BetterPlayerController>().setupForArena();
        }
    }
    private void semiCirclePedestals(float distanceFromCorn)
    {
        for (int q = 0; q < StaticData.numTributes; q++)
        {
            float x = q * Mathf.PI / (StaticData.numTributes - 1);
            setPedestal(Mathf.Cos(x), -Mathf.Sin(x), distanceFromCorn);
        }
    }
    private void fullCirclePedestals(float distanceFromCorn)
    {
        for (int q = 0; q < StaticData.numTributes; q++)
        {
            float x = q * Mathf.PI * 2 / StaticData.numTributes;
            setPedestal(Mathf.Cos(x), -Mathf.Sin(x), distanceFromCorn);
        }
    }
    private void separatedCirclePedestals(float distanceFromCorn)
    {
        int points = StaticData.numTributes + 12;
        for (int q = 0; q < points; q++)
        {
            if (q % 12 == 0)
            {
                continue;
            }
            float x = q * Mathf.PI * 2 / (StaticData.numTributes + points);
            setPedestal(Mathf.Cos(x), -Mathf.Sin(x), distanceFromCorn);
        }
    }
    private void setPedestal(float x, float z, float distanceFromCorn)
    {
        RaycastHit hit;
        Physics.Raycast((new Vector3(x, 0, z) * distanceFromCorn) + centerSky, Vector3.down, out hit, float.MaxValue, traversable);
        pedestals.Add(Instantiate(pedestal, hit.point, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerPaused)
        {
            timePassed = Time.deltaTime;
            arenaTimer += timePassed;
        } else
        {
            timePassed = 0;
            if (currentBattle != null)
            {
                if (currentBattle.containsPlayer)
                {
                    //TODO
                }
                else
                {

                }
            }
        }
    }

    private Tribute setupTribute(Tribute prefab, int pedestalNum)
    {
        Vector3 stage = StaticData.findDeepChild(pedestals[pedestalNum].transform, "Stage").position;
        Quaternion look = Quaternion.LookRotation(new Vector3(center.x, 0, center.z) - new Vector3(stage.x, 0, stage.z));
        return Instantiate(prefab, stage, look);
    }
    public class ArenaTrap
    {
        public char[] mapSections;
        public TrapType type;
        public TrapCondition condition;
        public int conditionInt;

        public GameObject mutt;

        public ArenaTrap(char[] mapSections, TrapType type, TrapCondition condition,
            int conditionInt, GameObject mutt)
        {
            this.mapSections = mapSections;
            this.type = type;
            this.mutt = mutt;
            this.condition = condition;
            this.conditionInt = conditionInt;
        }
    }
    public enum TrapType
    {
        MUTT, ATMOSPHERE, MAP_MORPH
    }
    public enum TrapCondition
    {
        SCHEDULED, ONE_TIME_SCHEDULED, PEACE_TIME
    }
}
