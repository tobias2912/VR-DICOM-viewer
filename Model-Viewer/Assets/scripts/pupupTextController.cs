using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pupupTextController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject text;
    private float lastrefresh;
    [SerializeField]
    private float defaultTimeout;
    private float timeout;
    [SerializeField]
    private float UIdistance;

    void Start()
    {
        text.SetActive(false);
    }
    void Update()
    {
        if (lastrefresh + timeout < Time.time)
        {
            text.SetActive(false);
        }
    }
    public void showText(string s, float duration)
    {
        timeout = duration;
        text.GetComponent<TextMeshProUGUI>().text = s;
        pupup();
    }
    public void showText(string s)
    {
        timeout = defaultTimeout;
        text.GetComponent<TextMeshProUGUI>().text = s;
        pupup();
    }

    private void pupup()
    {
        if (!text.activeSelf)
        {
            text.SetActive(true);
            Vector3 temp = Camera.current.transform.position + Camera.current.transform.forward * UIdistance;
            transform.position = temp;
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.current.transform.position);
        }
        lastrefresh = Time.time;
    }
}
