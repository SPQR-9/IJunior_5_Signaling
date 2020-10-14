using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Fox : MonoBehaviour
{
    [SerializeField] private float _speed = 0f;
    [SerializeField] private float _responseTime = 0f;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private bool _isDetected = false;
    private bool _isItEscaping = false;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _animator.SetBool("Idle", true);
    }

    private void FixedUpdate()
    {
        if (!_isDetected)
        {
            _rigidbody.velocity = transform.right * _speed;
        }
        else if(_isItEscaping)
        {
            _rigidbody.velocity = transform.right * _speed * -2;
        }
    }

    public void Detected()
    {
        _isDetected = true;
        _animator.SetBool("Idle", false);
        StartCoroutine(RespondToDanger());
    }

    public IEnumerator RespondToDanger()
    {
        yield return new WaitForSeconds(_responseTime);
        _animator.SetBool("Run", true);
        _renderer.flipX = true;
        _isItEscaping = true;
    }
}
