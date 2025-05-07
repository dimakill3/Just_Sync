using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace _Assets.Scripts.Game.Utils
{
    public static class MapUtil
    {
        public static Vector3 GetRandomMapPosition()
        {
            var terrain = Terrain.activeTerrain;
            var terrainData = terrain.terrainData;
            var terrainPos = terrain.transform.position;
            
            var x = Random.Range(0f, terrainData.size.x - 30);
            var z = Random.Range(0f, terrainData.size.z - 30);
            var y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;

            return new Vector3(x + terrainPos.x, y, z + terrainPos.z);
        }
    }
}