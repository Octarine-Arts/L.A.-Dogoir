using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AnimatorMisc : MonoBehaviour
{
    public Animator childAnimator;

    private RPCAnimTrigger thisAnimator;

    private void Awake()
    {
        thisAnimator = GetComponent<RPCAnimTrigger>();
    }

    [YarnCommand("StartAnim")]
    public void StartAnim()
    {
        thisAnimator.Trigger("Start");
    }

    public void SetObjectInactive(string gameObject)
    {
        GameObject.Find(gameObject).gameObject.SetActive(false);
    }

    public void SetObjectActive(string gameObject)
    {
        GameObject.Find(gameObject).gameObject.SetActive(true);
    }

    public void SetBoolFalse(string boolName)
    {
        childAnimator.SetBool(boolName, false);
    }

    public void SetBoolTrue(string boolName)
    {
        childAnimator.SetBool(boolName, true);
    }
}
