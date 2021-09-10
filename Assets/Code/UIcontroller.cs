using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{

    public Text healthUi, staminaUi;
    public int health;
    public float stamina;


    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        stamina = 100;
     //   healthUi.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        healthUi.text = "Health: " + health + "/100";
        staminaUi.text = "Stamina: " + Mathf.Round(stamina) + "/100";
    }
}
