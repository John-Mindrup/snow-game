using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : MonoBehaviour
{
    public double globalTemp = 10;
    private double experiencedTemp;

    private static double playerTemp;

    public const float MAX_WATER = 162000;
    private static float playerWater;

    public Temperature()
    {
        playerTemp = 98.6;
        playerWater = MAX_WATER;
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

    public void UpdateTemp()
    {
        List<firepit> firepits = Items.Instance.getFirepits();
        experiencedTemp = calcExperiencedTemp(firepits);
        double diff = (experiencedTemp + 20) - playerTemp;
        if (diff < 0)
            playerTemp += .00002 * diff;
        else
            playerTemp += .0001 * diff;
    }

    public float GetWater()
    {
        return playerWater;
    }

    public void UpdateWater()
    {
        playerWater--;
    }

    public static void Drink(Bottle bottle)
    {
        Debug.Log(bottle.GetContents());
        if(playerWater + bottle.GetContents() * 5 > MAX_WATER)
        {
            bottle.SetContents(bottle.GetContents() - (MAX_WATER - playerWater)/5);
            playerWater = MAX_WATER;
            
        }
        else
        {
            playerWater += (int)bottle.GetContents()*5;
            bottle.SetContents(0);
        }
        Debug.Log(bottle.GetContents());
    }
}
