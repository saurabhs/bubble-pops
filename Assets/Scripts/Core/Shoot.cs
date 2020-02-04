using UnityEngine;

namespace BubblePops.Core
{
    [RequireComponent(typeof(Move))]
    public class Shoot : MonoBehaviour
    {
        [NaughtyAttributes.Button]
        public void SetTarget()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.up);
            if(hit.collider != null)
            {
                Execute(GetEmptyCellAbove(hit.collider.gameObject.GetComponent<Cell>()).gameObject);
            }
        }

        private Cell GetEmptyCellAbove(Cell cell)
        {
            if(cell != null && cell.bubble == null && cell.Neighbours[1].GetComponent<Cell>().bubble != null)
                return cell;
            return GetEmptyCellAbove(cell.Neighbours[1].GetComponent<Cell>());
        }

        private void Execute(GameObject target)
        {
            GetComponent<Move>().Execute(target);
        }
    }
}