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
        public List<GameObject> _neighbours = new List<GameObject>();

        /// <summary>
        /// 
        /// </summary>
        public TextMeshProUGUI label = null;

        public List<GameObject> Neighbours => _neighbours;

        private void Start()
        {
            SetNeighbours();
        }

        private void SetNeighbours()
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
            for (int i = _neighbours.Count - 1; i >= 0; i--)
            {
                if(_neighbours[i] == null)
                    _neighbours.RemoveAt(i);
            }
        }

        // public void OnPointerClick(PointerEventData eventData)
        // {
        //     SetNeighbours();

        //     gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        //     foreach(var cell in _neighbours)
        //     {
        //         cell.GetComponent<SpriteRenderer>().color = Color.magenta;
        //     }
        // }
    }
}