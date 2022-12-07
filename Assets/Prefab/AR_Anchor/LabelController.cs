using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script controlls the 3D label that displays the translations for a object.
/// </summary>
public class LabelController : MonoBehaviour
{
    public Text label_text;

    public float damping = 5.0f;
    public bool lockLabelYRotation = true;

    void Update()
    {
        //LabelFaceCamera();
        TextFaceCamera();
    }

    private void LabelFaceCamera()
    {
        var target = Camera.main.gameObject.transform;

        var lookPos = target.position - transform.position;
        
        if (lockLabelYRotation)
        {
            lookPos.y = 0;
        }

        var rotation = Quaternion.LookRotation(-lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void TextFaceCamera()
    {
        label_text.gameObject.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
    }

    public void SetLabelText(string newLabelText)
    {
        label_text.text = newLabelText;
    }
}
