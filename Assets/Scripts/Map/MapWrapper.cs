using UnityEngine;
using System;
using System.Collections.Generic;
using Utils;

namespace Map
{
    public enum MapType : int
    {
        Home = 0,
        Dungeon,
        Count
    }

    public class MapWrapper
    {
        public tk2dTileMap map = null;

        public Nullable<Vector3> girlSpawnPoint = null;

        public Nullable<Vector3> petSpwanPoint = null;

        public Nullable<Vector3> monsterInPoint = null;

        public Nullable<Vector3> monsterOutPoint = null;

        public IList<SpwanPoint2D> bossSpawnPoints = null;
        
        public IDictionary<SpwanPointIds, IList<SpwanPoint2D>> monsterSpawnPoints  = null;

        public MapType type = default(MapType);

        public AbstractMapDirector director = null;

        public int directorID = 0;

        public MapWrapper(tk2dTileMap map, MapType type, int directorID = 0)
        {  
            this.map = map;

            this.type = type;

            this.directorID = directorID;

            Config();

        }

        private void Config()
        {
            Vector3 tileOrigin = map.data.tileOrigin;
            map.transform.position = new Vector3(-tileOrigin.x + (map.data.tileSize.x * map.width) / -2f, -tileOrigin.y + (map.data.tileSize.y * map.height) / -2f);
            map.renderData.transform.position = new Vector3(-tileOrigin.x + (map.data.tileSize.x * map.width) / -2f, -tileOrigin.y + (map.data.tileSize.y * map.height) / -2f);

            girlSpawnPoint = GameObject.FindGameObjectWithTag(TagDefines.TAG_GIRL_SPWAN_POINT).transform.position;

            petSpwanPoint = GameObject.FindGameObjectWithTag(TagDefines.TAG_PET_SPWAN_POINT).transform.position;

            if (type == MapType.Dungeon)
            {
                if (GameObject.FindGameObjectWithTag(TagDefines.TAG_MONSTER_IN_POINT) != null)
                {
                    monsterInPoint = GameObject.FindGameObjectWithTag(TagDefines.TAG_MONSTER_IN_POINT).transform.position;
                }

                if (GameObject.FindGameObjectWithTag(TagDefines.TAG_MONSTER_OUT_POINT) != null)
                {
                    monsterOutPoint = GameObject.FindGameObjectWithTag(TagDefines.TAG_MONSTER_OUT_POINT).transform.position;
                }
                                            
                bossSpawnPoints = new List<SpwanPoint2D>();

                foreach (GameObject go in GameObject.FindGameObjectsWithTag(TagDefines.TAG_BOSS_SPAWN_POINT))
                {
                    bossSpawnPoints.Add(go.GetComponent<SpwanPoint2D>());
                }

                monsterSpawnPoints = new Dictionary<SpwanPointIds, IList<SpwanPoint2D>>();

                foreach (GameObject go in GameObject.FindGameObjectsWithTag(TagDefines.TAG_MONSTER_SPAWN_POINT))
                {
                    SpwanPoint2D point = go.GetComponent<SpwanPoint2D>();

                    IList<SpwanPoint2D> points = null;
                    if (!monsterSpawnPoints.TryGetValue(point.type, out points))
                    {
                        points = new List<SpwanPoint2D>();
                        monsterSpawnPoints.Add(point.type, points);
                    }

                    points.Add(point);
                }
            }

            if (directorID != 0)
            {
                director = AbstractMapDirector.Bind(map, directorID);
            }
    
        }
    }
}