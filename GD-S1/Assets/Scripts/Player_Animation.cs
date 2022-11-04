using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private Vector2 m_InputDirection;

    private Vector2 m_WeaponInputDirection;
    private Quaternion m_AttackArmIdleRotation;

    [Header("Player Animation")]
    [SerializeField] private Animator m_PlayerAnimator;

    [Header("Weapon Animation")]
    [SerializeField] private Transform m_AttackArmJoint;
    [SerializeField] private Transform m_WeaponHead;
    [SerializeField] private SpriteRenderer m_WeaponChain;

    private void Start()
    {
        m_AttackArmIdleRotation = m_AttackArmJoint.rotation;
    }

    private void Update()
    {
        m_InputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_WeaponInputDirection = new Vector2(Input.GetAxis("W-Horizontal"), Input.GetAxis("W-Vertical"));
    }

    private void FixedUpdate()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (m_InputDirection.x > 0)
        {
            m_PlayerAnimator.transform.localScale = new Vector2(-1, 1);
            m_AttackArmJoint.localScale = new Vector2(-1, 1);
            m_PlayerAnimator.SetBool("isWalking", true);
        }
        else if (m_InputDirection.x < 0)
        {
            m_PlayerAnimator.transform.localScale = new Vector2(1, 1);
            m_AttackArmJoint.localScale = new Vector2(1, 1);
            m_PlayerAnimator.SetBool("isWalking", true);
        }
        else
        {
            m_PlayerAnimator.SetBool("isWalking", false);
        }

        if (m_WeaponInputDirection.magnitude > 0)
        {
            Vector3 WeaponVector = -m_AttackArmJoint.position + m_WeaponHead.position;
            Quaternion ArmRotation = Quaternion.identity;
            ArmRotation.SetFromToRotation((m_AttackArmJoint.rotation * Vector2.right).normalized, WeaponVector.normalized);
            m_AttackArmJoint.rotation *= ArmRotation;
            m_WeaponHead.rotation = m_AttackArmJoint.rotation;

            m_WeaponChain.size = new Vector2(WeaponVector.magnitude, m_WeaponChain.size.y);
            m_WeaponChain.transform.localPosition = new Vector3(0.5f * WeaponVector.magnitude, 0, 0);
        }
        else
        {
            m_AttackArmJoint.rotation = m_AttackArmIdleRotation;
            m_WeaponChain.size = new Vector2(0, m_WeaponChain.size.y);
        }
    }
}
