using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : MonoBehaviour
{
    public double globalTemp = 10;
    private double experiencedTemp;

    private double playerTemp;

    public Temperature()
    {
        playerTemp = 98.6;
    }
    private double calcExperiencedTemp(List<firepit> firepits)
    {
        double addedFromPits = 0;
        foreach(firepit f in firepits)
            addedFromPits += f.getOutputTemp();
        addedFromPits = Mathf.Min(80, (float)addedFromPits);
        return globalTemp + addedFromPits;
    }

    public double getExperiencedTemp()
    {
        return experiencedTemp;
    }

    public double getPlayerTemp()
    {
        return playerTemp;
    }

    public void setPlayerTemp(double t)
    {
        playerTemp = t;
    }

    // Update is called once per frame
    public void UpdateTemp()
    {
        List<firepit> firepits = Items.Instance.getFirepits();
        experiencedTemp = calcExperiencedTemp(firepits);
        double diff = (experiencedTemp + 20) - playerTemp;
        playerTemp += .00002 * diff;
    }
}
