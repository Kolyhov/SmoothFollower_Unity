using UnityEngine;

public enum UpdateMethod { Update, FixedUpdate }
public enum TargetType { Transform, Cursor }

public class SmoothFollower : MonoBehaviour
{
    [Header("Dynamics settings")]
    public float F = 1f; 
    public float Z = 0.5f; 
    public float R = 2f; 

    [Header("Target settings")]
    public TargetType targetType = TargetType.Transform;
    public Transform targetTransform;
    public Vector3 Offset;

    [Header("Additionally")]
    public UpdateMethod updateMethod = UpdateMethod.Update;

    [Header("Axis Motion Multipliers")]
    [Range(0f, 2f)] public float multiplierX = 1f;
    [Range(0f, 2f)] public float multiplierY = 1f;
    [Range(0f, 2f)] public float multiplierZ = 1f;

    private Vector3 targetPosition;
    private SecondOrderDynamics dynamics;
    private RectTransform rectTransform;
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (targetType == TargetType.Cursor)
            Cursor.visible = false;

        targetPosition = GetCurrentTargetPosition();
        dynamics = new SecondOrderDynamics(F, Z, R, targetPosition);
    }

    void OnValidate()
    {
        if (Application.isPlaying && dynamics != null)
            dynamics.UpdateCoefficients(F, Z, R);
    }

    void Update()
    {
        if (updateMethod == UpdateMethod.Update)
            Move(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (updateMethod == UpdateMethod.FixedUpdate)
            Move(Time.fixedDeltaTime);
    }

    private Vector3 GetCurrentTargetPosition()
    {
        if (targetType == TargetType.Cursor)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out localPoint);
            return new Vector3(localPoint.x, localPoint.y, 0);
        }
        else if (targetTransform != null)
        {
            if (rectTransform != null)
                return rectTransform.parent.InverseTransformPoint(targetTransform.position);
            else
                return targetTransform.position;
        }
        return transform.position;
    }

    private void Move(float deltaTime)
    {
        targetPosition = GetCurrentTargetPosition() + Offset;
        Vector3 smoothPosition = dynamics.Update(deltaTime, targetPosition);

        Vector3 currentPosition = transform.localPosition;

        smoothPosition = new Vector3(
            Mathf.Lerp(currentPosition.x, smoothPosition.x, multiplierX),
            Mathf.Lerp(currentPosition.y, smoothPosition.y, multiplierY),
            Mathf.Lerp(currentPosition.z, smoothPosition.z, multiplierZ)
        );

        transform.localPosition = smoothPosition;
    }

    // Created by Oleksii Koliukhov

    private class SecondOrderDynamics
    {
        private Vector3 xp;
        private Vector3 y;
        private Vector3 yd;
        private float k1, k2, k3;

        public SecondOrderDynamics(float f, float z, float r, Vector3 x0)
        {
            y = xp = x0;
            yd = Vector3.zero;
            UpdateCoefficients(f, z, r);
        }

        public void UpdateCoefficients(float f, float z, float r)
        {
            k1 = z / (Mathf.PI * f);
            k2 = 1f / ((2f * Mathf.PI * f) * (2f * Mathf.PI * f));
            k3 = r * z / (2f * Mathf.PI * f);
        }

        public Vector3 Update(float T, Vector3 x, Vector3? xd = null)
        {
            if (xd == null)
                xd = (x - xp) / T;

            xp = x;

            float k2_stable = Mathf.Max(k2, T * T / 2f + T * k1 / 2f, T * k1);

            y += T * yd;
            yd += T * (x + k3 * xd.Value - y - k1 * yd) / k2_stable;

            return y;
        }
    }
}