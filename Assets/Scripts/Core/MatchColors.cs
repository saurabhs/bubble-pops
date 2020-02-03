using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BubblePops.Core
{
    public class MatchColors : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// 
        /// </summary>
        private Cell _cell;

        /// <summary>
        /// 
        /// </summary>
        private List<GameObject> _similarBubbles = new List<GameObject>();

        private List<GameObject> _neighbours = new List<GameObject>();

        private void Awake() => _cell = GetComponent<Cell>();

        private void GetMatchingNeighbours(Cell current, EType type)
        {
            for (var i = 0; i < _neighbours.Count; i++)
            {
                var temp = new List<GameObject>();
                var neighbour = _neighbours[i].GetComponent<Cell>();
                if (neighbour.bubble.Type == type)
                {
                    _similarBubbles.Add(neighbour.gameObject);
                    temp = neighbour._neighbours;
                }

                foreach (var newCell in temp)
                {
                    if (!_neighbours.Contains(newCell))
                        _neighbours.Add(newCell);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _similarBubbles = new List<GameObject>();

            _neighbours = new List<GameObject>();
            _neighbours.Add(_cell.gameObject);
            for (int i = 0; i < _cell.Neighbours.Count; i++)
                _neighbours.Add(_cell.Neighbours[i]);

            GetMatchingNeighbours(_cell, _cell.bubble.Type);
        }
    }
}