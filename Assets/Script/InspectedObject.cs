using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InspectedObject : MonoBehaviour , IInteractible
{
    [SerializeField] private bool canBeInspected = true;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Transform startParent;

    Sequence PickUpSequence;

    private bool isInHand = false;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        startParent = gameObject.transform.parent;
    }

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        
    }

    public void OnInteract()
    {
        if (isInHand)
            return;

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
        if (!PickUpSequence.IsComplete())
            PickUpSequence.Kill();

        // Dettach
        gameObject.transform.SetParent(startParent);
        // enable colider
        boxCollider.enabled = true;
        // enable rigidbody
        rb.useGravity = true;
        rb.AddForce(PlayerControllerProto2.instance.cameraObj.transform.forward * 100);
        rb.AddRelativeTorque(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));

        isInHand = false;
    }
}
