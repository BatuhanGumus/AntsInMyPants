using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnimationElement : MonoBehaviour
{
    private const float AnimationStep = 0.1f;
    
    [SerializeField] private Image loadingImage;
    

    private bool _run = false;
    
    private void OnEnable()
    {
        _run = true;
        Invoke("Animation", AnimationStep);
    }

    private void Animation()
    {
        if(!_run) return;
        
        var newFill = loadingImage.fillOrigin + 1;
        if (newFill >= 4) newFill = 0;
        loadingImage.fillOrigin = newFill;
        Invoke("Animation", AnimationStep);
    }

    private void OnDisable()
    {
        _run = false;
    }
}
