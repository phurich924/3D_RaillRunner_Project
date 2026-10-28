using UnityEngine;

public class ScriptCameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float followSmooth = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // คำนวณตำแหน่งที่ต้องการให้กล้องอยู่เทียบกับตำแหน่งปัจจุบันของ Target
        Vector3 desiredPosition = target.position + offset;

        // ขยับกล้องตามแบบนุ่มนวล
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmooth * Time.deltaTime);

        // ให้กล้องมองไปที่ลำตัวของ Target เสมอ
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}