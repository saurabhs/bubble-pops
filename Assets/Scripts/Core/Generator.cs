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

        [SerializeField] private Grid _grid = null;

        /// <summary>
        /// list of generated bubbles
        /// </summary>
        private HashSet<GameObject> _bubbles = new HashSet<GameObject>();

        public GameObject[] BubblePrefab => _bubblesPrefab;

        public HashSet<GameObject> Bubbles => _bubbles;

        private void Start() => Setup();

        private void Setup()
        {
            var rows = _grid.Rows;
            var coloumns = _grid.Coloumn;
            var index = 0;

            for (var j = 0; j < coloumns; j++)
            {
                for (var i = 0; i < rows; i++)
                {
                    var cell = _grid.GridData[index++].transform;
                    var bubble = CreateBubble(i, j, cell.position, transform);
                    cell.GetComponent<Cell>().bubble = bubble.GetComponent<Bubble>();
                }
            }
        }

        private GameObject CreateBubble(int row, int coloumn, Vector2 position, Transform parent)
        {
            var bubble = Instantiate(GetRandomBubble(), position, Quaternion.identity, parent);
            bubble.name = $"Bubble_{row}_{coloumn}";
            return bubble;
        }

        private GameObject GetRandomBubble() => _bubblesPrefab[UnityEngine.Random.Range(0, _bubblesPrefab.Length / 2)];
    }
}