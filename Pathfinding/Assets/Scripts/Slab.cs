using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slab : MonoBehaviour
{

    private int cost = 0;
    public int x = -1;
    public int y = -1;

    private TextMeshProUGUI myTMP;
        

    public void setCoords(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public void setCost(int _cost)
    {
        this.cost = _cost; 
    }


    // Start is called before the first frame update
    void Start()
    {
        myTMP = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myTMP != null)
            myTMP.SetText(cost.ToString());
        else
            Debug.Log("Text Mesh Pro component not found!");
    }
}
