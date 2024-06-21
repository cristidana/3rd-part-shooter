using System.Collections;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class ShooterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private Image aimCross;
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Transform aimTargetPosition;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Settings")]
    [SerializeField] private float normalSensitivity = 2.0f;
    [SerializeField] private float aimSensitivity = 1.0f;
    [SerializeField] private float aimTransitionSpeed = 10.0f;

    private StarterAssetsInputs startAssetsInput;
    private ThirdPersonController thirdPersonController;
    private Animator animator;
    private RaycastHit rayHit;
    private Vector3 currentAimTargetPosition;

    private float currentHealth = 100.0f;

    private void Awake()
    {
        startAssetsInput = GetComponent<StarterAssetsInputs>();
        startAssetsInput.SetFocus(true);
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        currentAimTargetPosition = aimTargetPosition.position;
        gameOverPanel.SetActive(false);


        
    }

    private void Update()
    {
        if (startAssetsInput.aim)
        {
            EnterAimMode();
        }
        else
        {
            ExitAimMode();
        }
    }

    private void EnterAimMode()
    {
        ActivateAimMode(true);
        AdjustAnimatorLayerWeight(1, 1.0f, 15.0f);
        AdjustAimRigWeight(1.0f, 20.0f);

        Aim();

        if (startAssetsInput.shoot || Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
            startAssetsInput.shoot = false;
        }
    }

    private void ExitAimMode()
    {
        ActivateAimMode(false);
        AdjustAnimatorLayerWeight(1, 0.0f, 15.0f);
        AdjustAimRigWeight(0.0f, 20.0f);
    }

    private void ActivateAimMode(bool activate)
    {
        aimCamera.gameObject.SetActive(activate);
        thirdPersonController.SetSensitivity(activate ? aimSensitivity : normalSensitivity);
        thirdPersonController.SetRotateOnMove(!activate);
        aimCross.gameObject.SetActive(activate);
    }

    private void AdjustAnimatorLayerWeight(int layerIndex, float targetWeight, float lerpSpeed)
    {
        float currentWeight = animator.GetLayerWeight(layerIndex);
        float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * lerpSpeed);
        animator.SetLayerWeight(layerIndex, newWeight);
    }

    private void AdjustAimRigWeight(float targetWeight, float lerpSpeed)
    {
        float currentWeight = aimRig.weight;
        float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * lerpSpeed);
        aimRig.weight = newWeight;
    }

    private void Aim()
    {
        Vector3 mousePosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));

        if (Physics.Raycast(ray, out rayHit, 999.0f, aimMask))
        {
            mousePosition = rayHit.point;
        }

        currentAimTargetPosition = Vector3.Lerp(currentAimTargetPosition, mousePosition, Time.deltaTime * aimTransitionSpeed);
        aimTargetPosition.position = currentAimTargetPosition;

        Vector3 aimTarget = currentAimTargetPosition;
        aimTarget.y = transform.position.y;
        Vector3 aimDirection = (aimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20.0f);
    }

    private void Shoot()
    {
        WeaponManager gun = GetComponentInChildren<WeaponManager>();
        if (gun != null)
        {
            gun.Shoot(rayHit);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        setPanelControl();
        gameOverPanel.SetActive(true);
        
    }

    public void setPanelControl()
    {
        startAssetsInput.SetFocus(false);
        gameObject.SetActive(false);
        aimCross.gameObject.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
