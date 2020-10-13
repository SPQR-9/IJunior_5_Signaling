using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private float _timeToDetection = 0f;
    private AudioSource _source = new AudioSource();
    private bool _isEnemyExit = false;
    private Coroutine _signalCoroutine;

    private void Awake()
    {
        _source = gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(_source.volume==0  && _isEnemyExit)
        {
            StopCoroutine(_signalCoroutine);
            //StopCoroutine(Signaling());
            //StopAllCoroutines();
            _source.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Fox>(out Fox fox))
        {
            StartCoroutine(DetectingAnObject(_timeToDetection, fox));
            _signalCoroutine = StartCoroutine(Signaling());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Fox>(out Fox fox))
        {
            _isEnemyExit = true;
        }
    }

    private IEnumerator DetectingAnObject(float value, Fox fox)
    {
        yield return new WaitForSeconds(value);
        fox.Detected();
    }

    private IEnumerator Signaling()
    {
        _source.Play();
        while (true)
        {
            for (float i = 0; i < 1; i+=0.01f)
            {
                _source.volume = i;
                yield return null;
            }
            for (float i = 1; i > 0; i-=0.01f)
            {
                _source.volume = i;
                yield return null;
            }
        }
    }
}
