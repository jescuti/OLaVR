using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class CoralCSVtoPoints : MonoBehaviour
{
    private VoxelRenderer voxelRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voxelRenderer = GameObject.Find("Points2").GetComponent<VoxelRenderer>();
        ReadCSVFile();
    }

    void ReadCSVFile()
    {
        Debug.Log($"Starting setup");
        List<Vector3> positions = new List<Vector3>();
        List<Color> colors = new List<Color>();

        StreamReader sr = new StreamReader("Assets/Data/2025_coraltemp_v3.1_20251026-2.csv");
        bool eof = false;
        sr.ReadLine();
        Debug.Log($"Set up streamreader");

        while (!eof)
        {
            string line = sr.ReadLine();
            if (line == null)
            {
                eof = true;
                break;
            }

            float latitude;
            float longitude;
            float temp;
            try
            {
                var values = line.Split(',');
                latitude = float.Parse(values[1]);
                longitude = float.Parse(values[2]);
                temp = float.Parse(values[3]);
            }
            catch (FormatException e)
            {
                Debug.Log($"not a number");
                break;
            }

            Debug.Log($"Read {Normalize(latitude, 34, 36)}, {Normalize(longitude, -75, -76)}, {temp}");
            positions.Add(new Vector3(Normalize(latitude, 34, 36), Normalize(longitude, -76, -78), 0));
            colors.Add(new Color(0.5f, Normalize(temp, 18, 23), 0.5f));
        }
        voxelRenderer.SetVoxels(positions.ToArray(), colors.ToArray());
    }
    
    float Normalize(float x, float min, float max)
    {
        return (x - min) / (max - min);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
