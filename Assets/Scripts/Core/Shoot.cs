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
                Execute(hit.collider.gameObject);
            }
        }

        private void Execute(GameObject target)
        {
            GetComponent<Move>().Execute(target);
        }
    }
}