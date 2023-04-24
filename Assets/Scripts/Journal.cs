using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{

    CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
    }

    public void setVisibility(bool b)
    {
        canvasGroup.alpha = b ? 1 : 0;
        canvasGroup.interactable = b;
        canvasGroup.blocksRaycasts = b;
    }
}
