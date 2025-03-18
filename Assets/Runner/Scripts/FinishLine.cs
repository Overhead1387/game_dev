using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Collider))]
    public class GoalLine : SpawnableEntity
    {
        private const string PlayerTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                GameController.Instance.OnVictory();
            }
        }
    }
}