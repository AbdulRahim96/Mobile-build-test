using UnityEngine;
using UnityEngine.EventSystems;

public class DragInput : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Vector2 input; // Output input vector [-1, 1]
    public float maxDragDistance = 100f; // Max distance before input is fully 1 or -1

    private Vector2 startDragPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPosition = eventData.position;
        Vector2 delta = currentPosition - startDragPosition;

        // Normalize input relative to max drag distance
        float x = Mathf.Clamp(delta.x / maxDragDistance, -1f, 1f);
        float y = Mathf.Clamp(delta.y / maxDragDistance, -1f, 1f);

        input = new Vector2(x, y);
        PaperPlaneController.inputCallback?.Invoke(input); // Call the input callback with the new input value
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        input = Vector2.zero;
        PaperPlaneController.inputCallback?.Invoke(input);
    }
}
