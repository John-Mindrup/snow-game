using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class firepit : MonoBehaviour
{
    // Start is called before the first frame update

    public float smallFuel;
    public float largeFuel;
    public bool isLit;
    private SpriteRenderer sr;
    public Sprite empty, large, small, largesmall;
    public GameObject Fire;
    private GameObject fireInstance;
    void Start()
    {
        smallFuel = 0f;
        largeFuel = 0f;
        sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (smallFuel > 0f && largeFuel > 0f)
        {
            sr.sprite = largesmall;
        }
        else if (smallFuel > 0f)
        {
            sr.sprite = small;
        }
        else if (largeFuel > 0f)
        {
            sr.sprite = large;
        }
        else sr.sprite = empty;
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
            if(largeFuel <= 0f && smallFuel <= 0f)
            {
                isLit = false;
                Destroy(fireInstance);
            }
        }
    }

    public double getOutputTemp()
    {
        if (!isLit)
            return 0;
        float maxDistance = 10f;
        double ret = 20 * Mathf.Sqrt(Mathf.Sqrt(largeFuel * 8 + smallFuel*4));
        
        PlayerInput player = PlayerInput.Instance;
        float distance = (player.transform.position - transform.position).magnitude;
        if (distance > maxDistance || distance == 0)
            return 0;
        ret *= (1 - (distance / maxDistance));
        return ret;
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
            fireInstance = Instantiate(Fire, this.transform.position, Quaternion.identity);
        }
    }
}
