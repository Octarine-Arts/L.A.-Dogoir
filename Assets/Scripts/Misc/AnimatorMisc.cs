using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMisc : MonoBehaviour
{
    public Animator _animator;

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
        _animator.SetBool(boolName, false);
    }

    public void SetBoolTrue(string boolName)
    {
        _animator.SetBool(boolName, true);
    }
}
