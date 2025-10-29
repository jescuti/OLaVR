using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VoxelRenderer : MonoBehaviour
{
    ParticleSystem system;
    ParticleSystem.Particle[] voxels;
    bool voxelsUpdated = false;
    public float voxelScale = 1f;
    public float scale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //TESTING
        Vector3[] positions = new Vector3[] { new Vector3(0, 1, 0), new Vector3(1, 1, 1) };
        Color[] colors = new Color[] { new Color(Random.value, Random.value, Random.value), new Color(Random.value, Random.value, Random.value) };
        SetVoxels(positions, colors);

        system = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (voxelsUpdated)
        {
            system.SetParticles(voxels, voxels.Length);
            voxelsUpdated = false;
        }
        //Debug.Log("Updated voxels");
    }


    // Sets a voxel at each position in vector
    public void SetVoxels(Vector3[] positions, Color[] colors)
    {
        voxels = new ParticleSystem.Particle[positions.Length];


        for (int i = 0; i < positions.Length; i++)
        {
            voxels[i].position = positions[i] * scale;
            Debug.Log($"Find them at {positions[i]}");
            voxels[i].startColor = colors[i];
            voxels[i].startSize = voxelScale;
        }

        Debug.Log($"Set voxels for {positions.Length} points");
        voxelsUpdated = true;
    }
}
