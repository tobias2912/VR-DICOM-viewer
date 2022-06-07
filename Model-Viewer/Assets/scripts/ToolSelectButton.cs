using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelectButton : MonoBehaviour
{

    public Tooltype tooltype;
    public GameObject toolControllerobj;
    [Header("Screw type if relevant")]

    private Button yourButton;
    private ToolController controller;
    [SerializeField]
    private bool hideMenuOnPress = true;

    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(selectTool);
        controller = toolControllerobj.GetComponent<ToolController>();
        //spawn tool hides menu
        Button button = GetComponent<Button>();
        button.onClick.AddListener(hidemenu);
    }

    private void hidemenu()
    {
        if (hideMenuOnPress)
        {
            GetComponentInParent<floatMenu>().hide();
        }
    }

    void selectTool()
    {
        Debug.Log("Tool selected: " + tooltype);
        controller.setCurrentTool(tooltype);
    }
}
