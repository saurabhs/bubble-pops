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
        /// max rows
        /// </summary>
        [SerializeField] private int _rows = 6;

        /// <summary>
        /// max coloumn
        /// </summary>
        [SerializeField] private int _coloumns = 4;

        /// <summary>
        /// parent object to group the bubbles
        /// </summary>
        [SerializeField] private GameObject _parent = null;

        /// <summary>
        /// horizonatal and vertical offset for placing bubbles
        /// </summary>
        [SerializeField] private Vector2 _offset = new Vector2(0.83f, -0.7249f);

        /// <summary>
        /// offset for alternate rows
        /// </summary>
        [SerializeField] private float _alternateOffset = 0.4113f;

        /// <summary>
        /// list of generated bubbles
        /// </summary>
        private HashSet<GameObject> _bubbles = new HashSet<GameObject>();


        public GameObject[] BubblePrefab => _bubblesPrefab;

        public HashSet<GameObject> Bubbles => _bubbles;

        private void OnEnable()
        {
            Setup();
        }

        private void Setup()
        {
            for (var j = 0; j < _coloumns; j++)
            {
                for (var i = 0; i < _rows; i++)
                {
                    _bubbles.Add(CreateBubble(i, j, j % 2 == 1 ? _alternateOffset : 0));
                }
            }
        }

        private GameObject CreateBubble(int row, int coloumn, float offset = 0f)
        {
            var position = new Vector2((_offset.x * row) + offset, _offset.y * coloumn);
            var bubble = Instantiate(GetRandomBubble(), position, Quaternion.identity, _parent.transform);
            bubble.transform.localPosition = position;
            bubble.name = $"Bubble|{row}x{coloumn}";

            return bubble;
        }

        public void CreateNewRow(int coloumn)
        {
            for (var i = 0; i < _rows; i++)
                _bubbles.Add(CreateBubble(i, coloumn, coloumn % 2 == 1 ? _alternateOffset : 0));
        }

        private GameObject GetRandomBubble() => _bubblesPrefab[UnityEngine.Random.Range(0, _bubblesPrefab.Length / 2)];
    }
}