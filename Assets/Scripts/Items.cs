using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public List<GameObject> pickupableItems;
    public List<GameObject> recipe1, recipe2;
    private List<List<GameObject>> recipes = new();
    public List<GameObject> products;

    public Camera mainCamera;

    private static Items _instance;
    public static Items Instance { get { return _instance; } }
    public GameObject highlight, craftingButton;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.recipes.Add(recipe1);
            _instance = this;
        }
    }
    
    public bool isPickupable(Collider2D c)
    {
        foreach(GameObject o in pickupableItems)
        {
            if (c != null && c.name.StartsWith(o.name))
                return true;
        }
        return false;
        
    }

    public bool isCraftingButton(Collider2D  c)
    {
        if(c.attachedRigidbody.gameObject == craftingButton)
        {
            return true;
        }
        return false;
    }

    public int compareRecipes(List<GameObject> a, List<GameObject> b)
    {
        if (a.Count != b.Count)
            return -1;
        a.Sort(compareObjects);
        b.Sort(compareObjects);
        for(int i = 0; i < a.Count; i++)
        {
            if(compareObjects(a[i], b[i]) != 0)
            {
                return 1;
            }
        }
        return 0;
    }
    public int compareObjects(GameObject a, GameObject b)
    {
        return string.Compare(removeEnd(a.name), removeEnd(b.name));
    }

    public GameObject craft(List<GameObject> r) 
    {
        for(int i = 0; i < recipes.Count; i++) 
        {
            List<GameObject> l = recipes[i];
            if(compareRecipes(r, l) == 0)
            {
                return products[i];
            }
        }
        return null;
    }

    public string removeEnd(string s)
    {
        string[] c = s.Split(' ');
        return c[0];
    }
    
}
