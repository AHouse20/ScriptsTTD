using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private Material storedMaterial;
    private bool storedIsActive;

    public void Show(Material material)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = material;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }

    public void ShowTemp(Material material)
    {
        if(storedMaterial != material) storedMaterial = meshRenderer.material;
        if(storedIsActive != meshRenderer.enabled) storedIsActive = meshRenderer.enabled;
        Show(material);
    }

    public void HideTemp()
    {
        meshRenderer.enabled = storedIsActive;
        meshRenderer.material = storedMaterial;
    }
}
