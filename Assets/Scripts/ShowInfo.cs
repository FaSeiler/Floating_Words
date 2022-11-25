using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfo : MonoBehaviour
{
    public Canvas canv;
    public GameObject Textprefab;
    
    public void ShowLabel(string label,Vector2 center)
    {
        GameObject newText= Instantiate(Textprefab, new Vector2(0,0), Quaternion.identity);

        //TODO : optimize the code
        newText.transform.SetParent(canv.gameObject.transform);
        newText.transform.localScale = new Vector3(
        newText.transform.localScale.x * canv.gameObject.transform.localScale.x,
        newText.transform.localScale.y * canv.gameObject.transform.localScale.y,
        newText.transform.localScale.z * canv.gameObject.transform.localScale.z);

        newText.transform.position = center;
        newText.GetComponent<TMP_Text>().text=label;
    }
    public void CleanAll()
    {
        foreach (Transform child in canv.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
