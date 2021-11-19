using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private RawImage _hurtEffect;
    [SerializeField] private Text _text;
    [SerializeField] private Player _player;

    [SerializeField] private float _hurtEffectFadeOutTime = 0.5f;
    private float _hurtEffectRemainingTime = 0;
    private Color _hurtEffectColor;

    void Start()
    {
        GameController.Instance.GameUI = this; //-- Make sure our Singleton class can reference this instance of the GameUI-class

        Vector2 screenSize = new Vector2(Screen.width, Screen.height);  //-- Get the screen dimensions
        _hurtEffect.transform.localScale = screenSize; //-- Set the scale of the Hurt Effect to cover the entire screen;

        _hurtEffectColor = _hurtEffect.color; //-- Save the color set in the Inspector
        var hurtColor = _hurtEffectColor;     //-- Create a new copy of this color
        hurtColor.a = 0f;                     //-- Set the colors Alpha Channel to 0%
        _hurtEffect.color = hurtColor;        //-- Apply the new color to the screen overlay.
    }
    void Update()
    {
        FadeOutHurtEffect();
    }

    public void UpdateText() //-- Call this method from anywhere to refresh the displayed text
    {
        _text.text = $"Current Health: {_player.HealthPercentage}%";
    }

    private void FadeOutHurtEffect()
    {
        if (_hurtEffectRemainingTime <= 0) return;

        _hurtEffectRemainingTime -= 1 * Time.deltaTime;

        var alpha = Mathf.Max(0 ,(_hurtEffectRemainingTime / _hurtEffectFadeOutTime)); //-- Divide the remaining time by total time to get a number where 0 is no time left and 1 is the maximum remaining time. Using Mathf.Min to make sure value never goes beyond 0.
        var hurtColor = _hurtEffectColor;   //-- Create a copy of the cached color.
        hurtColor.a = alpha;                //-- Change the alpha value of the copied color to the calculated alpha value
        _hurtEffect.color = hurtColor;      //-- Apply the new color to the overlay
    }

    public void ToggleHurtEffect()
    {
        _hurtEffectRemainingTime = _hurtEffectFadeOutTime;  //-- Resets the fadeout time when this Method is called
    }
}
