using UnityEngine;

public class LineController : MonoBehaviour
{
    [Header("Line Settings")]
    public float lineLength = 5f;
    public LayerMask interactionLayer;
    public float startOffset = 0.5f; // —мещение от центра родител€

    private LineRenderer lineRenderer;
    private Vector2 lineDirection;
    private Vector2 lineStartPoint;
    private Vector2 lineEndPoint;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineDirection = transform.right;
        UpdateLine();
    }

    void Update()
    {
        UpdateLine();
        CheckForIntersections();
    }

    void UpdateLine()
    {
        // Ќачальна€ точка с учетом смещени€ от центра родител€
        Vector2 parentPosition = transform.parent != null ?
            (Vector2)transform.parent.position : (Vector2)transform.position;

        lineStartPoint = parentPosition + lineDirection * startOffset;
        lineEndPoint = lineStartPoint + lineDirection * lineLength;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, lineStartPoint);
        lineRenderer.SetPosition(1, lineEndPoint);
    }

    void CheckForIntersections()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            lineStartPoint,
            lineDirection,
            lineLength,
            interactionLayer
        );

        if (hit.collider != null)
        {
            LineSplitter splitter = hit.collider.GetComponent<LineSplitter>();
            if (splitter != null && !splitter.hasSplit)
            {
                splitter.SplitLine(hit.point, lineDirection, lineLength);
            }
        }
    }

    void OnDrawGizmos()
    {
        Vector2 parentPos = transform.parent != null ?
            (Vector2)transform.parent.position : (Vector2)transform.position;

        Vector2 start = parentPos + (Vector2)transform.right * startOffset;
        Vector2 end = start + (Vector2)transform.right * lineLength;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(start, 0.1f);
    }
}