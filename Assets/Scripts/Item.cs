using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool selected = false;
    private GameObject highlightInstance;
    public void addHighlight(GameObject o)
    {
        highlightInstance = Instantiate(Items.Instance.highlight, o.gameObject.transform.position, Quaternion.identity);
    }
    public void removeHighlight()
    {
        if (highlightInstance != null) Destroy(highlightInstance);
    }

}
