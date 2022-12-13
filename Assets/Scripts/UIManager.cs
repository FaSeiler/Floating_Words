using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Button settingButton;
    public GameObject mainSettings;
    public GameObject dictionaryMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSettings()
    {
        settingButton.gameObject.SetActive(false);
        dictionaryMenu.SetActive(false);
        mainSettings.SetActive(true);
    }

    public void CloseFromMain()
    {
        mainSettings.SetActive(false);
        dictionaryMenu.SetActive(false);
        settingButton.gameObject.SetActive(true);
    }

    public void CloseDictionary()
    {
        dictionaryMenu.SetActive(false);
        mainSettings.SetActive(true);
    }

    public void ShowDictionary()
    {
        mainSettings.SetActive(false);
        dictionaryMenu.SetActive(true);
    }
}
