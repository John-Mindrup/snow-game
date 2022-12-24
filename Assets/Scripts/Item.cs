using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool selected = false;
    private GameObject highlightInstance;
    private bool followCursor;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (followCursor)
        {
            rb.position = Items.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    public void addHighlight(GameObject o)
    {
        highlightInstance = Instantiate(Items.Instance.highlight, o.gameObject.transform.position, Quaternion.identity);
    }
    public void removeHighlight()
    {
        if (highlightInstance != null) Destroy(highlightInstance);
    }
    public void setFollowCursor(bool b)
    {
        followCursor = b;
    }
    public bool getFollowCursor()
    {
        return followCursor;
    }


}
