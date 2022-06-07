using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHints : MonoBehaviour
{
    private bool currentlyEnabled;

    // Start is called before the first frame update
    void Start()
    {
        showHints(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void toggleHints(){
        showHints(!currentlyEnabled);
    }
    public void showHints(bool enabled)
    {
        currentlyEnabled = enabled;
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject obj = transforms[i].gameObject;
            if (obj.name == "hintcanvas")
            {
                GameObject canvas = obj;
                canvas.SetActive(enabled);
            }
        }
    }
}
