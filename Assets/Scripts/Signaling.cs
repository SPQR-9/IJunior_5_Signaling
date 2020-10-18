using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        if(_source.volume<=0.01  && _isEnemyExit)
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
        bool isVolumeUp=true;
        while (true)
        {
            if (_source.volume >= 0.99f)
                isVolumeUp = false;
            else if (_source.volume <= 0.01f)
                isVolumeUp = true;
            if (isVolumeUp)
                _source.volume = Mathf.Lerp(_source.volume, 1f, 0.05f);
            else if (!isVolumeUp)
                _source.volume = Mathf.Lerp(_source.volume, 0f, 0.05f);
            yield return null;
        }
    }
}
