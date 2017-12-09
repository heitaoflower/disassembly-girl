using UnityEngine;
using System.Collections.Generic;
using System;

namespace Map
{
    public class AbstractMapDirector : MonoBehaviour
    {
        private static IDictionary<int, Type> directorRegistry = new Dictionary<int, Type>()
        {
            { 04, typeof(MapDungeonA04Director)},
            { 06, typeof(MapDungeonA06Director)},
            { 07, typeof(MapDungeonA07Director)},
            { 14, typeof(MapDungeonA14Director)},
            { 21, typeof(MapDungeonA21Director)},
            { 22, typeof(MapDungeonA22Director)},
            { 23, typeof(MapDungeonA23Director)},
            { 24, typeof(MapDungeonA24Director)},
            { 100, typeof(MapHome01Director)},            
        };

        private void Start()
        {
            RegisterElements();
        }

        protected virtual void RegisterElements()
        {

        }

        public static AbstractMapDirector Bind(tk2dTileMap map, int directorID)
        {
            Type directorType = null;

            if (directorRegistry.TryGetValue(directorID, out directorType))
            {
                return map.gameObject.AddComponent(directorType) as AbstractMapDirector;
            }

            return null;
        }
    }
}