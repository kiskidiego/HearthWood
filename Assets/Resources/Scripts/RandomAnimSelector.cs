using UnityEngine;

public class RandomAnimSelector : StateMachineBehaviour
{
    [SerializeField] int _numberOfAnims = 1;
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        int randomAnim = Random.Range(0, _numberOfAnims);
        Debug.Log("Selected Random Anim: " + randomAnim);
        animator.SetInteger("RandomAnim", randomAnim);
    }
}
