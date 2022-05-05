using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockParts : MonoBehaviour
{
    public Vault vault;
    public LayerMask layers;
    public AudioClip clickAudio;

    private bool _hasClicked;

    // Update is called once per frame
    void Update()
    {
        Ray();
    }

    private void Ray()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.up, out hitInfo, 5f, layers))
        {
            PlayAudio();
            vault.SetCurrentLetter(name);
        }
        else
        {
            _hasClicked = false;
        }
    }

    private void PlayAudio()
    {
        if (!_hasClicked) return;
        Debug.Log("Click");
        AudioManager.current.PlaySFX(clickAudio);
        _hasClicked = true;
    }
}
