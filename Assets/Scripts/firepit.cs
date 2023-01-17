using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firepit : MonoBehaviour
{
    // Start is called before the first frame update

    public float smallFuel;
    public float largeFuel;
    public bool isLit;
    public Sprite empty, large, small, largesmall;
    public GameObject Fire;
    private GameObject fireInstace;
    void Start()
    {
        smallFuel = 0f;
        largeFuel = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLit)
        {
            if(largeFuel > 0f)
            {
                Mathf.Clamp(largeFuel -= .01f, 0f, 100f);
            }
            if(smallFuel > 0f)
            {
                Mathf.Clamp(smallFuel -= .05f, 0f, 100f) ;
            }
            if(largeFuel == 0f && smallFuel == 0f)
            {
                isLit = false;
                Destroy(fireInstace);
            }
        }
    }

    public void addFuel(bool large, float amount)
    {
        if (large)
        {
            largeFuel += amount;
        }
        else
        {
            smallFuel += amount;
        }
    }
    public void ignite()
    {
        if (!isLit && smallFuel >= .05f)
        {
            isLit = true;
            fireInstace = Instantiate(Fire, this.transform.position, Quaternion.identity);
        }
    }
}
