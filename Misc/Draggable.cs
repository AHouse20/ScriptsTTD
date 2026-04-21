using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    private Canvas canvas;
    private float speed = 4;
    [HideInInspector] public bool isDragging = false;
    [SerializeField] protected Transform movingPiece;
    protected Vector3 targetPosition;
    private CursorManager cursor;
    [SerializeField] private Vector3 scalingVector = Vector3.one;
    [SerializeField] private EventReference pointerEnterSound;
    private Animator animator;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        cursor = GameManager.Instance.cursor;
        targetPosition = Vector3.zero;
        animator = GetComponent<Animator>();
        //ratio = new Vector2(canvas.scaleFactor, canvas.scaleFactor);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        movingPiece.transform.localScale = scalingVector;
        movingPiece.transform.SetParent(cursor.transform, true);
        movingPiece.transform.SetAsFirstSibling();
        Debug.Log("Click!");
    }

    /*
    public void OnPointerMove(PointerEventData eventData)
    {

    }
    */
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        ReturnObject();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(animator != null)
        {
            animator.SetTrigger("PointerEnter");
            AudioManager.Instance.PlayOneShot(pointerEnterSound, Vector3.zero);
        }
    }
    // - offset
    private void Update()
    {
        //if (isDragging) targetPosition = cursor.transform.position - movingPiece.transform.position;
        movingPiece.transform.localPosition = Vector3.Lerp(movingPiece.transform.localPosition, targetPosition, Time.deltaTime * speed);
    }

    public void ReturnObject()
    {
        movingPiece.transform.localScale = Vector3.one;
        movingPiece.transform.SetParent(transform, true);
        targetPosition = Vector3.zero;
    }


}
