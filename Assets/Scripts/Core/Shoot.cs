using System.Collections.Generic;
using UnityEngine;

namespace BubblePops.Core
{
    public class Shoot : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private float angle = 0f;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private ContactFilter2D filter;

        /// <summary>
        /// 
        /// </summary>
        private Transform _child = null;

        private Queue<Transform> _reflectQueue = new Queue<Transform>();

        private void Update()
        {
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
            //var hit = Physics2D.Raycast(transform.position, Vector2.up);
            Debug.DrawRay(transform.position, GetDirection(angle) * 1000, Color.magenta);

            if(masterCell != null)
            {
                Debug.DrawRay(masterCell.transform.position, GetDirection(angle > 0 ? angle - 90 : angle + 90) * 1000, Color.green);
            }
        }

        public Cell masterCell = null;
        public Cell reflectCell = null;

        [NaughtyAttributes.Button]
        public void SetTarget()
        {
            var result = new List<RaycastHit2D>();
            var count = Physics2D.Raycast(_child.position, GetDirection(angle), filter, result);

            reflectCell = null;

            if(count > 0)
            {
                var collider = result[result.Count - 1].collider;
                masterCell = collider.gameObject.GetComponent<Cell>();
                if(_reflectQueue.Count == 0 && masterCell.Neighbours[1].GetComponent<Cell>().BubbleObj != null)
                {
                    Execute(masterCell.gameObject);
                }
                else
                {
                    _reflectQueue.Enqueue(masterCell.transform);

                    var reflectResult = new List<RaycastHit2D>();
                    var reflectCount = Physics2D.Raycast(
                        masterCell.transform.position, GetDirection(angle > 0 ? angle - 90 : angle + 90),  filter, reflectResult
                        );

                    if(reflectCount > 0)
                    {
                        reflectCell = reflectResult[reflectResult.Count - 1].collider.gameObject.GetComponent<Cell>();
                        _reflectQueue.Enqueue(reflectCell.transform);

                        if(reflectCell.Neighbours[1].GetComponent<Cell>().BubbleObj == null)
                        {
                            //disable line renderer
                            _reflectQueue.Clear();
                        }
                        else
                        {
                            _child.GetComponent<Move>().Execute(_reflectQueue);
                        }
                    }
                }
            }
        }

        public void SetChild(Transform child) => _child = child;

        private Cell GetEmptyCellAbove(Cell cell)
        {
            if(cell != null && cell.BubbleObj == null && cell.Neighbours[1].GetComponent<Cell>().BubbleObj != null)
                return cell;
            return GetEmptyCellAbove(cell.Neighbours[1].GetComponent<Cell>());
        }

        private void Execute(GameObject target)
        {
            _child.gameObject.GetComponent<Move>().Execute(target);
        }

        private Vector2 GetDirection(float angle) => new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)).normalized;
    }
}