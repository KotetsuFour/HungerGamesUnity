using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameRestricter : MonoBehaviour
{
    private TMP_InputField input;
    public static int nameLimit = 24;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(char c in input.text)
        {
            if (!char.IsLetter(c) && c != '-' && c != '_')
            {
                input.text = input.text.Replace(c, '-');
            }
        }
        if (input.text.Length > nameLimit)
        {
            input.text = input.text.Substring(0, nameLimit);
        }
    }
}
