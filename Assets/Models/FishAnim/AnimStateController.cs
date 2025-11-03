using UnityEngine;

public class AnimStateController : MonoBehaviour
{
    [SerializeField] private Animator animationController;

    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private float movementThreshold = 1f; // ����� �������� ��� ��������
    public float movementMagnitude;
    public bool ismoving;

    // Update is called once per frame
    void Update()
    { 
        movementMagnitude = playerRB.linearVelocity.magnitude;
        // ��������� �������� ������
        if (playerRB.linearVelocity.magnitude > movementThreshold)
        {
            //animationController.SetTrigger("StartMoving");
            animationController.SetBool("IsMoving", true);
            ismoving = true;
        }
        else
        {
            //animationController.SetTrigger("StopMoving");
            animationController.SetBool("IsMoving", false);
            ismoving = false;
        }
    }

   
}
