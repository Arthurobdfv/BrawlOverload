using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class MapGenerator : MonoBehaviour
    {
        public MapTerrain[,] map;

        public MapTerrain m_terrain;

        public MapTerrain[] m_selection;

        [Range(1f, 10f)]
        public int HeightAdjustment;

        public int MapSize;

        private void Awake()
        {
            MapSize = 15;
            Injector.RegisterContainer(this);
        }

        void GenerateTerrain()
        {
            var heigths = NoiseGenerator.GenerateNoiseMap(MapSize, MapSize, 10);
            map = new MapTerrain[2 * MapSize, MapSize];
            for (int i = 0; i < MapSize * MapSize; i++)
            {
                var x = (int)Mathf.Floor(i / MapSize);
                var z = (int)Mathf.Floor(i % MapSize);
                map[x, z] = Instantiate(m_terrain, new Vector3(x, Mathf.Floor(heigths[x, z] * 10) / 2, z) + transform.position, Quaternion.identity);
            }
        }

        public void GenerateTerrain(Vector3[] positions)
        {
            for(int i = 0; i<positions.Length; i++)
            {
                Instantiate(m_terrain, positions[i], Quaternion.identity, transform);
            }
        }
    }

}

