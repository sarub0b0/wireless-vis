using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_target = null;

    [SerializeField]
    private float m_speed = 0.0f;

    public  Transform Target
    {
        get { return m_target; }
    }

    private Transform m_cameraTransform = null;
    private Transform m_pivot = null;

    [ContextMenu("ApplyTarget")]
    private void ApplyForceTarget()
    {
        if (Target == null)
        {
            return;
        }

        transform.position = Target.position;

        SetCameraTransform();
        if (m_cameraTransform == null)
        {
            return;
        }

        m_cameraTransform.transform.LookAt(Target);
    }

    private void SetCameraTransform()
    {
        Camera camera = GetComponentInChildren<Camera>();
        Debug.AssertFormat(camera != null, "カメラが無ぇよ!");
        if (camera == null)
        {
            return;
        }

        m_cameraTransform = camera.transform;
        m_pivot = m_cameraTransform.parent;
    }
    private void Awake()
    {
        Camera camera = GetComponentInChildren<Camera>();
        Debug.AssertFormat(camera != null, "カメラが無ぇよ!");
        if (camera == null)
        {
            return;
        }

        m_cameraTransform = camera.transform;
        m_pivot = m_cameraTransform.parent;
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    [SerializeField]
    private float m_waitRange = 0.0f;

    private void UpdateCamera()
    {
        if (Target == null)
        {
            return;
        }

        Vector3 toTargetVec = Target.position - transform.position;
        float sqrLength = toTargetVec.sqrMagnitude;

        // 設定した範囲内なら更新しない。
        // magnitudeはルート計算が重いので、二乗された値を利用しよう。
        if (sqrLength <= m_waitRange * m_waitRange)
        {
            return;
        }

        // ターゲットの位置から指定範囲内ギリギリの位置を目指すようにするよ。
        Vector3 targetPos = Target.position - toTargetVec.normalized * m_waitRange;

        float deltaSpeed = m_speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, deltaSpeed);
    }

    public void Turn(float i_angle)
    {
        transform.rotation *= Quaternion.AngleAxis(i_angle, Vector3.up);
    }
}
