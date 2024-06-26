using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupTestingScene : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject dialogueBox;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("setup");
        StaticData.TributeData data = new StaticData.TributeData("TEST");
        StaticData.playerData = data;
        player.GetComponent<PlayerController>().tributeData = data;
        StaticData.player = player;

        int num = 24;
        for (int q = 0; q < num; q++)
        {
            GameObject person = Instantiate(npc);
            float x = q * Mathf.PI / (num - 1);
            person.transform.position = new Vector3(Mathf.Cos(x), 0, -Mathf.Sin(x)) * 60;

            person.AddComponent<Talker>().setDialogue(new List<string>(new string[] { "Blah" }),
                new List<string>(new string[] { "Blah" }),
                dialogueBox);
            person.layer = 6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
