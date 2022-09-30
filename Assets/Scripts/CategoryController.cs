using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryController : MonoBehaviour
{
    public List<Category> categories;

    public ToolTipController ttip;

    public ObjectPlacer objPlacer;

    public void Activate(int id)
    {
        foreach (GameObject gm in categories[id].objects)
        {
            Debug.Log(gm);
            gm.SetActive(true);
        }


        foreach (Category gm in categories)
        {
            gm.button.SetActive(false);
        }


        ttip.ChangeToolTip("");
        ttip.ChangeToolTip(categories[id].tooltip);
    }


    public void Cancel()
    {
        foreach (Category gm in categories)
        {
            gm.button.SetActive(true);
            foreach (GameObject gm2 in gm.objects)
            {
                gm2.SetActive(false);
            }
        }

        objPlacer.DeSpawn();
        ttip.ChangeToolTip("");
    }
}