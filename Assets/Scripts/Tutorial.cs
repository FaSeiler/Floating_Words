using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject TutorialText;
    private TextMeshProUGUI Tutorialtext;
    private bool FinishMoveCamera;
    private Quaternion startQuaternion;


    private void Awake()
    {
        if (SystemInfo.supportsGyroscope)
            Input.gyro.enabled = true;
        else
        {
            Debug.Log("This Device doesn't support gyroscope");
            Tutorialtext.gameObject.SetActive(false);
            Destroy(this);
        }

    }
    void Start()
    {
        
        FinishMoveCamera = false;
        Tutorialtext = TutorialText.GetComponent<TextMeshProUGUI>();
        Tutorialtext.text = " Please move around your mobile phone";
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
        yield return new WaitForSeconds(2);
        Tutorialtext.text ="Well done!";
        yield return new WaitForSeconds(3);
        Tutorialtext.text = "You can clean all the anchors by clicking the trash bin button";
        yield return new WaitForSeconds(3);
        Tutorialtext.text = "You can also check the dictionary by clicking the settings button";
        yield return new WaitForSeconds(3);
        Tutorialtext.text = "Have fun learning new words!";
        yield return new WaitForSeconds(3);
        TutorialText.gameObject.SetActive(false);
    }


    IEnumerator WaitForGyroscopeEnabled()
    {
        yield return new WaitUntil(() =>Input.gyro.enabled);
        startQuaternion = Input.gyro.attitude;
        //Debug.Log(startQuaternion.ToString());
    }
}
