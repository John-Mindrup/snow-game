using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public GameObject log, stick, branch;
    // Start is called before the first frame update
    void Start()
    {
        if(Random.value < .4f)
            StartCoroutine(spawnDelay());
    }

    

    void SpawnObject()
    {
        if(Random.value < .4f)
            StartCoroutine(spawnDelay());
    }

    IEnumerator spawnDelay()
    {
        yield return new WaitForSeconds(Random.Range(0f, 10f));
        float dist = Random.Range(.5f, 1.5f);
        float theta = Random.Range(Mathf.PI, 2 * Mathf.PI);
        float x = dist * Mathf.Cos(theta);
        float y = dist * Mathf.Sin(theta);
        float r = Random.value;
        if (r < .2f)
            Instantiate(log, new Vector3(this.transform.position.x + x, (this.transform.position.y - 1f) + y), Quaternion.identity);
        else if (r < .8f)
            Instantiate(stick, new Vector3(this.transform.position.x + x, (this.transform.position.y - 1f) + y), Quaternion.identity);
        else
            Instantiate(branch, new Vector3(this.transform.position.x + x, (this.transform.position.y - 1f) + y), Quaternion.identity);
        yield return null;
    }
}
