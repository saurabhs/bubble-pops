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

        private List<GameObject> GetMatchingNeighbours(Cell current, EType type)
        {
            var similarBubbles = new List<GameObject>();
            for (var i = 0; i < _neighbours.Count; i++)
            {
                var temp = new List<GameObject>();
                var neighbour = _neighbours[i].GetComponent<Cell>();
                if (neighbour.bubble.Type == type)
                {
                    similarBubbles.Add(neighbour.gameObject);
                    temp = neighbour._neighbours;
                }

                foreach (var newCell in temp)
                {
                    if (!_neighbours.Contains(newCell))
                        _neighbours.Add(newCell);
                }
            }
            return similarBubbles;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var resultBubble = OnMatchColours();
            if(resultBubble == -1)
                return;

            var ss = _cell.bubble.name.Split('_');
            var row = int.Parse(ss[1]);
            var coloumn = int.Parse(ss[2]);

            for (var i = 0; i < _similarBubbles.Count; i++)
            {
                Destroy(_similarBubbles[i].GetComponent<Cell>().bubble.gameObject);
            }
            var prefab = Resources.Load($"Prefabs/Bubble{resultBubble}") as GameObject;
            var newBubble = Generator.CreateBubble(prefab, row, coloumn, _cell.gameObject.transform.position);
            newBubble.name = $"Bubble_{row}_{coloumn}";

            _cell.bubble = newBubble.GetComponent<Bubble>();
        }

        private int OnMatchColours()
        {
            _similarBubbles = new List<GameObject>();

            _neighbours = new List<GameObject>();
            _neighbours.Add(_cell.gameObject);
            for (int i = 0; i < _cell.Neighbours.Count; i++)
                _neighbours.Add(_cell.Neighbours[i]);

            _similarBubbles = GetMatchingNeighbours(_cell, _cell.bubble.Type);
            if (_similarBubbles.Count == 1)
                return -1;

            foreach (var cell in _similarBubbles)
            {
                cell.GetComponent<Cell>().bubble.GetComponent<SpriteRenderer>().color = Color.magenta;
            }

            return (int)_cell.bubble.Type * (int)Mathf.Pow(2, _similarBubbles.Count - 1);
        }
    }
}