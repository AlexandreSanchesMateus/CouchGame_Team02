using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool _enable;

    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float _frequency = 10.0f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = this.GetComponent<CharacterController>();
        _startPos = _camera.localPosition;
    }

    private void Update()
    {
        if (!enabled) 
            return;

        /*if (Input.GetKeyDown(UnityEngine.KeyCode.A))
        {
            _cameraHolder.DOLocalMoveX(15, 5);
        }*/

        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private void CheckMotion()
    {
        float speed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;
        // Debug.Log(_controller.velocity);
        /*Debug.Log(speed);
        Debug.Log(_controller.isGrounded);*/

        if (speed < _toggleSpeed)
            return;

        // Debug.Log("bite 1");

        if (!_controller.isGrounded)
            return;

        Debug.Log("bite 2");

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x += Mathf.Sin(Time.time * _frequency / 2) * _amplitude * 2;

        return pos;
    }

    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos)
            return;
        _camera.localPosition = Vector3.Lerp( _camera.localPosition, _startPos, 1 * Time.time);
    }

    private void PlayMotion(Vector3 motion)
    {
        Debug.Log("bite");
        _camera.localPosition += motion;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }
}
