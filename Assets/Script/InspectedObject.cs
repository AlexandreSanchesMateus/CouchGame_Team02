using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InspectedObject : MonoBehaviour , IInteractible
{
    [SerializeField] private bool canBeInspected = true;
    [SerializeField] private float defaultThrowForce = 100;
    [SerializeField] private float deltaHardThrow = 400;
    [SerializeField, Range(0.01f, 5)] private float turnSensibitive = 0.5f;

    [HideInInspector] public Rigidbody rb;
    private BoxCollider boxCollider;
    private Transform startParent;

    Sequence PickUpSequence = DOTween.Sequence();
    public delegate void OnGrabListner(InspectedObject source);
    public OnGrabListner OnGrab;

    private bool isInHand = false;
    private bool inspectMode = false;
    private float throwForce;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        startParent = gameObject.transform.parent;
        throwForce = defaultThrowForce;
    }

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        if (!isInHand || !inspectMode)
            return;

        gameObject.transform.Rotate(joystick.y * turnSensibitive, 0, joystick.x * turnSensibitive);
    }

    public void OnInteract()
    {
        if (isInHand)
            return;

        inspectMode = false;
        GUIManager.instance.EnablePick_upGUI(false);
        throwForce = defaultThrowForce;
        rb.constraints = RigidbodyConstraints.None;

        if(OnGrab != null)
            OnGrab(this);

        // Attach (move to transform parent)
        gameObject.transform.SetParent(PlayerControllerProto2.instance.hand);
        // disable colider
        boxCollider.enabled = false;
        // disable RigidBody
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // move to location
        PickUpSequence = DOTween.Sequence();
        PickUpSequence.Append(transform.DOLocalMove(Vector3.zero, 0.8f).SetEase(Ease.InOutSine));
        PickUpSequence.Join(transform.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f));

        isInHand = true;
    }

    public void OnItemExit()
    {
        GUIManager.instance.EnablePick_upGUI(false);
    }

    public void OnItemHover()
    {
        GUIManager.instance.EnablePick_upGUI(true);
    }

    public void OnReturn()
    {
        PlayerControllerProto2.enablePlayerMovement = true;

        if (!PickUpSequence.IsComplete())
            PickUpSequence.Kill();

        // Dettach
        gameObject.transform.SetParent(startParent);
        // enable colider
        boxCollider.enabled = true;
        // enable rigidbody
        rb.useGravity = true;
        rb.AddForce(PlayerControllerProto2.instance.cameraObj.transform.forward * throwForce);
        rb.AddRelativeTorque(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));

        isInHand = false;
    }

    public void OnRightShoulder()
    {
        if (!canBeInspected)
            return;

        inspectMode = !inspectMode;

        if(inspectMode)
            PlayerControllerProto2.enablePlayerMovement = false;
        else
            PlayerControllerProto2.enablePlayerMovement = true;

    }

    public void OnHoldReturn()
    {
        throwForce += deltaHardThrow;
    }
}
