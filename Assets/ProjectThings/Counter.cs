using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{

    public TextMeshProUGUI  countme;
    public TextMeshProUGUI  Total;
    public float price =25.0f;
    private float count;
    // Start is called before the first frame update
    void start()
    {
        countme = GetComponent<TextMeshProUGUI> ();
        Total = GetComponent<TextMeshProUGUI> ();
        count = 0.0f;
    }
    public void plusTrigger()
    {
        count = count +1.0f;

        Total.SetText("{0:1}",price*count); 
        countme.SetText("{0:1}",count);
    }
    public void minusTrigger()
    {
        count = count - 1.0f;
        

        if(count < 0.0f )
            count = 0.0f;

        
        Total.SetText("{0:1}",price*count);
        countme.SetText("{0:1}",count);
    }
}
