using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipController : MonoBehaviour
{
    private TMP_Text text;


    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = "";
    }


    public void ChangeToolTip(string str)
    {
        text.text = str;
    }
}