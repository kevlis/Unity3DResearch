
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStarBaseOnGrid : MonoBehaviour {
    Vector3[] vert;
    List<Vector3> vertList;
    int[] triangles;
    List<int> triangelsList;
    //int i = 0;
    Vector3 endPos = new Vector3(0.2f, 0.4f, 0.3f);
    Vector3[] endTrianglesVertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero };
    List<Vector3> navPointsList = new List<Vector3>();
    // Use this for initialization
    void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        triangles = mesh.triangles;
        triangelsList = triangles.ToList();

        Vector2[] uvs = mesh.uv;
        vert = mesh.vertices;
        vertList = vert.ToList();
        //for (int i = 4; i < 515; ++i)
        //{
        //    uvs[i] = uvs[0];
        //    vert[i] = vert[0];
        //}
        //for (int i = 6; i < 2034; ++i)
        //{
        //    triangles[i] = triangles[0];
        //}

        //// bottom surface
        //// up surface
        //// right Surface
        //// front Surface
        //// left Surface
        //// back Surface

        //mesh.Clear();
        //mesh.vertices = vert;
        //mesh.uv = uvs;
        //mesh.triangles = triangles;
        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();
        //Debug.Log(vert);

        endTrianglesVertices = GetTrianglesVerticesPosIn(transform.InverseTransformPoint(endPos));
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject endPoint = Instantiate(Resources.Load("end")) as GameObject;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                endPos = hit.point;
                endPoint.transform.position = endPos;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            navPointsList.Clear();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPos = transform.InverseTransformPoint(hit.point);
                Vector3[] startCurTri = GetTrianglesVerticesPosIn(hitPos);
                CreateSingnalSphere(startCurTri[0]);
                CreateSingnalSphere(startCurTri[1]);
                CreateSingnalSphere(startCurTri[2]);
                Vector3 firstWaypoint = CheckTheNearstVerticesInTrianglesArray(startCurTri, transform.InverseTransformPoint(endPos));
                drawLineFromPointToAround(startCurTri, hitPos);

                navPointsList.Add(hitPos);
                CreateSingnalSphere(hitPos);
                navPointsList.Add(firstWaypoint);

                Vector3[] v = GetAllVerticesIndexAroundTargetPos(firstWaypoint);
                int iii = 0;
                foreach (Vector3 vv in startCurTri)
                {
                    GameObject s = CreateSingnalSphere(vv);
                    s.transform.GetChild(0).GetComponent<TextMesh>().text = iii.ToString();
                    float vl = Vector3.Distance(transform.InverseTransformPoint(s.transform.position), transform.InverseTransformPoint(endPos));
                    Debug.Log(iii + " " + vl);
                    iii++;
                }


                Vector3 wayPos = Vector3.zero;
                while (wayPos != navPointsList[Mathf.Clamp(navPointsList.Count - 1, 0, navPointsList.Count)])
                {
                    Vector3[] aroundVectors = GetAllVerticesIndexAroundTargetPos(navPointsList[navPointsList.Count - 1]);
                    drawLineFromPointToAround(aroundVectors, navPointsList[navPointsList.Count - 1]);

                    Vector3 nearstWayPoint = CheckTheNearstVerticesInTrianglesArray(aroundVectors, transform.InverseTransformPoint(endPos));
                    wayPos = navPointsList[navPointsList.Count - 1];
                    Vector3 checkv = navPointsList.Find(s => s == nearstWayPoint);
                    if (checkv == nearstWayPoint)
                    {
                        break;
                    }
                    navPointsList.Add(nearstWayPoint);
                }
                navPointsList.Add(transform.InverseTransformPoint(endPos));
                Debug.Log("Find path end");

                Debug.DrawLine(transform.TransformPoint(navPointsList[0]), transform.TransformPoint(navPointsList[1]), Color.blue, 1000f);
                Debug.DrawLine(transform.TransformPoint(navPointsList[1]), transform.TransformPoint(navPointsList[2]), Color.yellow, 1000f);
                Debug.DrawLine(endPos, transform.TransformPoint(navPointsList[navPointsList.Count - 2]), Color.cyan, 1000f);
                Color color = Color.red;
                for (int i = 1; i < navPointsList.Count; ++i)
                {
                    if (i == navPointsList.Count - 1)
                    {
                        break;
                    }
                    CreateSingnalSphere(navPointsList[i]);
                    Debug.DrawLine(transform.TransformPoint(navPointsList[i]), transform.TransformPoint(navPointsList[i + 1]), color, 1000f);
                    color += new Color(-0.1f, 0.15f, 0.15f, 0f);
                }
            }
        }
	}

    GameObject CreateSingnalSphere(Vector3 pos)
    {
        GameObject s = Instantiate(Resources.Load("end")) as GameObject;
        s.transform.position = transform.TransformPoint(pos);
        return s;
    }
    void drawLineFromPointToAround(Vector3[] v, Vector3 pos)
    {
        for (int i = 0; i < v.Length; ++i)
        {
            //Debug.DrawLine(transform.TransformPoint(pos), transform.TransformPoint(v[i]), Color.red, 1000f);
        }
    }

    Vector3[] GetAllVerticesIndexAroundTargetPos(Vector3 targetPos)
    {
        // get all indexs of the target postion used in the vectices arrary
        List<int> indexList = new List<int>();
        List<Vector3> resList = new List<Vector3>();
        for (int i = 0; i < vert.Length; ++i)
        {
            // the index get
            if (targetPos == vert[i])
            {
                //indexList.Add(i);
                for (int n = 0; n < triangles.Length; ++n)
                {
                    if (triangles[n] == i)
                    {
                        if (n % 3 == 0)
                        {
                            resList.Add(vert[triangles[n]]);
                            resList.Add(vert[triangles[n + 1]]);
                            resList.Add(vert[triangles[n + 2]]);
                        }
                        if (n % 3 == 1)
                        {
                            resList.Add(vert[triangles[n]]);
                            resList.Add(vert[triangles[n-1]]);
                            resList.Add(vert[triangles[n+1]]);
                        }
                        if (n % 3 == 2)
                        {
                            resList.Add(vert[triangles[n]]);
                            resList.Add(vert[triangles[n - 1]]);
                            resList.Add(vert[triangles[n - 2]]);
                        }
                    }
                }
            }
        }
        //resList.Add(targetPos);
        return resList.Distinct().ToList().ToArray();
    }

    // Get the postion vector which have the shortest distance between itself and the targetPos
    Vector3 CheckTheNearstVerticesInTrianglesArray(Vector3[] triCur, Vector3 endPos)
    {
        Vector3 resVec = triCur[0];
        float dis = Vector3.Distance(triCur[0], endPos);
        foreach(Vector3 v in triCur)
        {
            float disCompare = Vector3.Distance(v, endPos);
            if (disCompare < dis)
            {
                resVec = v;
            }
            //if (disCompare != 0f)
            //{
            //    if (dis < disCompare)
            //    {
            //        resVec = v; 
            //    }
            //}
            //else
            //{
            //    dis = disCompare;
            //}
        }
        return resVec;
    }

    // Get the triangleswhich the target pos in , return it by the form of a array include all three vertices postion of it
    Vector3[] GetTrianglesVerticesPosIn(Vector3 pos)
    {
        for (int i = 0; i < triangles.Length; ++i)
        {
            Vector3[] vertTri = {vert[triangles[i]],
                                        vert[triangles[i+1]],
                                        vert[triangles[i+2]]};
            if ((vert[triangles[i]] - pos).magnitude < 0.2)
            {
                bool isin = JudgePosInTriangles(pos, vertTri);
                if (isin)
                {
                    return vertTri;
                }
            }
            i = i + 2;
        }
        return endTrianglesVertices;
    }

    // Judge wheather the target pos in the given triangles
    bool JudgePosInTriangles(Vector3 pos, Vector3[] tri)
    {
        Vector3 v0 = tri[2] - tri[0];
        Vector3 v1 = tri[1] - tri[0];
        Vector3 v2 = pos - tri[0];
        Vector3 cross = Vector3.Cross(v0, v1);
        //float dot = v2.x * cross.x + v2.y * cross.y + v2.z * cross.z;
        //if (dot != 0)
        //{
        //    return false;
        //}
        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float inverDeno = 1 / (dot00 * dot11 - dot01 * dot01);

        float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;
        if (u < 0 || u > 1) // if u out of range, return directly
        {
            return false;
        }

        float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;
        if (v < 0 || v > 1) // if v out of range, return directly
        {
            return false;
        }

        return u + v <= 1;
    }
}
