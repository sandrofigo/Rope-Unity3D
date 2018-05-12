using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{


    public bool initialized = false;



    Transform anchor1;
    Transform anchor2;

    List<Vector3> nodes = new List<Vector3>();

    [SerializeField]
    [Range(2, 50)]
    private int nodeCount = 15;

    LineRenderer lineRenderer;

    private void SpawnNodes()
    {
        // Setup Line Renderer
        lineRenderer.positionCount = nodeCount;


        nodes.Clear();

        for (int i = 0; i < nodeCount; i++)
        {
            nodes.Add(Vector3.Lerp(anchor1.position, anchor2.position, (float)i / (float)(nodeCount - 1)));
        }

        
    }

    public void UpdateNodes()
    {
        nodes[0] = anchor1.position;
        nodes[nodes.Count - 1] = anchor2.position;

        lineRenderer.positionCount = nodes.Count;
        for (int i = 0; i < nodes.Count; i++)
        {
            lineRenderer.SetPosition(i, nodes[i]);
        }
    }

    private void Reset()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        GameObject a1 = new GameObject();
        a1.name = "Anchor 1";
        a1.transform.parent = transform;
        a1.transform.localPosition = new Vector3(-1, 0, 0);

        GameObject a2 = new GameObject();
        a2.name = "Anchor 2";
        a2.transform.parent = transform;
        a2.transform.localPosition = new Vector3(1, 0, 0);

        anchor1 = a1.transform;
        anchor2 = a2.transform;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, anchor1.localPosition);
        lineRenderer.SetPosition(1, anchor2.localPosition);

        SpawnNodes();

        initialized = true;
    }

    private void OnDrawGizmosSelected()
    {
        UpdateNodes();
    }

}
