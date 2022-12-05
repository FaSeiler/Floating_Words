using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfo : MonoBehaviour
{
    public GameObject labelParentGO;
    public GameObject Textprefab;
    
    public void ShowLabel(string label,Vector2 center)
    {
        GameObject newText= Instantiate(Textprefab, new Vector2(0,0), Quaternion.identity);

        //TODO : optimize the code
        newText.transform.SetParent(labelParentGO.transform);
        newText.transform.localScale = new Vector3(
        newText.transform.localScale.x * labelParentGO.transform.localScale.x,
        newText.transform.localScale.y * labelParentGO.transform.localScale.y,
        newText.transform.localScale.z * labelParentGO.transform.localScale.z);

        newText.transform.position = center;
        newText.GetComponent<TMP_Text>().text=label;
    }
    public void CleanAll()
    {
        foreach (Transform child in labelParentGO.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
