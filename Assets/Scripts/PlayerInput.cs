using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public int moveSpeed = 2;

    Vector2 movement, mouseMovement, mouseMovementLast;

    Rigidbody2D rb;

    Animator animator;

    private Temperature temperature;

    public Tilemap footGrid1;
    public Tilemap footGrid2;
    public TMP_Text feelsLike, bodyTemp;
    public GameObject hotbar, Ember, firePit;
    public GameObject waterBottle, hoodieString;
    public Camera mainCamera;
    private Tile currentFootprints;

    private bool isDrilling = false;
    private static PlayerInput _instance;
    GameObject[] inventory = new GameObject[10];
    private int drillTime;
    private List<GameObject> recipe = new();
    private GameObject heldItem;
    private bool placeingPit = false;
    private bool journalOpen = false;
    private GameObject journal;
    private const int MAX_CONDITION = 5;
    private int Condition;
    public static PlayerInput Instance { get { return _instance; } }
    enum direction
    {
        None,
        North,
        East,
        South,
        West
    }
    direction currentDirection = direction.None;
    private int spawnCountdown = 18000;

    private void Start()
    {
        Condition = MAX_CONDITION;
        journal = GetComponentInChildren<Journal>().gameObject;
        temperature = new Temperature();
        heldItem = null;
        for (int i = 0;i < inventory.Length;i++) { inventory[i] = null; }
        addToInvetory(waterBottle.gameObject);
        addToInvetory(hoodieString.gameObject);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        spawnCountdown--;
        if(spawnCountdown == 0)
        {
            spawnCountdown = 36000;
            GameObject[] trees = GameObject.FindGameObjectsWithTag("TreeObject");
            foreach (GameObject t in trees) { t.SendMessage("SpawnObject"); }
        }
        temperature.UpdateTemp();
        temperature.UpdateWater();
        feelsLike.text = "Feels Like: " + System.String.Format("{0:0.00}",temperature.getExperiencedTemp());
        bodyTemp.text = "Body Temp: " + System.String.Format("{0:0.00}", temperature.getPlayerTemp());
        float speedAdjust = (float)GetCondition()/(float)MAX_CONDITION;
        if(movement != Vector2.zero && !isDrilling)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            rb.Cast(movement, hits, moveSpeed * Time.fixedDeltaTime);
            bool collide = false;
            foreach(RaycastHit2D r in hits)
            {
                if (r.rigidbody != null && r.rigidbody.gameObject.GetComponent<isCollision>() != null)
                    collide = true;
                if (r.collider != null && r.collider.gameObject.GetComponent<isCollision>())
                    collide = true;
            }
            if(!collide)
            {
                Debug.Log(GetCondition());
                Debug.Log(speedAdjust);
                Vector2 adjustedMovement = new Vector2(movement.x * speedAdjust, movement.y * speedAdjust);
                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * adjustedMovement);
                footprints();
            }
            
        }
        if (isDrilling)
        {
            if(drillTime > 0)
            {
                drillTime--;
            }
            else
            {
                isDrilling = false;
                animator.SetTrigger("EndDrilling");
                GameObject g = Instantiate(Ember, this.transform.position, Quaternion.identity);
                addToInvetory(g);
            }
            
        }
    }

    private int GetCondition()
    {
        int ret = MAX_CONDITION;
        float water = temperature.GetWater();
        double temp = temperature.getPlayerTemp();
        if (water / Temperature.MAX_WATER < .333333f)
            ret -= 2;
        else if (water / Temperature.MAX_WATER < .6666667f)
            ret -= 1;
        if (temp < 89.6f)
            ret -= 2;
        else if (temp < 95.0f)
            ret -= 1;
        Debug.Log(ret);
        return ret;
    }
    private void setAnimatorBools()
    {
        resetAnimatorBools();
        if (Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
        {
            if (movement.x > 0)
            {
                animator.SetBool("right", true);
                currentDirection = direction.East;
                
            }
            else if(movement.x < 0)
            {
                animator.SetBool("left", true);
                currentDirection = direction.West;
               
            }
            int val = Mathf.FloorToInt(rb.position.y);
            if (rb.position.y - val < .5)
            {
                currentFootprints = TileMapSprites.Instance.ePrints;
            }
            else
            {
                currentFootprints = TileMapSprites.Instance.wPrints;
            }
        }
        else
        {
            if (movement.y > 0)
            {
                animator.SetBool("up", true);
                currentDirection = direction.North;
            }
            else if(movement.y < 0)
            {
                animator.SetBool("down", true);
                currentDirection = direction.South;
            }
            int val = Mathf.FloorToInt(rb.position.x);
            if (rb.position.x - val < .5)
            {
                currentFootprints = TileMapSprites.Instance.sPrints;
            }
            else
            {
                currentFootprints = TileMapSprites.Instance.nPrints;
            }
        }
    }
    private void resetAnimatorBools()
    {
        animator.SetBool("right", false);
        animator.SetBool("left", false);
        animator.SetBool("up", false);
        animator.SetBool("down", false);
    }

    
    private void footprints()
    {
        //if first layer is empty place footprints there
        if (footGrid1.GetTile(footGrid1.WorldToCell(rb.position)) == null)
            footGrid1.SetTile(footGrid1.WorldToCell(rb.position), currentFootprints);
        //if first layer is full place footprints in second layer
        else if (footGrid2.GetTile(footGrid2.WorldToCell(rb.position)) == null)
        {
            //dont place same footprints in both layers
            if (currentFootprints != footGrid1.GetTile(footGrid1.WorldToCell(rb.position)))
            {
                footGrid2.SetTile(footGrid2.WorldToCell(rb.position), currentFootprints);
            }
        }
        //if both are full and trying to place new prints they get trudged
        else if (currentFootprints != footGrid1.GetTile(footGrid1.WorldToCell(rb.position)) && currentFootprints != footGrid2.GetTile(footGrid2.WorldToCell(rb.position)))
            footGrid2.SetTile(footGrid2.WorldToCell(rb.position), TileMapSprites.Instance.tPrints);
    }

    private void onLook(InputValue mouseMovement)
    {
        if (isDrilling)
        {
            Vector2 mouseDelta = mouseMovement.Get<Vector2>();
            if(this.mouseMovement != null)
            {
                mouseMovementLast = this.mouseMovement;
            }
            this.mouseMovement = mouseDelta;
        }
    }
    private void OnMove(InputValue movementValue)
    {
        movement = movementValue.Get<Vector2>();
        setAnimatorBools();
    }

    private void OnInteract()
    {
        Collider2D[] items = pickup();
        foreach(Collider2D i in items){
            if(Items.Instance.isPickupable(i))
            {
                if (addToInvetory(i.attachedRigidbody.gameObject))
                    return;
            }
        }
    }

    void OnOpenJournal()
    {
        journal.SendMessage("setVisibility", !journalOpen);
        journalOpen = !journalOpen;
    }

    public bool addToInvetory(GameObject g)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] == null)
            {
                inventory[i] = g;
                g.transform.parent = hotbar.transform;
                g.transform.position = hotbar.transform.position + Vector3.right / 2 + Vector3.right * i;
                SpriteRenderer r = g.GetComponent<SpriteRenderer>();
                r.sortingLayerName = "UI";
                return true;
            }
        }
        return false;
    }
    public bool removeFromInvetory(GameObject g)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == g)
            {
                Item item = g.GetComponent<Item>();
                item.removeHighlight();
                inventory[i] = null;
                Transform childToRemove = hotbar.transform.Find(g.name);
                childToRemove.SetParent(null);
                SpriteRenderer r = g.GetComponent<SpriteRenderer>();
                r.sortingLayerName = "Default";
                return true;
            }
        }
        return false;
    }

    private Collider2D[] pickup()
    {
        Collider2D[] results = new Collider2D[20];
        int count;
        if(currentDirection == direction.North)
        {
            count = Physics2D.OverlapBox(rb.position + Vector2.up/2f, Vector2.one/1.8f, 0, new ContactFilter2D(), results);
        }
        else if(currentDirection == direction.East)
        {
            count = Physics2D.OverlapBox(rb.position + Vector2.right / 2f, Vector2.one/1.8f, 0, new ContactFilter2D(), results);
        }
        else if (currentDirection == direction.West)
        {
            count = Physics2D.OverlapBox(rb.position - Vector2.right / 2f, Vector2.one/1.8f, 0, new ContactFilter2D(), results);
        }
        else
        {
            count = Physics2D.OverlapBox(rb.position + Vector2.down / 2f, Vector2.one/1.8f, 0, new ContactFilter2D(), results);
        }
        return results;

    }

    private void OnSelect()
    {
        if (heldItem != null)
            return;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] results = new Collider2D[5];
        Physics2D.OverlapBox(mousePos, new Vector2(0.1f, 0.1f), 0.0f, new ContactFilter2D(), results);
        foreach(Collider2D c in results)
        {
            if(c != null)
            {
                GameObject o = c.attachedRigidbody.gameObject;
                Bottle b = o.GetComponent<Bottle>();
                if(b != null)
                {
                    b.drink();
                    return;
                }
                if (Items.Instance.isCraftingButton(c))
                {
                    Vector3 left = recipe[0].transform.position;
                    GameObject product = Items.Instance.craft(recipe);
                    if(product != null)
                    {
                        
                        for(int i = recipe.Count -1; i >= 0; i--)
                        {
                            GameObject ob = recipe[i];
                            removeFromInvetory(ob);
                            recipe.Remove(ob); 
                            Item item = ob.GetComponent<Item>(); 
                            item.removeHighlight(); 
                            Destroy(ob);
                        }
                        if (Items.Instance.removeEnd(product.name).Equals("Joe_BowDrill_valid"))
                        {
                            isDrilling = true;
                            drillTime = 300 + Random.Range(-100, 300);
                            animator.SetTrigger("StartDrilling");
                            return;
                        }
                        addToInvetory(Instantiate(product,left,Quaternion.identity));
                    }
                }
                if (Items.Instance.isFirepitButton(c))
                {
                    if (heldItem != null) return;
                    heldItem = Instantiate(firePit, this.transform.position, Quaternion.identity);
                    Items.Instance.addFirepit(heldItem.GetComponent<firepit>());
                    Item item = heldItem.GetComponent<Item>();
                    item.setFollowCursor(true);
                    return;

                }
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] != null && o == inventory[i])
                    {
                        Item item = o.GetComponent<Item>();
                        if (!item.selected)
                        {
                            item.selected = true;
                            recipe.Add(o);
                            item.addHighlight(o);
                            return;
                        }
                        else
                        {
                            item.selected = false;
                            recipe.Remove(o);
                            item.removeHighlight();
                            return;
                        }

                    }
                }
            }
                       
        }
    }

    private bool inventoryContains(GameObject g)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if (g == inventory[i]) return true;
        }
        return false;
    }

    private void OnTest()
    {
        
    }

    private void OnPlace()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] results = new Collider2D[5];
        Physics2D.OverlapBox(mousePos, new Vector2(0.1f, 0.1f), 0.0f, new ContactFilter2D(), results);
        
        Flammable f = null;
        if(heldItem != null)
            f = heldItem.GetComponent<Flammable>();
        if (f != null)
        {
            foreach (Collider2D c in results)
            {
                if(c != null)
                {
                    firepit pit = c.attachedRigidbody.gameObject.GetComponent<firepit>();
                    if (pit != null)
                    {
                        addFuel(pit, f);
                        heldItem.GetComponent<Item>().setFollowCursor(false);
                        Destroy(heldItem);
                        heldItem = null;
                        return;
                    }
                }
                
            }
        }
        if (heldItem != null && !inventoryContains(heldItem))
        {
            Item item = heldItem.GetComponent<Item>();
            item.setFollowCursor(false);
            heldItem = null;
        }


        foreach (Collider2D c in results)
        {
            if (c != null)
            {
                GameObject o = c.attachedRigidbody.gameObject;
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] != null && o == inventory[i])
                    {
                        
                        Item item = o.GetComponent<Item>();
                        if (heldItem == null)
                        {
                            heldItem = o;
                            item.selected = false;
                            recipe.Remove(o);
                            removeFromInvetory(o);
                            item.removeHighlight();
                            item.setFollowCursor(true);
                            return;
                        }
                        else
                        {
                            List<Collider2D> res = new();
                            Physics2D.OverlapCircle(this.transform.position, 1, new ContactFilter2D(), res);
                            List<GameObject> objects = new();
                            foreach(Collider2D collider in res)
                            {
                                objects.Add(collider.attachedRigidbody.gameObject);
                            }
                            item.setFollowCursor(false);
                            inventory[i] = null;
                            if (!objects.Contains(heldItem))
                            {
                                addToInvetory(heldItem);
                            }
                            heldItem = null;
                        }

                    }
                }
            }

        }
    }

    private void addFuel(firepit pit, Flammable f)
    {
        pit.addFuel(f.large, f.burnTime);
        if (f.ignites) pit.ignite();
    }
}

