using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private bool FinishMoveCamera;
    private Quaternion startQuaternion;


    private void Awake()
    {
        if (SystemInfo.supportsGyroscope && PlayerPrefs.GetInt("tutorial_finished" !) != 1)
            Input.gyro.enabled = true;
        else
        {
            Debug.Log("This Device doesn't support gyroscope");
            tutorialText.gameObject.SetActive(false);
            Destroy(this);
        }

    }
    void Start()
    {
        FinishMoveCamera = false;
        tutorialText.text = "Move your phone around to calibrate";
        StartCoroutine(WaitForGyroscopeEnabled());
    }

    void Update()
    {
        if (!FinishMoveCamera && Input.gyro.enabled && Quaternion.Angle(startQuaternion, Input.gyro.attitude)>=45)
        {
            //Debug.Log(startQuaternion.ToString());
            //Debug.Log(Quaternion.Angle(startQuaternion, Input.gyro.attitude));
            FinishMoveCamera =true;
            StartCoroutine(StartTutorial());
        }
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(7);
        tutorialText.text ="Well done!";
        yield return new WaitForSeconds(3);
        tutorialText.text = "Clear anchors by clicking the trash bin button";
        yield return new WaitForSeconds(4);
        tutorialText.text = "Check the dictionary by clicking the settings button";
        yield return new WaitForSeconds(4);
        tutorialText.text = "Have fun learning new words!";
        yield return new WaitForSeconds(5);
        tutorialText.gameObject.SetActive(false);
        PlayerPrefs.SetInt("tutorial_finished", 1);
    }


    IEnumerator WaitForGyroscopeEnabled()
    {
        yield return new WaitUntil(() =>Input.gyro.enabled);
        startQuaternion = Input.gyro.attitude;
        //Debug.Log(startQuaternion.ToString());
    }
}
