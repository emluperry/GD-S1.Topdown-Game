using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private Vector2 m_InputDirection;

    private Vector2 m_WeaponInputDirection;
    private Quaternion m_AttackArmIdleRotation;

    private SpriteRenderer m_ChainSpriteRenderer;
    private BoxCollider2D m_ChainCollider;

    [Header("Player Animation")]
    [SerializeField] private Animator m_PlayerAnimator;

    [Header("Weapon Animation")]
    [SerializeField] private Transform m_AttackArmJoint;
    [SerializeField] private Transform m_WeaponHead;
    [SerializeField] private Transform m_WeaponChain;

    private void Start()
    {
        m_AttackArmIdleRotation = m_AttackArmJoint.rotation;

        m_ChainSpriteRenderer = m_WeaponChain.GetComponent<SpriteRenderer>();
        m_ChainCollider = m_WeaponChain.GetComponent<BoxCollider2D>();
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
        AdjustPlayerSprite();

        AdjustWeaponSprites();
    }

    private void AdjustPlayerSprite()
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
    }

    private void AdjustWeaponSprites()
    {
        Vector2 size;
        if (m_WeaponInputDirection.magnitude > 0)
        {
            Vector3 WeaponVector = -m_AttackArmJoint.position + m_WeaponHead.position;
            Quaternion ArmRotation = Quaternion.identity;
            ArmRotation.SetFromToRotation((m_AttackArmJoint.rotation * Vector2.right).normalized, WeaponVector.normalized);
            m_AttackArmJoint.rotation *= ArmRotation;
            m_WeaponHead.rotation = m_AttackArmJoint.rotation;

            size = new Vector2(WeaponVector.magnitude, m_ChainSpriteRenderer.size.y);
            m_ChainSpriteRenderer.size = size;
            m_ChainCollider.size = size;
            m_WeaponChain.transform.localPosition = new Vector3(0.5f * WeaponVector.magnitude, 0, 0);
        }
        else
        {
            m_AttackArmJoint.rotation = m_AttackArmIdleRotation;

            size = new Vector2(0, m_ChainSpriteRenderer.size.y);
            m_ChainSpriteRenderer.size = size;
            m_ChainCollider.size = size;
            m_WeaponChain.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    private void KillPlayer()
    {
        //m_PlayerAnimator.SetTrigger("Die");
    }

}
