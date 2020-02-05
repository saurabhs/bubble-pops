using System;
using System.Collections.Generic;
using UnityEngine;

namespace BubblePops.Core
{
    public class Shoot : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private float _angle = 0f;

        /// <summary>
        /// 
        /// </summary>
        private Transform _child = null;

        /// <summary>
        /// 
        /// </summary>
        private Queue<Transform> _reflectQueue = new Queue<Transform>();

        [SerializeField, NaughtyAttributes.MinMaxSlider(-75, 75)]
        private Vector2 _angleRange = Vector2.zero;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private LineRenderer _path;

        /// <summary>
        /// 
        /// </summary>
        private Cell _masterCell = null;

        /// <summary>
        /// 
        /// </summary>
        private Cell _reflectCell = null;

        /// <summary>
        /// 
        /// </summary>
        private Vector3 _start = Vector2.zero;

        private void Update()
        {
            if(Input.GetMouseButton(0))
            {
                var drag = Input.mousePosition - _start;
                _angle = drag.x / 3;
                if(_angle < _angleRange.x)
                    _angle = _angleRange.x;
                else if(_angle > _angleRange.y)
                    _angle = _angleRange.y;

                var result = new List<RaycastHit2D>();
                var count = Physics2D.Raycast(_child.position, GetDirection(_angle), new ContactFilter2D(), result);

                if(count > 0)
                {
                    ClearHighlights();

                    _reflectQueue.Clear();
                    _reflectCell = null;

                    var collider = GetLastEmptyCell(result);
                    _masterCell = collider.gameObject.GetComponent<Cell>();

                    if(_masterCell != null)
                        _masterCell.GhostSprite.SetActive(true);

                    _path.positionCount = 2;
                    _path.SetPosition(1, _masterCell.transform.position);

                    if(_masterCell.Neighbours[1].GetComponent<Cell>().BubbleObj == null)
                    {
                        _reflectQueue.Enqueue(_masterCell.transform);

                        var reflectResult = new List<RaycastHit2D>();
                        var reflectCount = Physics2D.Raycast(_masterCell.transform.position, GetDirection(_angle > 0 ? _angle - 90 : _angle + 90), new ContactFilter2D(), reflectResult);

                        if(reflectCount > 0)
                        {
                            if(_reflectCell)
                                _reflectCell.GhostSprite.SetActive(false);

                            _reflectCell = GetLastEmptyCell(reflectResult).gameObject.GetComponent<Cell>();

                            _path.positionCount = 3;
                            _path.SetPosition(2, _reflectCell.transform.position);

                            _reflectCell.GhostSprite.SetActive(true);
                            _masterCell.GhostSprite.SetActive(false);
                        }
                    }
                }
            }
            if(Input.GetMouseButtonDown(0))
            {
                _start = Input.mousePosition;
                _path.SetPosition(0, transform.position);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                if(_reflectCell != null)
                {
                    _reflectQueue.Enqueue(_reflectCell.transform);
                    var isOrpahn = true;
                    foreach(var n in _reflectCell.Neighbours)
                    {
                        if(n.GetComponent<Cell>().BubbleObj != null)
                        {
                            isOrpahn = false;
                            break;
                        }
                    }

                    if(isOrpahn)
                        _reflectQueue.Clear();
                }

                if(_reflectQueue.Count == 2)
                {
                    _child.GetComponent<Move>().Execute(_reflectQueue);
                }
                else if(_masterCell != null && _masterCell.Neighbours[1].GetComponent<Cell>().BubbleObj != null)
                {
                    Execute(_masterCell.gameObject);
                }

                _path.positionCount = 1;
                ClearHighlights();
            }
        }

        private Collider2D GetLastEmptyCell(List<RaycastHit2D> result)
        {
            var collider = result[0].collider;
            for(var i = 0; i < result.Count; i++)
            {
                if(result[i].collider.gameObject.GetComponent<Cell>().BubbleObj == null)
                    collider = result[i].collider;
                else
                    break;
            }

            return collider;
        }

        private void ClearHighlights()
        {
            if(_masterCell)
                _masterCell.GhostSprite.SetActive(false);
            if(_reflectCell)
                _reflectCell.GhostSprite.SetActive(false);
        }

        public void SetChild(Transform child) => _child = child;

        private Cell GetEmptyCellAbove(Cell cell)
        {
            if(cell != null && cell.BubbleObj == null && cell.Neighbours[1].GetComponent<Cell>().BubbleObj != null)
                return cell;
            return GetEmptyCellAbove(cell.Neighbours[1].GetComponent<Cell>());
        }

        private void Execute(GameObject target) => _child.gameObject.GetComponent<Move>().Execute(target);

        private Vector2 GetDirection(float angle) => new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)).normalized;
    }
}