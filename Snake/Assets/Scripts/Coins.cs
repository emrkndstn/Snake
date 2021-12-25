using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines.Primitives;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public bool canCollect = false;
    public bool collected = false;
    
    private void Start()
    {
        StartCoroutine(CollectDelay());
    }

    private void Update()
    {
        if (collected)
        {
            transform.DOScale(.01f, .5f).OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain")) // Coinler yere düşünce bulunduğu yere sabitleme
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = 0;
        }
    }
    
    IEnumerator CollectDelay()
    {
        yield return new WaitForSecondsRealtime(.5f);
        canCollect = true;
    }

    public void SnakeHit()
    {
        GetComponent<Collider>().enabled = false;
        canCollect = false;
        collected = true;
    }
    
}
