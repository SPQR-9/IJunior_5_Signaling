using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fox : MonoBehaviour
{
    private Animator _animator = new Animator();
    private Rigidbody2D _rigidbody = new Rigidbody2D();
    private SpriteRenderer _renderer = new SpriteRenderer();
    private bool _isDetected = false;
    private bool _isItEscaping = false;
    [SerializeField] float _speed  = 0f;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _animator.SetBool("Idle", true);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_isDetected)
        {
            _rigidbody.velocity = transform.right * _speed;
        }
        else if (!_isItEscaping)
        {
            _animator.SetBool("Run", true);
            _isItEscaping = true;
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            _renderer.flipX = true;
            _rigidbody.velocity = transform.right * _speed * -2;
        }
    }

    public void Detected()
    {
        _isDetected = true;
        _animator.SetBool("Idle", false);
    }
}
