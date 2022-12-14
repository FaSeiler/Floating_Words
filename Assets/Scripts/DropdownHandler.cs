using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public string selected_language;
    public TMP_Dropdown dropdown;


    void Start()
    {
        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("English");
        items.Add("German");
        items.Add("Chinese");
        items.Add("Japanese");
        items.Add("Spanish");
        items.Add("French");

        foreach (var item in items)
        {
            //dropdown.options.Add(new Dropdown.OptionData() { text = item });
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;

        switch (index)
        {
            case 0:
                selected_language = "en";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.english;
                break;
            case 1:
                selected_language = "de";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.german;
                break;
            case 2:
                selected_language = "cn";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.chinese;
                break;
            case 3:
                selected_language = "jp";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.japanese;
                break;
            case 4:
                selected_language = "es";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.spanish;
                break;
            case 5:
                selected_language = "fr";
                VocabularyDB.activeLanguageMode = VocabularyDB.LanguageMode.french;
                break;
        }

        Debug.Log(selected_language);
    }

    //Ouput the new value of the Dropdown into Text
}
