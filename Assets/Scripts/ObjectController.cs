//------------------------------------------------------------------------
//
// (C) Copyright 2017 Urahimono Project Inc.
//
//------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ObjectController : MonoBehaviour
{
    [Header("Control")]
    [SerializeField]
    private bool m_controlled = true;
    [SerializeField]
    private float m_moveSpeed = 0.0f;
    [SerializeField]
    private float m_turnSpeed = 0.0f;
    [SerializeField]
    private float m_jumpForce = 0.0f;

    [Header("Layer")]
    [SerializeField, LayerTypeField]
    private int m_copiedLayer = 0;
    [SerializeField, LayerTypeField]
    private int m_defaultLayer = 0;
    [SerializeField, LayerTypeField]
    private int m_exitLayer = 0;

    [Header("Camera")]
    [SerializeField]
    private CameraController m_camera = null;
    [SerializeField]
    private float m_camraTurnSpeed = 0.0f;

    private ExitBoxTrigger m_exitTrigger = null;
    private Rigidbody m_rigidbody = null;
    private Renderer m_renderer = null;


    private Color m_defaultColor = Color.white;

    private Color ObjectColor
    {
        get { return m_renderer.material.color; }
        set { m_renderer.material.color = value; }
    }

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_renderer = GetComponentInChildren<Renderer>();

        // m_defaultColor = ObjectColor;
    }

    private void Update()
    {
        if (m_controlled)
        {
            ControlObject();
        }

        ControlCamera();
    }

    private void ApplyDefaultObject()
    {
        gameObject.layer = m_defaultLayer;
        // ObjectColor = m_defaultColor;
    }

    private void ControlObject()
    {
        Vector3 moveDir = Vector3.zero;

        Vector3 forwardDir = m_camera.transform.forward;
        Vector3 rightDir = m_camera.transform.right;

        if (Input.GetKey(KeyCode.K))
        {
            moveDir += forwardDir;
        }
        if (Input.GetKey(KeyCode.J))
        {
            moveDir -= forwardDir;
        }
        if (Input.GetKey(KeyCode.L))
        {
            moveDir += rightDir;
        }
        if (Input.GetKey(KeyCode.H))
        {
            moveDir -= rightDir;
        }

        if (moveDir.sqrMagnitude > Mathf.Epsilon)
        {
            moveDir = moveDir.normalized;
            Turn(moveDir);
            Move(moveDir);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Duplication();
        }
    }

    private void Move(Vector3 i_forward)
    {
        Vector3 delta = i_forward * m_moveSpeed * Time.deltaTime;
        Vector3 targetPos = transform.position + delta;
        m_rigidbody.MovePosition(targetPos);
    }

    private void Turn(Vector3 i_forward)
    {
        Quaternion toRot = Quaternion.LookRotation(i_forward);
        Quaternion fromRot = transform.rotation;

        float delta = m_turnSpeed * Time.deltaTime;
        Quaternion targetRot = Quaternion.RotateTowards(fromRot, toRot, delta);

        m_rigidbody.MoveRotation(targetRot);
    }

    private void Jump()
    {
        // えっ、このままじゃ空中でもジャンプできちゃうって！？
        // 仕様だよ！

        m_rigidbody.velocity = Vector3.zero;

        Vector3 jumpVec = Vector3.up * m_jumpForce;
        m_rigidbody.AddForce(jumpVec, ForceMode.Impulse);
    }

    private void Duplication()
    {
        var copiedObj = Instantiate(this, transform.position, transform.rotation);
        copiedObj.SetDuplicationParameter();
    }

    private void SetDuplicationParameter()
    {
        m_controlled = false;
        gameObject.layer = m_copiedLayer;

        // ObjectColor = new Color(0.0f, 0.0f, 0.0f, 0.3f);

        var trigger = new GameObject("Trigger").AddComponent<ExitBoxTrigger>();
        trigger.Initialize(transform, GetComponent<BoxCollider>(), m_exitLayer);
        trigger.onExit += OnExitObject;

        m_exitTrigger = trigger;

    }

    private void OnExitObject()
    {
        ApplyDefaultObject();

        if (m_exitTrigger != null)
        {
            Destroy(m_exitTrigger.gameObject);
        }
    }

    private void ControlCamera()
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_camera.Turn(m_camraTurnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_camera.Turn(-m_camraTurnSpeed * Time.deltaTime);
        }
    }

}
// class ObjectController
