using UnityEngine;

public class SoldierAnimator : MonoBehaviour
{
    private Animator animator;
    private SoldierMove movement;

    public void Initialise(SoldierMove soldierMove) {
        animator = GetComponent<Animator>();
        movement = soldierMove;
        movement.OnMovementStarted += StartRun;
        movement.OnMovementStopped += StopRun;
    }

    private void OnDisable() {
        movement.OnMovementStarted -= StartRun;
        movement.OnMovementStopped -= StopRun;
    }

    private void StartRun() {
        // Stop idle animation and play running
        if (animator != null) {
            animator.SetBool("Run 0", true);
            animator.SetBool("Stop 0", false);
        }
    }

    private void StopRun() {
        // Stop running animation and play idle
        if (animator != null) {
            animator.SetBool("Run 0", false);
            animator.SetBool("Stop 0", true);
        }
    }
}
