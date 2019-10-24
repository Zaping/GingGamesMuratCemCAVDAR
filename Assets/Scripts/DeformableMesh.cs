
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GeneratePlaneMesh))]
public class DeformableMesh : MonoBehaviour          //Mesh deformation and update of mesh collider
{

    public float maximumDepression=0.05f;     //how deep is the most deep depression
    public List<Vector3> originalVertices;
    public List<Vector3> modifiedVertices;

    private MeshFilter plane;
    public float radius=0.08f;            //radius of the depression
    
    public void MeshRegenerated()
    {
        GetComponent<MeshFilter>().mesh.MarkDynamic();          //Marking meshes as dynamic so mesh deformation will have less performance impact
        plane = GetComponent<MeshFilter>();
        plane.mesh.MarkDynamic();
        originalVertices = plane.mesh.vertices.ToList();        //Initializing vertice lists
        modifiedVertices = plane.mesh.vertices.ToList();
        Debug.Log("Mesh Regenerated");        
        gameObject.AddComponent<MeshCollider>();        
    }

    private void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;                                                      
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                AddDepression(hit.point, radius);
            }
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                AddDepression(hit.point, radius);
            }
        }

    }

    public void AddDepression(Vector3 depressionPoint, float radius)       //mesh is being depressed and new vertices are being recorded
    {
        var worldPos4 = this.transform.worldToLocalMatrix * depressionPoint;
        var worldPos = new Vector3(worldPos4.x, worldPos4.y, worldPos4.z);
        for (int i = 0; i < modifiedVertices.Count; ++i)                
        {
            var distance = (worldPos - (modifiedVertices[i] + Vector3.down * maximumDepression)).magnitude;              
            if (distance < radius)
            {
                var newVert = originalVertices[i] + Vector3.down * maximumDepression;
                modifiedVertices.RemoveAt(i);
                modifiedVertices.Insert(i, newVert);
            }
        }

        plane.mesh.SetVertices(modifiedVertices);              //mesh collider is being updated here
        Debug.Log("Mesh Depressed");        
        plane.mesh.RecalculateBounds();     
        GetComponent<MeshCollider>().sharedMesh = plane.mesh;        //this requires a lot of resources --Optimization Concern
    }
}