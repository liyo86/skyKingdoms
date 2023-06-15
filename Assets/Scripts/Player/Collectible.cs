﻿using Managers;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class Collectible : MonoBehaviour
{
    public enum CollectibleTypes { NoType, GemBlue, GemPurple, GemRed, GemGreen, Type5 };

    public CollectibleTypes CollectibleType; // this gameObject's type
    public bool rotate; // do you want it to rotate?
    public float rotationSpeed;
    public AudioClip collectSound;
    public GameObject collectEffect;
  

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (rotate)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Dragon"))
        {
            if (other.CompareTag("Player"))
            {
                Vector3 dragonPosition = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
                if(!MyGameManager.Instance.AIDemoControl)
                    BoyController.Instance.ShowDragon(dragonPosition);
                else
                    SceneManager.LoadScene("Menu_game");
            }
            
            Collect();
        }
    }

    private void Collect()
    {
        if (collectSound)
            MyAudioManager.Instance.PlaySfx("levelCompleteSFX");
        if (collectEffect)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        MyGameManager.Instance.CollectGem(CollectibleType);

        Destroy(gameObject);
    }
}