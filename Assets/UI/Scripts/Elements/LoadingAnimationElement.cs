using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnimationElement : MonoBehaviour
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private float animationStep = 0.2f;

    private bool _run = false;
    
    private void OnEnable()
    {
        _run = true;
        Invoke("Animation", animationStep);
    }

    private void Animation()
    {
        if(!_run) return;
        
        var newFill = loadingImage.fillOrigin + 1;
        if (newFill >= 4) newFill = 0;
        loadingImage.fillOrigin = newFill;
        Invoke("Animation", animationStep);
    }

    private void OnDisable()
    {
        _run = false;
    }
}
