using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform target;
    [SerializeField] private GameObject hand;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject projectilePrefab;
    private InputReader playerInput;

    [SerializeField] private PlayerAnimator anim;
    [SerializeField] private AnimatorEvents animEventController;

    private void Awake()
    {
        playerInput = GetComponent<InputReader>();
    }

    private void Start()
    {
        playerInput.OnAttack += SimpleAttack;
    }

    private void SimpleAttack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        SimpleProjectile projectile = projectileObject.GetComponent<SimpleProjectile>();

        if (projectile != null)
        {
            projectile.SetTarget(target.position);
            projectile.handPoistion = hand;
            animEventController.projectile = projectile;
        }
        else
        {
            Debug.LogError("Projectile component not found on the instantiated object!");
            Destroy(projectileObject);
        }
        anim.SimpleAttack();
    }
}
