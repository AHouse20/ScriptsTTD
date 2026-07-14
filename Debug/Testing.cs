using AStar;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] GridPosition startGridPosition;
    [SerializeField] GridPosition endGridPosition;

    [Button]
    private void GetPath()
    {
        Pathfinding.Instance.FindPath(startGridPosition, endGridPosition);
    }
}
