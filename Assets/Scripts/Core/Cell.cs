using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BubblePops.Core
{
    public class Cell : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Bubble _bubble = null;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private GameObject _ghostSprite = null;

        /// <summary>
        /// 
        /// </summary>
        private List<Cell> _neighbours = new List<Cell>();

        /// <summary>
        /// 
        /// </summary>
        private List<GameObject> _ceilingNeighbours = new List<GameObject>();

        /// <summary>
        /// 
        /// </summary>
        public TextMeshProUGUI label = null;

        public Bubble BubbleObj => _bubble;
        public GameObject GhostSprite => _ghostSprite;
        public List<Cell> Neighbours => _neighbours;

        private void Start()
        {
            var ss = gameObject.name.Split('_');
            label.text = $"{int.Parse(ss[1])},{int.Parse(ss[2])}";
        }

        public void SetNeighbours()
        {
            var name = gameObject.name;
            var x = int.Parse(name.Split('_')[1]);
            var y = int.Parse(name.Split('_')[2]);

            var neighbours = new List<GameObject>()
            {
                GameObject.Find($"Cell_{x - 1}_{(x % 2 == 0 ? y : y - 1)}"),
                GameObject.Find($"Cell_{x}_{y - 1}"),
                GameObject.Find($"Cell_{x + 1}_{(x % 2 == 0 ? y : y - 1)}"),
                GameObject.Find($"Cell_{x + 1}_{(x % 2 == 0 ? y + 1 : y)}"),
                GameObject.Find($"Cell_{x}_{y + 1}"),
                GameObject.Find($"Cell_{x -1 }_{(x % 2 == 0 ? y + 1 : y)}")
            };

            foreach(var go in neighbours)
            {
                if(go != null)
                    _neighbours.Add(go.GetComponent<Cell>());
            }
        }

        [NaughtyAttributes.Button]
        public void RemoveOrphan()
        {
            if(_bubble == null)
                return;

            var name = gameObject.name;
            var x = int.Parse(name.Split('_')[1]);
            var y = int.Parse(name.Split('_')[2]);

            if (y == 0)
            {
                if (_neighbours.Count == 0)
                    OnOrphan();
            }
            else
            {
                _ceilingNeighbours = new List<GameObject>()
                {
                    GameObject.Find($"Cell_{x - 1}_{(x % 2 == 0 ? y : y - 1)}"),
                    GameObject.Find($"Cell_{x}_{y - 1}"),
                    GameObject.Find($"Cell_{x + 1}_{(x % 2 == 0 ? y : y - 1)}"),
                };

                var count = 0;
                foreach (var n in _ceilingNeighbours)
                {
                    if (n == null || n.GetComponent<Cell>()._bubble == null)
                        count++;
                }
                if (count == _ceilingNeighbours.Count)
                {
                    OnOrphan();
                }
            }
        }

        private void OnOrphan()
        {
            var rb = _bubble.gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = Random.Range(0.5f, 2.5f);
            _bubble = null;
            Destroy(_bubble, 3f);
        }

        public void SetBubble(Bubble bubble)
        {
            this._bubble = bubble;
            gameObject.layer = bubble == null ? LayerMask.NameToLayer("Empty") : LayerMask.NameToLayer("Occupied");
        }
    }
}