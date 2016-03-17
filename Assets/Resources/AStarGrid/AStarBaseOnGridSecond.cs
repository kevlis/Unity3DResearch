
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStarBaseOnGridSecond : MonoBehaviour {
    Vector3[] vert;
    List<Vector3> vertList;
    int[] triangles;
    List<int> triangelsList;
    Vector3[] startCurTri;
    //int i = 0;
    Vector3 endPos = new Vector3(0.2f, 0.4f, 0.3f);
    Vector3 secondToLastPos = Vector3.zero;
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
                endTrianglesVertices = GetTrianglesVerticesPosIn(transform.InverseTransformPoint(endPos));
                //secondToLastPos = endTrianglesVertices[0];
                //endPos = transform.TransformPoint(secondToLastPos);
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

                float dis = Vector3.Distance(secondToLastPos, hitPos);                
                foreach (Vector3 elementVec in endTrianglesVertices)
                {
                    float disCompare = Vector3.Distance(elementVec, hitPos);
                    if (disCompare < dis) 
                    {
                        dis = disCompare;
                        secondToLastPos = elementVec;
                    }
                }
                //endPos = transform.TransformPoint(secondToLastPos);
                Debug.Log("Second to last postion: " + secondToLastPos);

                startCurTri = GetTrianglesVerticesPosIn(hitPos);
                //CreateSingnalSphere(startCurTri[0]);
                //CreateSingnalSphere(startCurTri[1]);
                //CreateSingnalSphere(startCurTri[2]);
                Vector3 firstWaypoint = CheckTheNearstVerticesInTrianglesArray(startCurTri, transform.InverseTransformPoint(endPos));
                drawLineFromPointToAround(startCurTri, hitPos);

                Vector3[] aroundPointsTemp = GetAllVerticesIndexAroundTargetPos(hitPos);

                navPointsList.Add(hitPos);
                CreateSingnalSphere(hitPos);
                navPointsList.Add(firstWaypoint);

                Vector3 wayPos = Vector3.zero;
                while (wayPos != navPointsList[Mathf.Clamp(navPointsList.Count - 1, 0, navPointsList.Count)])
                {
                    Vector3[] aroundPoints = GetAllVerticesIndexAroundTargetPos(navPointsList[navPointsList.Count - 1]);
                    drawLineFromPointToAround(aroundPoints, navPointsList[navPointsList.Count - 1]);
                    Vector3 nearstWayPoint = CheckTheNearstVerticesInTrianglesArray(aroundPoints, transform.InverseTransformPoint(endPos));

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

                CalculateProperPointsPostion(navPointsList);

                Debug.DrawLine(transform.TransformPoint(navPointsList[0]), transform.TransformPoint(navPointsList[1]), Color.green, 1000f);
                Debug.DrawLine(transform.TransformPoint(navPointsList[1]), transform.TransformPoint(navPointsList[2]), Color.green, 1000f);
                Debug.DrawLine(endPos, transform.TransformPoint(navPointsList[navPointsList.Count - 2]), Color.green, 1000f);
                color = Color.green;
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

    Vector3 CalculateProperPointsPostion(List<Vector3> points)
    {
        for (int index = 0; index < points.Count - 3; ++index)
        {
            Vector3[] aroundPoints1;
            if (index == 0)
            {
                aroundPoints1 = startCurTri;
            }
            else
            {
                aroundPoints1 = GetAllVerticesIndexAroundTargetPos(navPointsList[index]);
            }
            if (aroundPoints1.Length != 0)
            {
                Vector3[] aroundPoints2 = GetAllVerticesIndexAroundTargetPos(navPointsList[index + 2]);
                List<Vector3> triCommon = new List<Vector3>();
                List<Vector3> tri1 = new List<Vector3>();
                List<Vector3> tri2 = new List<Vector3>();

                Vector3[] longArr = { };
                Vector3[] shortArr = { };
                if (aroundPoints1.Length >= aroundPoints2.Length)
                {
                    longArr = aroundPoints1;
                    shortArr = aroundPoints2;
                }
                else
                {
                    longArr = aroundPoints2;
                    shortArr = aroundPoints1;
                }

                for (int i = 1; i < longArr.Length; ++i)
                {
                    if (shortArr.ToList().Find(v => v == longArr[i]) == longArr[i])
                    {
                        triCommon.Add(longArr[i]);
                        tri1.Add(longArr[i]);
                        tri2.Add(longArr[i]);
                    }
                }
                tri1.Add(navPointsList[index]);
                tri2.Add(navPointsList[index + 2]);

                if (tri1.Count > 2 && tri2.Count > 2)
                {
                    Vector3 commVector = triCommon[1] - triCommon[0];
                    Vector3 normalCommonVector = commVector.normalized;
                    Vector3 side = tri2[2] - triCommon[0];
                    Vector3 sidePre = tri1[2] - triCommon[1];

                    float dot1 = Vector3.Dot(side, commVector);
                    Vector3 f1 = normalCommonVector * dot1 / commVector.magnitude;
                    Vector3 pos1 = triCommon[0] + f1;
                    float pedal1 = Vector3.Distance(tri2[2], pos1);

                    float dot2 = Vector3.Dot(sidePre, -commVector);
                    Vector3 f2 = -normalCommonVector * dot2 / commVector.magnitude;
                    Vector3 pos2 = triCommon[1] + f2;
                    float pedal2 = Vector3.Distance(tri1[2], pos2);

                    float addtiveMag = (pedal1 * Vector3.Distance(pos1, pos2))/(pedal1 + pedal2);
                    Vector3 addtiveVector = addtiveMag * normalCommonVector;

                    CreateSingnalSphere(triCommon[0] + f1 + addtiveVector);
                    CreateSingnalSphere(triCommon[1] + f2);

                    navPointsList[index + 1] = triCommon[0] + f1 + addtiveVector;
                }
            }
        }


        //Vector3[] aroundPoints1 = GetAllVerticesIndexAroundTargetPos(navPointsList[3]);
        //Vector3[] aroundPoints2 = GetAllVerticesIndexAroundTargetPos(navPointsList[5]);
        //foreach (Vector3 vv in aroundPoints2)
        //{
        //    GameObject s = CreateSingnalSphere(vv);
        //    float vl = Vector3.Distance(vv, transform.InverseTransformPoint(endPos));
        //    float y = float.Parse(vl.ToString("#0.00"));
        //}

        //List<Vector3> triCommon = new List<Vector3>();
        //for (int i = 1; i < aroundPoints1.Length; ++i)
        //{
        //    if (aroundPoints2.ToList().Find(v => v == aroundPoints1[i]) == aroundPoints1[i])
        //    {
        //        triCommon.Add(aroundPoints1[i]);
        //    }
        //}
        //List<Vector3> tri1 = triCommon;
        //tri1.Add(navPointsList[3]);

        //List<Vector3> tri2 = triCommon;
        //tri2.Add(navPointsList[5]);


        //Vector3 commVector = triCommon[1] - triCommon[0];
        //Vector3 f = commVector.normalized * Vector3.Dot(tri2[2] - triCommon[0], commVector) / commVector.magnitude;
        //CreateSingnalSphere(triCommon[0] + f);


        return Vector3.zero;
    }

    /// <summary>
    /// Create a sphere on the give position to show where the postion locate;
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    GameObject CreateSingnalSphere(Vector3 pos)
    {
        GameObject s = Instantiate(Resources.Load("end")) as GameObject;
        s.transform.position = transform.TransformPoint(pos);
        return s;
    }

    /// <summary>
    /// Draw lines from the give point to all the points who are around it
    /// </summary>
    /// <param name="aroundPoints"></param>
    /// <param name="points"></param>
    void drawLineFromPointToAround(Vector3[] aroundPoints, Vector3 points)
    {
        for (int i = 0; i < aroundPoints.Length; ++i)
        {
            //Debug.DrawLine(transform.TransformPoint(points), transform.TransformPoint(aroundPoints[i]), Color.red, 1000f);
        }
    }

    /// <summary>
    /// Get all the vectices postions around the give points
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    Vector3[] GetAllVerticesIndexAroundTargetPos(Vector3 targetPoint)
    {
        // get all indexs of the target postion used in the vectices arrary
        List<int> indexList = new List<int>();
        List<Vector3> resList = new List<Vector3>();
        for (int i = 0; i < vert.Length; ++i)
        {
            // the index get
            if (targetPoint == vert[i])
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

    // Calculate the postion vector which have the shortest distance between itself and the targetPos
    Vector3 CheckTheNearstVerticesInTrianglesArray(Vector3[] triCur, Vector3 endPos)
    {
        return CheckTheNearstVerticesByDot(triCur, endPos);
    }

    Vector3 CheckTheNearstVerticesByDot(Vector3[] triCur, Vector3 endPos)
    {
        Vector3 resVec = triCur[0];
        float dot = Vector3.Dot(triCur[0], endPos);
        foreach (Vector3 v in triCur)
        {
            float dotCompare = Vector3.Dot(v, endPos);
            if (dotCompare > dot)
            {
                dot = dotCompare;
                resVec = v;
            }
        }
        return resVec;
    }

    Vector3 CheckTheNearstVerticesByDistance(Vector3[] triCur, Vector3 endPos)
    {
        Vector3 resVec = triCur[0];
        float dis = Vector3.Distance(triCur[0], endPos);
        foreach (Vector3 v in triCur)
        {
            float disCompare = Vector3.Distance(v, endPos);
            if (disCompare < dis)
            {
                dis = disCompare;
                resVec = v;
            }
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
