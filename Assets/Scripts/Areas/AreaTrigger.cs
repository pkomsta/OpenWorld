using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class AreaTrigger : MonoBehaviour
{

    [SerializeField] private List<Transform> areaTransforms;
    public MeshCollider areaRenderer;

    public UnityEvent onEnter;

    [SerializeField] private bool debug = true;


    private LineRenderer lineRenderer = null;
    private MeshCollider meshCollider = null;

    private void GenerateMeshCollider()
    {
        if (lineRenderer != null)
        {

            DestroyImmediate(lineRenderer);

        }
        if (meshCollider != null)
        {
            DestroyImmediate(meshCollider);
        }
        if (areaRenderer != null)
        {
            DestroyImmediate(areaRenderer.gameObject);
        }
        lineRenderer = this.AddComponent<LineRenderer>();
        meshCollider = this.AddComponent<MeshCollider>();
        lineRenderer.positionCount = areaTransforms.Count + 1;
        lineRenderer.useWorldSpace = true;
        int i = 0;
        foreach (var t in areaTransforms)
        {

            if (i + 1 < areaTransforms.Count)
            {
                CreateBoxColliderBetweenPoints(i);
            }
            else
            {
                CreateBoxColliderBetweenPoints(i, true);
            }

            i++;
        }


        Mesh mesh = new Mesh();

        lineRenderer.transform.TransformPoint(transform.position);
        meshCollider.transform.TransformPoint(transform.position);
        lineRenderer.BakeMesh(mesh, true);
        CreateAtreaTrigger(mesh);

    }

    private void CreateAtreaTrigger(Mesh mesh)
    {
        areaRenderer = new GameObject("MC").AddComponent<MeshCollider>();
        
        areaRenderer.convex = true;
        areaRenderer.isTrigger = true;
        areaRenderer.sharedMesh = mesh;
        areaRenderer.transform.TransformPoint(Vector3.zero);
        areaRenderer.transform.localScale = new Vector3(1f, 25f, 1f);
        areaRenderer.AddComponent<Area>();
        areaRenderer.GetComponent<Area>().Init(this);
    }

    public void CreateBoxColliderBetweenPoints(int i,bool lastPoint = false)
    {
        // Calculate the length of the box collider
        if (!lastPoint)
        {
            lineRenderer.SetPosition(i, areaTransforms[i].position);
        }
        else 
        {
            lineRenderer.SetPosition(i, areaTransforms[i].position);
            lineRenderer.SetPosition(i+1, areaTransforms[0].position);
        }
        


        // Calculate the center of the box collider


    }

    public void UpdateArea()
    {
        GenerateMeshCollider();
        if (debug)
        {
            Debug.Log("Area updated.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onEnter.Invoke();

            // Change lighting and fog here
        }
    }
    public void AddTransform()
    {
        var tr = new GameObject($"point {areaTransforms.Count}");
        tr.transform.parent = transform;
        tr.transform.localPosition = Vector3.zero;
        areaTransforms.Add(tr.transform);
    }
    public void ClearAllTransforms()
    {
        foreach(Transform t in areaTransforms)
        {
            DestroyImmediate(t.gameObject);
        }
        if (lineRenderer != null)
        {

            DestroyImmediate(lineRenderer);

        }
        if (meshCollider != null)
        {
            DestroyImmediate(meshCollider);
        }
        if (areaRenderer != null)
        {
            DestroyImmediate(areaRenderer);
        }
        areaTransforms.Clear();
    }
    [Button()]
    public void GenerateArea() => UpdateArea();
    [Button()]
    public void AddTransformToList() => AddTransform();

    [Button()]
    public void ClearList() => ClearAllTransforms();

    private void OnDrawGizmosSelected()
    {
        if (debug)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < areaTransforms.Count; i++)
            {
                Vector3 p1 = areaTransforms[i].position;
                Vector3 p2 = areaTransforms[(i + 1) % areaTransforms.Count].position;
                Gizmos.DrawLine(p1, p2);
            }
        }
    }
}
