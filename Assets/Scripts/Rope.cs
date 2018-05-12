using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Rope : MonoBehaviour
{


    public bool initialized = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Reset()
    {

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        GameObject anchor1 = new GameObject();
        anchor1.name = "Anchor 1";
        anchor1.transform.parent = transform;
        anchor1.transform.localPosition = new Vector3(-1, 1, -1);

        GameObject anchor2 = new GameObject();
        anchor2.name = "Anchor 2";
        anchor2.transform.parent = transform;
        anchor2.transform.localPosition = new Vector3(1, 1, 1);

        initialized = true;

    }


}
