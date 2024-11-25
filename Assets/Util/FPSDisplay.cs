using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This class is meant to show the real fps of the game.
/// Just put this class over a GameObject and it will function
/// </summary>

public class FPSDisplay : MonoBehaviour
{
    [Header("text details")]
    [SerializeField] private int _fontSize = 24;
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField] private Rect _textRect = new Rect(20, 20, 200, 20);


    private float _deltaTime = 0.0f;
    private string _text;
    private GUIStyle _style = new GUIStyle();

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        _text = string.Format("({0:0.} fps)", fps);
    }

    void OnGUI()
    {
        _style.fontSize = _fontSize;
        _style.normal.textColor = _textColor;

        GUI.Label(_textRect, _text, _style);
    }
}