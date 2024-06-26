using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.AI.Navigation;

public class Arena : MonoBehaviour
{
    private static string mapsPath = "Assets/Arenas/";

    [SerializeField] private ArenaTile tile;
    [SerializeField] private GameObject pedestal;
    private ArenaTile[,] tileMap;
    private char[,] sectionMap;
    private List<GameObject> pedestals;
    private int width;
    private int height;
    public static float cellSize = 50;

    [SerializeField] private List<char> environmentKeys;
    [SerializeField] private List<Material> environments;
    private Dictionary<char, Material> environmentDictionary;

    [SerializeField] private List<char> decorationKeys;
    [SerializeField] private List<GameObject> decorations;
    private Dictionary<char, GameObject> decorationDictionary;

    [SerializeField] private List<GameObject> mutts;
    private List<ArenaTrap> traps;

    [SerializeField] private List<GameObject> animals;
    [SerializeField] private List<GameObject> plants;
    [SerializeField] private List<GameObject> inanimateObjects;
    [SerializeField] private List<GameObject> cornucopiaTypes;
    private GameObject cornucopia;
    private int supplySetup;
    private float feastProbability;

    public static float arenaTimer;
    public static float timePassed;
    public static bool timerPaused;
    public static Battle currentBattle;

    public void construct(string[] possibleMaps)
    {
        string mapFolderPath = mapsPath + possibleMaps[Random.Range(0, possibleMaps.Length)] + "/";

        environmentDictionary = new Dictionary<char, Material>();
        for (int q = 0; q < environmentKeys.Count; q++)
        {
            environmentDictionary.Add(environmentKeys[q], environments[q]);
        }

        decorationDictionary = new Dictionary<char, GameObject>();
        for (int q = 0; q < decorationKeys.Count; q++)
        {
            decorationDictionary.Add(decorationKeys[q], decorations[q]);
        }

        StreamReader readDetails = new StreamReader(mapFolderPath + "MapDetails.txt");

        StreamReader readTiles = new StreamReader(mapFolderPath + "TileMap.txt");
        List<List<char>> tileList = new List<List<char>>();
        while (readTiles.Peek() != -1)
        {
            string s = readTiles.ReadLine();
            int idx = tileList.Count;
            tileList.Add(new List<char>());
            for (int q = 0; q < s.Length; q++)
            {
                tileList[idx].Add(s[q]);
            }
        }
        readTiles.Close();

        StreamReader readSections = new StreamReader(mapFolderPath + "SectionMap.txt");
        List<List<char>> sectionList = new List<List<char>>();
        while (readSections.Peek() != -1)
        {
            string s = readSections.ReadLine();
            int idx = sectionList.Count;
            sectionList.Add(new List<char>());
            for (int q = 0; q < s.Length; q++)
            {
                sectionList[idx].Add(s[q]);
            }
        }
        readSections.Close();

        StreamReader readDecorations = new StreamReader(mapFolderPath + "DecorationMap.txt");
        List<List<char>> decorationList = new List<List<char>>();
        while (readDecorations.Peek() != -1)
        {
            string s = readDecorations.ReadLine();
            int idx = decorationList.Count;
            decorationList.Add(new List<char>());
            for (int q = 0; q < s.Length; q++)
            {
                decorationList[idx].Add(s[q]);
            }
        }
        readDecorations.Close();

        if (tileList.Count != sectionList.Count || tileList[0].Count != sectionList[0].Count)
        {
            Debug.Log("Tile and section maps are not the same size");
            return;
        }
        if (tileList.Count != decorationList.Count || tileList[0].Count != decorationList[0].Count)
        {
            Debug.Log("Tile and decoration maps are not the same size");
            return;
        }

        width = tileList[0].Count;
        height = tileList.Count;
        tileMap = new ArenaTile[width, height];

        for (int q = 0; q < tileList.Count; q++)
        {
            for (int w = 0; w < tileList[0].Count; w++)
            {
                ArenaTile toPlace = Instantiate(tile);
                char tileDetails = tileList[q][w];
                char tileSection = sectionList[q][w];
                char tileDecoration = decorationList[q][w];
                toPlace.environment = environmentDictionary[tileDetails];
                if (tileDetails == 'w')
                {
                    toPlace.setWater(2);
                    toPlace.gameObject.layer = 4;
                } else if (tileDetails == 'W')
                {
                    toPlace.setWater(1);
                    toPlace.gameObject.layer = 4;
                }
                toPlace.transform.position = new Vector3((w - (width / 2.0f)) * cellSize, 0, (q - (height / 2.0f)) * cellSize);

                if (decorationDictionary.ContainsKey(tileDecoration))
                {
                    GameObject deco = Instantiate(decorationDictionary[tileDecoration]);
                    deco.transform.position = toPlace.transform.position;
                    deco.transform.SetParent(toPlace.transform);
                }
                toPlace.gameObject.isStatic = true;
                tileMap[w, q] = toPlace;
                sectionMap[w, q] = tileSection;
            }
        }

        int pedestalArrangement = int.Parse(readDetails.ReadLine());
        float distanceFromCorn = 60 + StaticData.numTributes - 24;
        pedestals = new List<GameObject>();
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

        int cornType = int.Parse(readDetails.ReadLine());
        cornucopia = Instantiate(cornucopiaTypes[cornType]);
        cornucopia.transform.position = Vector3.zero;

        tileMap[0, 0].gameObject.AddComponent<NavMeshSurface>().BuildNavMesh();

        readDetails.Close();
    }
    private void semiCirclePedestals(float distanceFromCorn)
    {
        for (int q = 0; q < StaticData.numTributes; q++)
        {
            GameObject ped = Instantiate(pedestal);
            float x = q * Mathf.PI / (StaticData.numTributes - 1);
            ped.transform.position = new Vector3(Mathf.Cos(x), 0, -Mathf.Sin(x)) * distanceFromCorn;
            pedestals.Add(ped);
        }
    }
    private void fullCirclePedestals(float distanceFromCorn)
    {
        for (int q = 0; q < StaticData.numTributes; q++)
        {
            GameObject ped = Instantiate(pedestal);
            float x = q * Mathf.PI * 2 / StaticData.numTributes;
            ped.transform.position = new Vector3(Mathf.Cos(x), 0, -Mathf.Sin(x)) * distanceFromCorn;
            pedestals.Add(ped);
        }
    }
    private void separatedCirclePedestals(float distanceFromCorn)
    {
        //TODO
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
