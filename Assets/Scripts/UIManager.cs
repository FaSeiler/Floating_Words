using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Button settingButton;
    public GameObject mainSettings;
    public DictionaryUI dictionaryUI;

    public void ShowSettings()
    {
        PauseController.PauseGame();
        settingButton.gameObject.SetActive(false);
        dictionaryUI.CloseDictionaryUI();
        mainSettings.SetActive(true);
    }

    public void CloseSettings()
    {
        PauseController.ResumeGame();
        mainSettings.SetActive(false);
        dictionaryUI.CloseDictionaryUI();
        settingButton.gameObject.SetActive(true);
    }

    public void CloseDictionary()
    {
        dictionaryUI.CloseDictionaryUI();
        mainSettings.SetActive(true);
    }

    public void ShowDictionary()
    {
        mainSettings.SetActive(false);
        dictionaryUI.OpenDictionaryUI();
    }
}
