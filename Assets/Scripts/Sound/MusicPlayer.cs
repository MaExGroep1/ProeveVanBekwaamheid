using System;
using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private SoundService soundService;
    private void Start()
    {
        soundService.PlaySound();
    }
}
