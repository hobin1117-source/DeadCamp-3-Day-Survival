using System.Threading;
using UnityEngine;

public class ZombieDeathBehaviour : StateMachineBehaviour
{
    //애니메이션 상태를 벗어날 때 호출이 됨(즉 Death 이후의 애니메이션은 없으니 Death애니메이션 이후 호출이 됨)
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Monster monster = animator.GetComponent<Monster>();

        if (monster != null)
        {
            monster.DestroySelf();
        }
    }
}
