using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;

public class InteractionManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    
    Camera arCam;
    GameObject selectedLabel;
    bool colorSwitched = false;

    public AnchorCreator AnchorCreator;


    // Start is called before the first frame update
    void Start()
    {
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;
            
        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if (Physics.Raycast(ray, out hit))
            {
                //if (hit.collider.gameObject.tag == "3D_Label")
                //{
                    //seletedLabel = hit.collider.gameObject;
                    //Debug.Log("3d_label selected");
                //}
                //else
                //{
                    //Debug.Log("3d_label not seleted");
                //}
                selectedLabel = hit.collider.gameObject;
                
                Destroy(GameObject.FindWithTag("3D_Label"));
                //AnchorCreator.removefromlist(selectedLabel);



                //var objectRenderer = selectedLabel.GetComponent<Renderer>();
                //objectRenderer.material.SetColor("_Color", Color.red);


                //if (!colorSwitched)
                //{
                //objectRenderer.material.SetColor("_Color", Color.red);
                //colorSwitched ^= true;
                //}

                //if (colorSwitched)
                //{
                //objectRenderer.material.SetColor("_Color", Color.blue);
                //colorSwitched ^= true;
                //}

                //objectRenderer.material.SetColor("_Color", Color.blue);


                //var color = objectRenderer.material.color;
                //if (color.Equals(Color.red))
                //{
                //objectRenderer.material.SetColor("_Color", Color.blue);
                //}
                //if (color.Equals(Color.blue))
                //{
                //objectRenderer.material.SetColor("_Color", Color.red);
                //}
            }
        }
        
    }
}
