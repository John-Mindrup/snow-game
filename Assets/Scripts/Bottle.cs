using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    Animator animator;
    float MaxVolume = 1000;
    float CurVolume;
    // Start is called before the first frame update
    void Start()
    {
        CurVolume = MaxVolume;
        animator = GetComponent<Animator>();
    }

    public float GetContents()
    {
        return CurVolume;
    }

    public void SetContents(float v)
    {
        CurVolume = v;
        animator.SetFloat("Water_Level", CurVolume / MaxVolume);
    }

    public void drink()
    {
        Temperature.Drink(this);
        
    }


}
