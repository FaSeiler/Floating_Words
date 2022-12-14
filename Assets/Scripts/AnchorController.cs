using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;


public class AnchorController : MonoBehaviour
{
    private AnchorCreator anchorCreator;
    private bool labelStyleBillboard = false;

    private void Start()
    {
        anchorCreator = FindObjectOfType<AnchorCreator>();
    }

    /// <summary>
    /// Toggle the style of the label.
    /// Mode1: Arrow always faces the camera (labelStyleBillboard = true)
    /// Mode2: Arrow faces the camera but Y-Rotation is locked (labelStyleBillboard = false)
    /// </summary>
    public void ToggleLabelStyle()
    {
        if (labelStyleBillboard)
        {
            foreach (var anchor in anchorCreator.m_Anchors)
            {
                var labelController = anchor.GetComponent<LabelController>();
                labelController.lockLabelYRotation = false;
            }
        }
        else
        {
            foreach (var anchor in anchorCreator.m_Anchors)
            {
                var labelController = anchor.GetComponent<LabelController>();
                labelController.lockLabelYRotation = true;
            }
        }

        labelStyleBillboard = !labelStyleBillboard;
    }

    public void RemoveAllAnchors()
    {
        anchorCreator.RemoveAllAnchors();
    }
}
