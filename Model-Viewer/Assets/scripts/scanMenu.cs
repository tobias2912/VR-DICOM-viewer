using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scanMenu : MonoBehaviour
{
    public GameObject scancollection;
    public GameObject buttonPre;
    [SerializeField]
    private GameObject buttonGroup;
    private scanController scanController;

    // Start is called before the first frame update
    void Start()
    {
        scanCollection scanCollection = scancollection.GetComponent<scanCollection>();
        scanController = scanCollection.GetScanController();
        List<scanPackage> scanPackages = scanController.getScanPackages();
        createButtons(scanPackages);
    }

    private void createButtons(List<scanPackage> scanPackages)
    {
        int i = 0;
        foreach (var item in scanPackages)
        {
            print("create button for " + item.description + " index " + i);
            GameObject buttonObj = Instantiate(buttonPre, buttonGroup.transform);
            TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI[] textMeshProUGUIs = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var TMP in textMeshProUGUIs)
            {
                if (TMP.name == "Text")
                {
                    text = TMP;
                }
            }
            text.text = "Scan " + i + "\n" + item.description;
            Button buttonComp = buttonObj.GetComponent<Button>();
            int icopy = i;
            buttonComp.onClick.AddListener(delegate { buttonPressed(icopy); });
            i++;
        }
    }

    void buttonPressed(int i)
    {
        print("button press to change to scan " + i);
        scanController.switchToScan(i);
        GetComponent<floatMenu>().hide();
    }

}
