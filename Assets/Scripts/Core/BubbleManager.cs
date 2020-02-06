using System.Collections.Generic;
using UnityEngine;

namespace BubblePops.Core
{
    public class BubbleManager : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private GameObject _nextBubbleParent = null;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private GameObject _laterBubbleParent = null;

        /// <summary>
        /// 
        /// </summary>
        private GameObject _nextBubble = null;

        /// <summary>
        /// 
        /// </summary>
        private GameObject _laterBubble = null;

        /// <summary>
        /// 
        /// </summary>
        private GameObject[] _bubblePrefabs = null;

        /// <summary>
        /// 
        /// </summary>
        private Grid _grid = null;

        private void Awake()
        {
            var generator = FindObjectOfType<Generator>();
            _bubblePrefabs = generator.BubblePrefab;

            _grid = FindObjectOfType<Grid>();
        }

        private void Start()
        {
            UpdateQueue();
        }

        [NaughtyAttributes.Button]
        public void UpdateQueue()
        {
            _nextBubble = _laterBubble == null ? GetRandomBubbleFromLastColoumn(_nextBubbleParent.transform) : _laterBubble;
            _nextBubble.transform.parent = _nextBubbleParent.transform;
            _nextBubble.transform.localPosition = Vector2.zero;
            _nextBubble.transform.localScale = Vector3.one;
            _nextBubble.AddComponent<Move>();

            _laterBubble = GetRandomBubbleFromLastColoumn(_laterBubbleParent.transform);

            _nextBubbleParent.GetComponent<Shoot>().SetChild(_nextBubble.transform);
        }

        private GameObject GetRandomBubble(GameObject prefab, Transform parent)
        {
            var bubble = Instantiate(prefab, Vector2.zero, Quaternion.identity, parent);
            bubble.transform.localPosition = Vector2.zero;
            bubble.name = $"Bubble_0_0";
            return bubble;
        }

        private GameObject GetRandomBubbleFromLastColoumn(Transform parent)
        {
            var prefabs = new List<GameObject>();
            var lowest = _grid.Coloumn;
            var isValid = true;

            while(isValid)
            {
                foreach(var cell in _grid.GridData)
                {
                    var col = int.Parse(cell.name.Split('_')[2]);
                    if(col == lowest - 1)
                    {
                        if(cell.BubbleObj != null)
                            prefabs.Add(cell.BubbleObj.gameObject);
                    }
                }
                isValid = prefabs.Count < 2;
                if(isValid)
                    lowest--;
            }
            return GetRandomBubble(prefabs[Random.Range(0, prefabs.Count)], parent);
        }
    }
}