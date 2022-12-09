using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public string selected_language;


    void Start()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("English");
        items.Add("German");
        items.Add("Chinese");
        items.Add("Japanese");
        items.Add("Spanish");
        items.Add("French");
        
        foreach(var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() {text = item});
        }

        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown);});
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;

        switch (index)
        {
            case 0: selected_language = "en"; 
                break;
            case 1: selected_language = "de"; 
                break;
            case 2: selected_language = "cn"; 
                break;
            case 3: selected_language = "jp"; 
                break;
            case 4: selected_language = "es"; 
                break;
            case 5: selected_language = "fr"; 
                break;
        }
        Debug.Log(selected_language);
    }

    //Ouput the new value of the Dropdown into Text
}
