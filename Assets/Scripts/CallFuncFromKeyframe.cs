using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallFuncFromKeyframe : MonoBehaviour
{
    [SerializeField] EnemyFollowerAI ai;

    public void Call ()
    {
        ai.Attack();
    }
}
