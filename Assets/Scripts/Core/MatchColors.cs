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
        private List<GameObject> _neighbours = new List<GameObject>();

        private void Awake() => _cell = GetComponent<Cell>();

        private List<GameObject> GetMatchingNeighbours(Cell cell)
        {
            var similarBubbles = new List<GameObject>();
            if(cell.BubbleObj == null)
                return similarBubbles;

            var type = cell.BubbleObj.Type;

            for (var i = 0; i < _neighbours.Count; i++)
            {
                var temp = new List<GameObject>();
                var neighbour = _neighbours[i].GetComponent<Cell>();
                if (neighbour.BubbleObj != null && neighbour.BubbleObj.Type == type)
                {
                    similarBubbles.Add(neighbour.gameObject);
                    temp = neighbour.Neighbours;
                }

                foreach (var newCell in temp)
                {
                    if (!_neighbours.Contains(newCell))
                        _neighbours.Add(newCell);
                }
            }
            return similarBubbles;
        }

#if UNITY_EDITOR
        public void OnPointerClick(PointerEventData eventData)
        {
            GroupBubbles();
        }
#endif

        public void GroupBubbles()
        {
            var similarBubbles = OnMatchColours();
            if (similarBubbles.Count < 2)
            {
                FindObjectOfType<Grid>().UpdateGrid();
                return;
            }

            var resultValue = (int)_cell.BubbleObj.Type * (int)Mathf.Pow(2, similarBubbles.Count - 1);
            if (resultValue >= (int)EType.TwentyFortyEight)
            {
                StartCoroutine(FindObjectOfType<Generator>().Reset());
            }
            else
            {
                var ss = _cell.BubbleObj.name.Split('_');
                var row = int.Parse(ss[1]);
                var coloumn = int.Parse(ss[2]);

                for (var i = 0; i < similarBubbles.Count; i++)
                {
                    var cell = similarBubbles[i].GetComponent<Cell>();
                    //StartCoroutine(AnimateThenDestroy(cell));

                    Destroy(cell.BubbleObj.gameObject);
                    cell.SetBubble(null);
                }

                var prefab = Resources.Load($"Prefabs/Bubble{resultValue}") as GameObject;
                var newBubble = FindObjectOfType<Generator>().CreateBubble(prefab, row, coloumn, _cell.gameObject.transform.position);
                newBubble.name = $"Bubble_{row}_{coloumn}";

                _cell.SetBubble(newBubble.GetComponent<Bubble>());
                _cell.SetNeighbours();
                FindObjectOfType<Grid>().UpdateGrid();
                
                GroupBubbles();
            }
        }

        private List<GameObject> OnMatchColours()
        {
            _neighbours = new List<GameObject>();
            _neighbours.Add(_cell.gameObject);
            for (var i = 0; i < _cell.Neighbours.Count; i++)
                _neighbours.Add(_cell.Neighbours[i]);

            return GetMatchingNeighbours(_cell);
        }
    }
}