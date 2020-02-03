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
        [SerializeField] private GameObject _nextBubble = null;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private GameObject _laterBubble = null;

        /// <summary>
        /// 
        /// </summary>
        private GameObject[] _bubblePrefabs = null;

        private void Awake()
        {
            var generator = FindObjectOfType<Generator>();
            _bubblePrefabs = generator.BubblePrefab;
        }

        private void OnEnable()
        {
            UpdateQueue();
        }

        [NaughtyAttributes.Button]
        public void UpdateQueue()
        {
            var generator = FindObjectOfType<Generator>();
            _nextBubble = _laterBubble == null ? GetRandomBubble(generator.GetRandomBubble(), _nextBubbleParent.transform) : _laterBubble;
            _nextBubble.transform.parent = _nextBubbleParent.transform;
            _nextBubble.transform.localPosition = Vector2.zero;
            _nextBubble.transform.localScale = Vector3.one;

            _laterBubble = GetRandomBubble(generator.GetRandomBubble(), _laterBubbleParent.transform);
        }

        private GameObject GetRandomBubble(GameObject prefab, Transform parent)
        {
            var bubble = Instantiate(prefab, Vector2.zero, Quaternion.identity, parent);
            bubble.transform.localPosition = Vector2.zero;
            return bubble;
        }
    }
}