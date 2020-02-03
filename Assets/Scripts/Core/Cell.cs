using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BubblePops.Core
{
    public class Cell : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private static int _rows = -1;

        /// <summary>
        /// 
        /// </summary>
        private static int _coloumns = -1;

        /// <summary>
        /// 
        /// </summary>
        public Bubble bubble = null;

        /// <summary>
        /// 
        /// </summary>
        private List<GameObject> _neighbours = new List<GameObject>();

        /// <summary>
        /// 
        /// </summary>
        public TextMeshProUGUI label = null;

        public List<GameObject> Neighbours => _neighbours;

        public void SetNeighbours()
        {
            var name = gameObject.name;
            var x = Int32.Parse(name.Split('_')[1]);
            var y = Int32.Parse(name.Split('_')[2]);

            _neighbours = new List<GameObject>()
            {
                GameObject.Find($"Cell_{x - 1}_{(x % 2 == 0 ? y : y - 1)}"),
                GameObject.Find($"Cell_{x}_{y - 1}"),
                GameObject.Find($"Cell_{x + 1}_{(x % 2 == 0 ? y : y - 1)}"),
                GameObject.Find($"Cell_{x + 1}_{(x % 2 == 0 ? y + 1 : y)}"),
                GameObject.Find($"Cell_{x}_{y + 1}"),
                GameObject.Find($"Cell_{x -1 }_{(x % 2 == 0 ? y + 1 : y)}")
            };
            // print($"pre {gameObject.name} {_neighbours.Count}");

            for (int i = _neighbours.Count - 1; i >= 0; i--)
            {
                if (_neighbours[i] == null || _neighbours[i].GetComponent<Cell>().bubble == null)
                    _neighbours.RemoveAt(i);
            }

            // print($"post {gameObject.name} {_neighbours.Count}");
        }

        public List<GameObject> ceilingNeighbours = new List<GameObject>();

        [NaughtyAttributes.Button]
        public void RemoveOrphan()
        {
            if(bubble == null)
                return;

            var name = gameObject.name;
            var x = Int32.Parse(name.Split('_')[1]);
            var y = Int32.Parse(name.Split('_')[2]);

            if (y == 0)
            {
                if (_neighbours.Count == 0)
                    OnOrphan();
            }
            else
            {
                ceilingNeighbours = new List<GameObject>()
                {
                    GameObject.Find($"Cell_{x - 1}_{(x % 2 == 0 ? y : y - 1)}"),
                    GameObject.Find($"Cell_{x}_{y - 1}"),
                    GameObject.Find($"Cell_{x + 1}_{(x % 2 == 0 ? y : y - 1)}"),
                };

                var count = 0;
                foreach (var n in ceilingNeighbours)
                {
                    if (n == null || n.GetComponent<Cell>().bubble == null)
                        count++;
                }
                if (count == ceilingNeighbours.Count)
                {
                    print($"cell {gameObject.name}, bubble {bubble.name}, ceilingNeighbours {count}");
                    OnOrphan();
                }
            }
        }

        private void OnOrphan()
        {
            print($"{gameObject.name} -> {bubble.name} -> OnOrphan");
            var rb = bubble.gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = UnityEngine.Random.Range(0.5f, 2.5f);
            bubble = null;
            Destroy(bubble, 3f);
        }
    }
}