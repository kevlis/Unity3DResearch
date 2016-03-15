using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStarGrid : MonoBehaviour {

    Mesh mesh;
    List<Vector3> vertList;
    Vector3 target;
    // Use this for initialization
    void Start () {
        // At frist
        mesh = GetComponent<MeshFilter>().mesh;        
        vertList = mesh.vertices.ToList();
        vertList = vertList.Distinct().ToList();
        Debug.Log(vertList);
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Vector3 hitPos = transform.InverseTransformPoint(hit.point);
                CaculateShortVec(hitPos);
            }
        }
	}

    void CaculateShortVec(Vector3 pos)
    {
        Dictionary<Vector3, float> dict = new Dictionary<Vector3, float>();
        for (int i = 0; i < vertList.Count; ++i)
        {
            dict.Add(vertList[i], (vertList[i] - pos).sqrMagnitude);
        }
        dict = dict.OrderBy(v => v.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        Vector3 aroundVec = dict.ElementAt(0).Key;
        Vector3.Distance(aroundVec, target);
        Debug.Log(dict);
    }
}
