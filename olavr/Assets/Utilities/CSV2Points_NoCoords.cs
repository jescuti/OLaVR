using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

// This utility class takes in a CSV holding a grid of data points WITHOUT latitute and longitude 
// and spawns a point cloud with colors corresponding to the data points.
public class CSV2Points_NoCoords : MonoBehaviour
{
    public enum ColorRamp
    {
        //SEQUENTIAl (color transition)
        RedYellow,
        GreenBlue,
        BlueIndigo,
        GreenYellow,
        BlueMagenta,
        BlackWhite,
        Cool,
        Spring,
        //SEQUENTIAL (white at min value)
        Iceberg,
        Emerald,
        Aubergine,
        Russet,
        Sulfur,
        //PERCEPTUALLY UNIFORM
        Mako,
        Inferno,
        Ocean
        
    }
    private VoxelRenderer voxelRenderer;
    public ColorRamp color = ColorRamp.RedYellow;
    public string object_name = "PointCloud";
    public string file_name = "Assets/Data/analysed_sst_fiji_coral.csv";
    public float max_value = 0;
    public float min_value = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voxelRenderer = GameObject.Find(name).GetComponent<VoxelRenderer>();
        ReadCSVFile();
    }

    void ReadCSVFile()
    {
        Debug.Log($"Starting setup");
        List<Vector3> positions = new List<Vector3>();
        List<Color> colors = new List<Color>();

        StreamReader sr = new StreamReader(file_name);
        bool eof = false;
        Debug.Log($"Set up streamreader");

        int row = 0; 

        while (!eof)
        {
            string line = sr.ReadLine();
            if (line == null)
            {
                eof = true;
                break;
            }

            var values = line.Split(',');
            
            try
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == "NaN")
                    {
                        continue;
                    }
                    float value = float.Parse(values[i]);
                    positions.Add(new Vector3(i, 0, row));
                    colors.Add(colorCreator(value));

                    Debug.Log($"Read {value} at {row}, {i}");
                }
            }
            catch (FormatException e)
            {
                Debug.Log($"not a number");
                break;
            }
            row++;
        }
        voxelRenderer.SetVoxels(positions.ToArray(), colors.ToArray());
    }
    
    float Normalize(float x, float min, float max)
    {
        return (x - min) / (max - min);
    }

    Color colorCreator(float value)
    {
        switch (color)
        {
            case ColorRamp.RedYellow:
                return new Color(1f, Normalize(value, min_value, max_value), 0f);
            case ColorRamp.GreenBlue:
                return new Color(0f, 1f, Normalize(value, min_value, max_value));
            case ColorRamp.BlueIndigo:
                return new Color(0f, Normalize(value, min_value, max_value), 1f);
            case ColorRamp.GreenYellow:
                return new Color(Normalize(value, min_value, max_value), 1f, 0f);
            case ColorRamp.BlueMagenta:
                return new Color(Normalize(value, min_value, max_value), 0f, 1f);
            case ColorRamp.BlackWhite:
                return new Color(Normalize(value, min_value, max_value), Normalize(value, min_value, max_value), Normalize(value, min_value, max_value));
            case ColorRamp.Cool:
                return new Color(Normalize(value, min_value, max_value), 1 - (Normalize(value, min_value, max_value)/2), 1f);
            case ColorRamp.Spring:
                return new Color(1f, 1 - (Normalize(value, min_value, max_value)/2), Normalize(value, min_value, max_value)/2);
            case ColorRamp.Iceberg:
                return new Color(1- Normalize(value, min_value, max_value), 1 - (Normalize(value, min_value, max_value)/2), 1f);
            case ColorRamp.Emerald:
                return new Color(1 - Normalize(value, min_value, max_value), 1 - Normalize(value, min_value, max_value)/2, 1 - Normalize(value, min_value, max_value)/2);
            case ColorRamp.Aubergine:
                return new Color(1 - Normalize(value, min_value, max_value)/2, 1 - Normalize(value, min_value, max_value), 1 - Normalize(value, min_value, max_value)/2);
            case ColorRamp.Russet:
                return new Color(1 - Normalize(value, min_value, max_value)/2, 1 - Normalize(value, min_value, max_value), 1 - Normalize(value, min_value, max_value));
            case ColorRamp.Sulfur:
                return new Color(1 - Normalize(value, min_value, max_value)/2, 1 - Normalize(value, min_value, max_value)/2, 1 - Normalize(value, min_value, max_value));
            case ColorRamp.Mako:
                if (value > max_value / 2)
                {
                    return new Color(0.3f + ((Normalize(value, min_value, max_value)-0.5f)/2), 0.5f + (Normalize(value, min_value, max_value)-0.5f), 0.6f + (Normalize(value, min_value, max_value)-0.5f)/4);
                }
                else
                {
                    return new Color(0.6f*Normalize(value, min_value, max_value) + (0.2f - 0.4f*Normalize(value, min_value, max_value)), Normalize(value, min_value, max_value) + (0.1f - 0.2f*Normalize(value, min_value, max_value)), 1.2f*Normalize(value, min_value, max_value)+ (0.25f - 0.5f*Normalize(value, min_value, max_value)));
                }
            case ColorRamp.Inferno:
                if (value > max_value / 2)
                {
                    return new Color(0.75f + ((Normalize(value, min_value, max_value)-0.5f)/2f), 0.25f + ((Normalize(value, min_value, max_value)-0.5f)*1.5f), 0.3f + (Normalize(value, min_value, max_value)-0.5f)/2f);
                }
                else
                {
                    return new Color(1.5f*Normalize(value, min_value, max_value) + (0.25f - 0.5f*Normalize(value, min_value, max_value)), 0.6f*Normalize(value, min_value, max_value) + (0.1f - 0.2f*Normalize(value, min_value, max_value)), 0.6f*Normalize(value, min_value, max_value)+ (0.5f - Normalize(value, min_value, max_value)));
                }
            case ColorRamp.Ocean:
                if (value > max_value / 2)
                {
                    return new Color(2*Normalize(value, min_value, max_value) - 1f, 0.75f + ((Normalize(value, min_value, max_value)-0.5f)/2), 0.9f + (Normalize(value, min_value, max_value)-0.5f)/5f);
                }
                else
                {
                    return new Color(0f, 1.5f*Normalize(value, min_value, max_value) + (0.1f - 0.2f*Normalize(value, min_value, max_value)), 1.8f*Normalize(value, min_value, max_value)+ (0.75f -1.5f*Normalize(value, min_value, max_value)));
                }
            default:
                return new Color(1f, Normalize(value, min_value, max_value), 0f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
