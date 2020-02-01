using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BubblePops.Core
{
    public class Cell : MonoBehaviour, IPointerClickHandler
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
        private GameObject go = null;

        // [NaughtyAttributes.ReadOnly] public Vector2Int index = new Vector2Int(-1, -1);

        /// <summary>
        /// 
        /// </summary>
        public List<GameObject> _neighbours = new List<GameObject>();

        // public static void Setup(int rows, int coloumn)
        // {
        //     _rows = rows;
        //     _coloumns = coloumn;
        // }

        private void SetNeighbours()
        {
            var name = gameObject.name;
            var row = Int32.Parse(name.Split('_')[1]);
            var coloumn = Int32.Parse(name.Split('_')[2]);

            _neighbours = new List<GameObject>()
            {
                GameObject.Find($"Cell_{row - 1}_{coloumn - 1}"),
                GameObject.Find($"Cell_{row}_{coloumn - 1}"),
                GameObject.Find($"Cell_{row + 1}_{coloumn - 1}"),
                GameObject.Find($"Cell_{row - 1}_{coloumn}"),
                GameObject.Find($"Cell_{row + 1}_{coloumn}"),
                GameObject.Find($"Cell_{row}_{coloumn + 1}")
            };
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SetNeighbours();

            print("go " + gameObject.name);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            foreach(var cell in _neighbours)
            {
                print("neighbours " + cell.name);
                cell.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
    }
}