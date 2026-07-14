using TMPro;
using UnityEngine;

public class GridDebug : MonoBehaviour
{
    private GridObject gridObject;
    [SerializeField] private TMP_Text debugText;
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void Update()
    {
        debugText.text = gridObject.ToString();
    }
}
