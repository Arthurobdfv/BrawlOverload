using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class MapGenerator : MonoBehaviour
    {
        public int MapSize
        {
            get { return m_mapSize; }
            set
            {
                if (value != m_mapSize)
                {
                    m_mapSize = value;
                }
            }
        }

        public MapTerrain[,] map;

        public MapTerrain m_terrain;

        public MapTerrain[] m_selection;

        [Range(1f, 10f)]
        public int HeightAdjustment;

        private int m_mapSize;

        private void Awake()
        {
            MapSize = 15;
            GenerateTerrain();
        }

        void GenerateTerrain()
        {
            var heigths = NoiseGenerator.GenerateNoiseMap(MapSize, MapSize, 10);
            map = new MapTerrain[2 * MapSize, MapSize];
            for (int i = 0; i < MapSize * MapSize; i++)
            {
                var x = (int)Mathf.Floor(i / MapSize);
                var z = (int)Mathf.Floor(i % MapSize);
                map[x, z] = Instantiate(m_terrain, new Vector3(x, Mathf.Floor(heigths[x, z] * 10) / 2, z), Quaternion.identity);
            }
        }
    }

}

