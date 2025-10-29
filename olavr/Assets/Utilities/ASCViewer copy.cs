using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class ASCViewer : MonoBehaviour
{
    private VoxelRenderer voxelRenderer;
    public float normalizeX;
    public float normalizeY;
    public float normalizeZ;
    // Start is called before the first frame update
    void Start()
    {
        voxelRenderer = GameObject.Find("PointCloud").GetComponent<VoxelRenderer>();
        if (voxelRenderer == null)
        {
            Debug.LogError("PointCloud not found.");
        }

        List<Vector3> positions = new List<Vector3>();
        List<Color> colors = new List<Color>();
        using (var sr = new StreamReader("Assets/Data/rilidar1.txt"))
        {
            string[] firstline = sr.ReadLine().Split(',');
            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(',');
                Debug.Log($"{line[0]}, {line[1]}, {line[2]}");
                positions.Add(new Vector3(float.Parse(line[0])-357000, float.Parse(line[1])-266000, float.Parse(line[2])));
                colors.Add(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
            }
        }
        voxelRenderer.SetVoxels(positions.ToArray(), colors.ToArray());

    }

    // Update is called once per frame
    void Update()
    {

    }
}
