using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slicer2D;
using Utilities2D;

public class SliceController : MonoBehaviour
{

    public Transform pointA;
    public Transform pointB;


    private void Start()
    {
        EventMgr.GetInstance().AddLinstener<bool>("Slice", Slice);
    }

    public void Slice(bool slice)
    {
        if (slice)
        {
            Pair2D pair = new Pair2D(pointA.position, pointB.position);
            List<Slice2D> results = Slicing.LinearSliceAll(pair);

            foreach (Slice2D id in results)
            {
                List<Polygon2D> polygons = id.GetPolygons();
                int minIndex = 0;
                float minHeight = polygons[0].GetBounds().center.y;

                for (int i = 0; i < polygons.Count; i++)
                {
                    if (polygons[i].GetBounds().center.y < minHeight)
                    {
                        minIndex = i;
                        minHeight = polygons[i].GetBounds().center.y;
                    }
                }

                id.GetGameObjects()[minIndex].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                AddForce.LinearSlice(id, 100);
            }
        }
    }
}
