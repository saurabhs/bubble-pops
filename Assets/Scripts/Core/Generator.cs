using System;
using System.Collections.Generic;
using UnityEngine;

namespace BubblePops.Core
{
    public class Generator : MonoBehaviour
    {
        /// <summary>
        /// collection of bubbles prefabs
        /// </summary>
        [SerializeField] private GameObject[] _bubblesPrefab = null;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Grid _grid = null;

        /// <summary>
        /// list of generated bubbles
        /// </summary>
        private HashSet<GameObject> _bubbles = new HashSet<GameObject>();

        ///// <summary>
        ///// 
        ///// </summary>
        //[SerializeField] private int _lowestColoumn = 0;

        public GameObject[] BubblePrefab => _bubblesPrefab;

        public HashSet<GameObject> Bubbles => _bubbles;

        //public int LowestColoumn => _lowestColoumn;

        private void Start() => Setup();

        private void Setup()
        {
            var rows = _grid.Rows;
            var coloumns = _grid.Coloumn;
            var index = 0;

            for(var j = 0; j < coloumns; j++)
            {
                for(var i = 0; i < rows; i++)
                {
                    var cell = _grid.GridData[index++].transform;
                    var bubble = CreateBubble(GetRandomBubble(), i, j, cell.position);
                    cell.GetComponent<Cell>().bubble = bubble.GetComponent<Bubble>();
                }
            }

            PostSetup();
        }

        private void PostSetup()
        {
            var rows = _grid.Rows;
            var coloumns = _grid.Coloumn;
            var index = 0;

            for(var j = 0; j < coloumns + 1; j++)
            {
                for(var i = 0; i < rows; i++)
                {
                    var cell = _grid.GridData[index++].GetComponent<Cell>();
                    cell.SetNeighbours();
                }
            }
        }

        public System.Collections.IEnumerator Reset()
        {
            for(var i = _grid.GridData.Count - 1; i >= 0; i--)
            {
                var cell = _grid.GridData[i].GetComponent<Cell>();
                if(cell.bubble == null)
                    continue;
                var rb = cell.bubble.gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = UnityEngine.Random.Range(0.5f, 2.5f);
                Destroy(cell.bubble.gameObject, 3f);
            }

            yield return new WaitForSeconds(0.75f);

            Setup();
        }

        public GameObject CreateBubble(GameObject prefab, int row, int coloumn, Vector2 position)
        {
            var bubble = Instantiate(prefab, position, Quaternion.identity);
            bubble.name = $"Bubble_{row}_{coloumn}";
            return bubble;
        }

        public GameObject GetRandomBubble() => _bubblesPrefab[UnityEngine.Random.Range(0, 8)];
    }
}