using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField] private GameObject beginningDisplay;
    [SerializeField] private GameObject routePicker;
    private GameObject instantiatedBeginningDisplay;
    private CanvasGroup fadeIn;

    private float originalTime;
    private float timer;
    private SelectionMode selectionMode;

    private TMP_Dropdown[] currentDropdowns;
    private Button currentConfirm;
    private TMP_InputField currentField;

    // Start is called before the first frame update
    void Start()
    {
        instantiatedBeginningDisplay = Instantiate(beginningDisplay);
        instantiatedBeginningDisplay.SetActive(true);

        /*
        setTimer(1);
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Text1")
            .gameObject.SetActive(true);
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Text1")
            .GetComponent<CanvasGroup>().alpha = 0;
        selectionMode = SelectionMode.FADE_IN_TITLE;
        */
        Transform prompt1 = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Prompt1");
        prompt1.gameObject.SetActive(true);
        fadeIn = prompt1.GetComponent<CanvasGroup>();
        fadeIn.alpha = 0;
        setTimer(1);
        currentDropdowns = new TMP_Dropdown[] {
                        StaticData.findDeepChild(prompt1, "District").GetComponent<TMP_Dropdown>()
                    };
        currentConfirm = StaticData.findDeepChild(prompt1, "Confirm").GetComponent<Button>();

        selectionMode = SelectionMode.PROMPT1;
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
            } else if (selectionMode == SelectionMode.STAY_TITLE)
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
                    Transform prompt1 = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Prompt1");
                    prompt1.gameObject.SetActive(true);
                    fadeIn = prompt1.GetComponent<CanvasGroup>();
                    fadeIn.alpha = 0;
                    setTimer(1);
                    currentDropdowns = new TMP_Dropdown[] {
                        StaticData.findDeepChild(prompt1, "District").GetComponent<TMP_Dropdown>()
                    };
                    currentConfirm = StaticData.findDeepChild(prompt1, "Confirm").GetComponent<Button>();

                    selectionMode = SelectionMode.PROMPT1;
                }
            } else if (selectionMode == SelectionMode.PROMPT1 || selectionMode == SelectionMode.PROMPT2
                || selectionMode == SelectionMode.PROMPT3 || selectionMode == SelectionMode.PERSONAL_PROMPT
                || selectionMode == SelectionMode.NAME_PROMPT)
            {
                fadeIn.alpha = (originalTime - timer) / originalTime;
            }
        }
    }

    public void setDistrict(int dist)
    {
        StaticData.district = dist;
    }
    public void confirmDistrict()
    {
        if (StaticData.district != 0)
        {
            confirmData();
            Transform prompt2 = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Prompt2");
            prompt2.gameObject.SetActive(true);
            fadeIn = prompt2.GetComponent<CanvasGroup>();
            fadeIn.alpha = 0;
            setTimer(1);
            currentDropdowns = new TMP_Dropdown[] {
                        StaticData.findDeepChild(prompt2, "Selection").GetComponent<TMP_Dropdown>()
                    };
            currentConfirm = StaticData.findDeepChild(prompt2, "Confirm").GetComponent<Button>();
            if (StaticData.allReaped)
            {
                StaticData.findDeepChild(prompt2.transform, "TheDayI").GetComponent<TextMeshProUGUI>()
                    .text = "It's the day they voted to enter me in the Annual Hunger Games.";
                foreach(TMP_Dropdown drop in currentDropdowns)
                {
                    drop.gameObject.SetActive(false);
                }
                currentConfirm.transform.GetChild(0).GetComponent<TextMeshProUGUI>()
                    .text = "Next";
            }

            selectionMode = SelectionMode.PROMPT2;
        }
    }
    public void setSelection(int select)
    {
        StaticData.reapStatus = (StaticData.Reap_Status) select;
    }
    public void confirmSelection()
    {
        if (StaticData.reapStatus != StaticData.Reap_Status.UNSPECIFIED)
        {
            confirmData();
            Transform prompt3 = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Prompt3");
            prompt3.gameObject.SetActive(true);
            fadeIn = prompt3.GetComponent<CanvasGroup>();
            fadeIn.alpha = 0;
            setTimer(1);
            currentDropdowns = new TMP_Dropdown[] {
                        StaticData.findDeepChild(prompt3, "Age").GetComponent<TMP_Dropdown>(),
                        StaticData.findDeepChild(prompt3, "Gender").GetComponent<TMP_Dropdown>()
                    };
            currentConfirm = StaticData.findDeepChild(prompt3, "Confirm").GetComponent<Button>();

            selectionMode = SelectionMode.PROMPT3;
        }
    }
    public void setAge(int age)
    {
        StaticData.age = age + 11;
    }
    public void setGender(int gender)
    {
        StaticData.gender = (StaticData.Gender)gender;
    }
    public void confirmAgeAndGender()
    {
        if (StaticData.age >= 12 && StaticData.gender != StaticData.Gender.UNSPECIFIED)
        {
            confirmData();
            Transform personal = null;
            if (StaticData.reapStatus == StaticData.Reap_Status.REAPED)
            {
                personal = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "ReapedPrompt");
                currentDropdowns = new TMP_Dropdown[] {
                        StaticData.findDeepChild(personal, "Resilience").GetComponent<TMP_Dropdown>()
                };
            }
            else if (StaticData.reapStatus == StaticData.Reap_Status.VOLUNTEERED)
            {
                personal = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "VolunteeredPrompt");
                currentDropdowns = new TMP_Dropdown[] {
                        StaticData.findDeepChild(personal, "Motive").GetComponent<TMP_Dropdown>()
                };
            }
            personal.gameObject.SetActive(true);
            fadeIn = personal.GetComponent<CanvasGroup>();
            fadeIn.alpha = 0;
            setTimer(1);
            currentConfirm = StaticData.findDeepChild(personal, "Confirm").GetComponent<Button>();

            selectionMode = SelectionMode.PERSONAL_PROMPT;
        }
    }
    public void setResilience(int val)
    {
        StaticData.attitude = (StaticData.Attitude)val;
    }
    public void setMotive(int val)
    {
        if (val != 0)
        {
            val += 2;
        }
        StaticData.attitude = (StaticData.Attitude)val;
    }
    public void confirmAttitude()
    {
        if (StaticData.attitude != StaticData.Attitude.UNSPECIFIED)
        {
            confirmData();
            Transform namePrompt = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "NamePrompt");
            namePrompt.gameObject.SetActive(true);
            fadeIn = namePrompt.GetComponent<CanvasGroup>();
            fadeIn.alpha = 0;
            setTimer(1);
            currentDropdowns = null;
            currentField = StaticData.findDeepChild(namePrompt.transform, "Name").GetComponent<TMP_InputField>();
            currentConfirm = StaticData.findDeepChild(namePrompt, "Confirm").GetComponent<Button>();
            StaticData.findDeepChild(namePrompt.transform, "TributeOf")
                .GetComponent<TextMeshProUGUI>().text = ", tribute of District " + StaticData.district;

            selectionMode = SelectionMode.NAME_PROMPT;
        }
    }
    public void confirmName()
    {
        if (currentField.text.Length > 0)
        {
            confirmData();
            StaticData.playerName = currentField.text;
            Debug.Log(StaticData.playerName + "\n" + StaticData.gender + " " + StaticData.district
                + "\n" + StaticData.reapStatus + " " + StaticData.age + "\n" + StaticData.attitude);

            Destroy(instantiatedBeginningDisplay);
            instantiatedBeginningDisplay = Instantiate(routePicker);
            instantiatedBeginningDisplay.SetActive(true);
            Transform routePrompt = StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Options");
            routePrompt.gameObject.SetActive(true);
            fadeIn = routePrompt.GetComponent<CanvasGroup>();
            fadeIn.alpha = 0;
            setTimer(1);
        }
    }
    public void tellCapitol()
    {
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Explanation")
            .GetComponent<TextMeshProUGUI>().text = "Receive the full Hunger Games experience " +
            "as all tributes build their stats in training, interviews, etc. before entering the " +
            "arena.";
    }
    public void tellArena()
    {
        StaticData.findDeepChild(instantiatedBeginningDisplay.transform, "Explanation")
            .GetComponent<TextMeshProUGUI>().text = "Skip the Capitol experience and go straight " +
            "to the arena, where all tributes will compete with their base stats.";
    }
    public void gotoCapitol()
    {
        StaticData.initializeTributes();
        SceneManager.LoadScene("Train");
    }
    public void gotoArena()
    {
        //TODO enter specialty/appearance/edit-tributes menu before switching scenes
    }

    private void setTimer(float time)
    {
        originalTime = time;
        timer = originalTime;
    }
    private void confirmData()
    {
        if (currentDropdowns != null)
        {
            foreach (TMP_Dropdown drop in currentDropdowns)
            {
                drop.interactable = false;
            }
        }
        if (currentField != null)
        {
            currentField.interactable = false;
        }
        currentConfirm.interactable = false;
    }

    private enum SelectionMode
    {
        FADE_IN_TITLE, STAY_TITLE, FADE_OUT_TITLE, PROMPT1, PROMPT2, PROMPT3, PERSONAL_PROMPT,
        NAME_PROMPT
    }
}
