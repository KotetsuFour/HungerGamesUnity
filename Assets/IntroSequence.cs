using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] private GameObject beginningDisplay;
    private GameObject instantiatedBeginningDisplay;
    private CanvasGroup fadeIn;

    private float originalTime;
    private float timer;
    private SelectionMode selectionMode;


    // Start is called before the first frame update
    void Start()
    {
        instantiatedBeginningDisplay = Instantiate(beginningDisplay);
        instantiatedBeginningDisplay.SetActive(true);
        setTimer(1);
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Text1")
            .gameObject.SetActive(true);
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Text1")
            .GetComponent<CanvasGroup>().alpha = 0;
        selectionMode = SelectionMode.FADE_IN_TITLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (selectionMode == SelectionMode.FADE_IN_TITLE)
            {
                StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Text1")
                    .GetComponent<CanvasGroup>().alpha = (originalTime - timer) / originalTime;
                if (timer <= 0)
                {
                    setTimer(5);
                    selectionMode = SelectionMode.STAY_TITLE;
                }
            }
            else if (selectionMode == SelectionMode.STAY_TITLE)
            {
                if (timer <= 0)
                {
                    setTimer(1);
                    selectionMode = SelectionMode.FADE_OUT_TITLE;
                }
            }
            else if (selectionMode == SelectionMode.FADE_OUT_TITLE)
            {
                StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Text1")
                    .GetComponent<CanvasGroup>().alpha = timer / originalTime;
                if (timer <= 0)
                {
                    StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Text1")
                        .gameObject.SetActive(false);
                    Transform gamemode = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Gamemode");
                    gamemode.gameObject.SetActive(true);
                    fadeIn = gamemode.GetComponent<CanvasGroup>();
                    fadeIn.alpha = 0;
                    setTimer(1);

                    selectionMode = SelectionMode.GAMEMODE;
                }
            } else if (selectionMode == SelectionMode.GAMEMODE)
            {
                fadeIn.alpha = (originalTime - timer) / originalTime;
            }
        }
    }
    public void tellStandard()
    {
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Description")
            .GetComponent<TextMeshProUGUI>().text = "Enter a standard Hunger Games with " +
            "24 tributes.";
    }
    public void tell25th()
    {
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Description")
            .GetComponent<TextMeshProUGUI>().text = "In the 25th Games, tributes were chosen" +
            " by the people of their district. You can choose the other tributes' ages, " +
            "appearances, and specialties.";
    }
    public void tell50th()
    {
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Description")
            .GetComponent<TextMeshProUGUI>().text = "In the 50th Games, there were four tributes" +
            " chosen from each district instead of two. Enter a match with 48 tributes.";
    }
    public void tell75th()
    {
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Description")
            .GetComponent<TextMeshProUGUI>().text = "In the 75th Games, the tributes were chosen" +
            " from the existing pool of Victors. Use your save files from past games to compete " +
            "for their districts again or use the default Victors provided.";
    }
    public void tellCustom()
    {
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Description")
            .GetComponent<TextMeshProUGUI>().text = "Mix and match rules from Quarter Quells, with" +
            " even more advanced settings.";
    }
    public void tellSpectator()
    {
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Description")
            .GetComponent<TextMeshProUGUI>().text = "Be a Capitol spectator, observing and betting " +
            "on the Games with any set of default or custom rules.";
    }
    public void chooseStandard()
    {
        StaticData.numTributes = 24;
        SceneManager.LoadScene("Reaping");
    }
    public void choose25th()
    {
        StaticData.numTributes = 24;
        StaticData.allReaped = true;
        StaticData.chooseAges = true;
        StaticData.chooseAppearances = true;
        StaticData.chooseSpecialties = true;
        StaticData.reapStatus = StaticData.Reap_Status.REAPED;
        SceneManager.LoadScene("Reaping");
    }
    public void choose50th()
    {
        StaticData.numTributes = 48;
        SceneManager.LoadScene("Reaping");
    }
    public void choose75th()
    {
        StaticData.numTributes = 24;
        StaticData.usingVictors = true;
        //TODO load next scene
    }
    public void chooseCustom()
    {
        //TODO load next scene and figure stuff out there
    }
    public void chooseSpectator()
    {
        //TODO load next scene and figure stuff out there
    }
    private void setTimer(float time)
    {
        originalTime = time;
        timer = originalTime;
    }

    private enum SelectionMode
    {
        FADE_IN_TITLE, STAY_TITLE, FADE_OUT_TITLE, GAMEMODE
    }

}
