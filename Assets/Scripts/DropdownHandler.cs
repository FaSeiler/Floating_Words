using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Manages the language dropdown.
/// </summary>
public class DropdownHandler : MonoBehaviour
{
    public string selected_language;
    public TMP_Dropdown dropdown;
    public AnchorCreator anchorCreator;
    void Start()
    {
        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("German");
        items.Add("Chinese");
        items.Add("Japanese");
        items.Add("Spanish");
        items.Add("French");

        foreach (var item in items)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });

        dropdown.value = (int)VocabularyDB.activeLanguageMode;
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        string OldLanguageMode = VocabularyDB.activeLanguageMode.ToString();
        switch (index)
        {
            case 0:
                selected_language = "de";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.german;
                break;
            case 1:
                selected_language = "cn";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.chinese;
                break;
            case 2:
                selected_language = "jp";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.japanese;
                break;
            case 3:
                selected_language = "es";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.spanish;
                break;
            case 4:
                selected_language = "fr";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.french;
                break;
        }
        anchorCreator.SetAllAnchorsText(OldLanguageMode);


        Debug.Log(selected_language);
    }
}
