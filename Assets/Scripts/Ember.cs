using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ember : MonoBehaviour
{

    private int lifeSpan;
    private void Awake()
    {
        lifeSpan = 500;
    }
    void FixedUpdate()
    {
        lifeSpan--;
        if(lifeSpan <= 0)
        {
            PlayerInput.Instance.removeFromInvetory(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
