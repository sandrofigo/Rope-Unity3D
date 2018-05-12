using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{


    public bool initialized = false;

    [SerializeField]
    private Material lineMaterial;

    [HideInInspector]
    [SerializeField]
    private Transform anchor1;
    [HideInInspector]
    [SerializeField]
    private Transform anchor2;

    List<Vector3> nodes = new List<Vector3>();

    [SerializeField]
    [Range(2, 50)]
    private int nodeCount = 15;
    [SerializeField]
    private float gravity = -0.01f;

    LineRenderer lineRenderer;

    [MenuItem("GameObject/3D Object/Rope")]
    private static void DoSomething()
    {
        GameObject rope = new GameObject();
        
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        if (sceneCamera != null)
        {
            rope.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * 5f;
        }

        rope.AddComponent(typeof(Rope));
        rope.name = "Rope";
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        CreateNodes(nodeCount);
    }

    private void Update()
    {
        UpdateNodes();
    }

    private void CreateNodes(int count)
    {
        // Setup Line Renderer
        lineRenderer.positionCount = count;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = lineMaterial;
        float brightness = 0.20f;
        Color c = new Color(brightness, brightness, brightness);
        lineRenderer.startColor = c;
        lineRenderer.endColor = c;


        nodes.Clear();

        for (int i = 0; i < count; i++)
        {
            nodes.Add(Vector3.Lerp(anchor1.position, anchor2.position, (float)i / (float)(count - 1)));
        }


    }

    public void UpdateNodes()
    {
        nodes[0] = anchor1.position;
        nodes[nodes.Count - 1] = anchor2.position;


        for (int i = 1; i < nodes.Count - 1; i++)
        {
            Vector3 dir = nodes[i + 1] - nodes[i - 1];
            dir *= 0.5f;

            nodes[i] = nodes[i - 1] + dir;

            Vector3 pos = nodes[i];
            pos.y += gravity;
            nodes[i] = pos;
        }

        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();

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
        a1.AddComponent(typeof(RopeAnchor));

        GameObject a2 = new GameObject();
        a2.name = "Anchor 2";
        a2.transform.parent = transform;
        a2.transform.localPosition = new Vector3(1, 0, 0);
        a2.AddComponent(typeof(RopeAnchor));

        anchor1 = a1.transform;
        anchor2 = a2.transform;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, anchor1.localPosition);
        lineRenderer.SetPosition(1, anchor2.localPosition);

        CreateNodes(2);

        UpdateLineRenderer();

        initialized = true;
    }

    private void OnDrawGizmos()
    {
        if (nodes.Count >= 2)
        {
            nodes[0] = anchor1.position;
            nodes[nodes.Count - 1] = anchor2.position;

            UpdateLineRenderer();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 dir = anchor2.position - anchor1.position;
        Vector3 pos = anchor1.position + dir * 0.5f;

        Vector3 oldAnchor1 = anchor1.position;
        Vector3 oldAnchor2 = anchor2.position;

        transform.position = pos;
        anchor1.position = oldAnchor1;
        anchor2.position = oldAnchor2;

        UpdateLineRenderer();
    }

}
