using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    [SerializeField] private float _timeToDetection = 0f;

    private AudioSource _source;
    private bool _isEnemyExit = false;
    private Coroutine _signalCoroutine;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(_source.volume==0  && _isEnemyExit)
        {
            StopCoroutine(_signalCoroutine);
            _source.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Fox>(out Fox fox))
        {
            StartCoroutine(DetectingAnObject(_timeToDetection, fox));
            _signalCoroutine = StartCoroutine(AlarmOperation());
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

    private IEnumerator AlarmOperation()
    {
        _source.Play();
        while (true)
        {
            for (float i = 0; i < 100; i++)
            {
                _source.volume = 0f + (1f / 100f * i);
                yield return null;
            }
            for (float i = 0; i < 100; i++)
            {
                _source.volume = 1f - (1f / 100f * i);
                yield return null;
            }
        }
    }
}
