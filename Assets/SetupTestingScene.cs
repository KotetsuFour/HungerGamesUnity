using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupTestingScene : MonoBehaviour
{
    [SerializeField] private BetterPlayerController player;
    [SerializeField] private NPCTribute olderMaleNPC;
    [SerializeField] private NPCTribute olderFemaleNPC;
    [SerializeField] private NPCTribute youngerMaleNPC;
    [SerializeField] private NPCTribute youngerFemaleNPC;
    [SerializeField] private GameObject dialogueBox;
    // Start is called before the first frame update
    void Awake()
    {
        StaticData.gender = StaticData.Gender.FEMALE;
        StaticData.numTributes = 24;
        StaticData.age = 16;
        StaticData.district = 12;
        StaticData.playerName = "Katniss";
        StaticData.attitude = StaticData.Attitude.PROTECTOR;

        StaticData.initializeTributes();

        for (int q = 0; q < StaticData.numTributes; q++)
        {
            StaticData.TributeData data = StaticData.tributesData[q];
            Tribute tr = null;
            if (data == StaticData.playerData)
            {
                tr = Instantiate(player);
                tr.GetComponent<BetterPlayerController>().setupForArena();
            }
            else if (data.gender == StaticData.Gender.FEMALE)
            {
                tr = Instantiate(olderFemaleNPC);
            }
            else
            {
                tr = Instantiate(olderMaleNPC);
            }
            tr.setData(data);
            float x = q * Mathf.PI / (StaticData.numTributes - 1);
            tr.transform.position = new Vector3(Mathf.Cos(x), 0, -Mathf.Sin(x)) * 60;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
