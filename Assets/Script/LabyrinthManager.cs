using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class LabyrinthManager : MonoBehaviour , IInteractible
{
    public static LabyrinthManager instance { get; private set; }

    public GameObject king;
    public ScrLabyrinth labyrinth;
    public GameObject beginScreen;

    public GameObject vcam;
    public Transform chestParent;

    private List<Transform> slotChest = new List<Transform>();
    private bool isOpen = false;
    private static bool canNewInput = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log(chestParent.childCount);

        for (int i = 0; i < chestParent.childCount; i++)
        {
            Transform horizontaleParent = chestParent.GetChild(i);

            for (int y = 0; y < horizontaleParent.childCount; y++)
            {
                slotChest.Add(horizontaleParent.GetChild(y));
            }
        }

        labyrinth.idPlayerSlot = labyrinth.idStartLabyrinth;
        // king.transform.localPosition = slotChest[labyrinth.idPlayerSlot].localPosition;
        king.transform.SetParent(slotChest[labyrinth.idPlayerSlot].transform);
        king.transform.localPosition = Vector3.zero;

        gameObject.layer = LayerMask.GetMask("Default");
        beginScreen.SetActive(true);
    }

    public void InitLabyrintheScreen()
    {
        gameObject.layer = LayerMask.GetMask("Interactible");
        beginScreen.SetActive(false);
    }

    public void MovePlayerOnGrid(Vector2 direction, bool isRobberMoving = false)
    {
        if (!canNewInput)
            return;

        bool allowToMove = false;

        switch (direction)
        {
                // ---- Vers la droite ---- //
            case { x: 1, y: 0 }:
                allowToMove = CheckSlotAccess(labyrinth.grid[labyrinth.idPlayerSlot].rightWall, isRobberMoving);
                if(allowToMove)
                    labyrinth.idPlayerSlot++;
                break;

                // ---- Vers le haut ---- //
            case { x: 0, y: 1 }:
                if (labyrinth.idPlayerSlot - labyrinth.gridSizeX < 0)
                    break;

                allowToMove = CheckSlotAccess(labyrinth.grid[labyrinth.idPlayerSlot - labyrinth.gridSizeX].bottomWall, isRobberMoving);
                if (allowToMove)
                    labyrinth.idPlayerSlot -= labyrinth.gridSizeX;
                break;

                // ---- Vers la gauche ---- //
            case { x: -1, y: 0 }:
                if (labyrinth.idPlayerSlot - 1 < 0 || labyrinth.idPlayerSlot % labyrinth.gridSizeY == 0 )
                    break;

                allowToMove = CheckSlotAccess(labyrinth.grid[labyrinth.idPlayerSlot - 1].rightWall, isRobberMoving);
                if (allowToMove)
                    labyrinth.idPlayerSlot --;
                break;

                // ---- Vers le bas ---- //
            case { x: 0, y: -1 }:
                allowToMove = CheckSlotAccess(labyrinth.grid[labyrinth.idPlayerSlot].bottomWall, isRobberMoving);
                if (allowToMove)
                    labyrinth.idPlayerSlot += labyrinth.gridSizeX;
                break;
        }

        if (allowToMove)
        {
            // Vector3 position = slotChest[labyrinth.idPlayerSlot].position;
            king.transform.SetParent(slotChest[labyrinth.idPlayerSlot].transform);
            //robberRound = !robberRound;
            canNewInput = false;
            Sequence moveKinkSequence = DOTween.Sequence();
            // moveKinkSequence.Append(king.transform.DOJump(position, 0.3f, 1, 0.8f).SetEase(Ease.Linear));
            moveKinkSequence.Append(king.transform.DOLocalMove(Vector3.zero, 0.8f));
            moveKinkSequence.AppendInterval(0.2f);
            moveKinkSequence.AppendCallback(() => canNewInput = true);

            if (labyrinth.idPlayerSlot == labyrinth.idEndLabyrinth)
                Debug.Log("WIN");
        }
    }

    private bool CheckSlotAccess(Slot.ACCESS access, bool isRobberMoving)
    {
        switch (access)
        {
            case Slot.ACCESS.BLOCK:
                return false;

            case Slot.ACCESS.BOTH:
                return true;

            case Slot.ACCESS.EITHER:
                ResetLabyrinthe();
                return false;

            case Slot.ACCESS.HACKER:
                if (!isRobberMoving)
                    return true;
                else
                {
                    ResetLabyrinthe();
                    return false;
                }

            case Slot.ACCESS.ROBBER:
                if (isRobberMoving)
                    return true;
                else
                {
                    ResetLabyrinthe();
                    return false;
                }
        }

        return false;
    }

    private void ResetLabyrinthe()
    {
        canNewInput = false;
        labyrinth.idPlayerSlot = labyrinth.idStartLabyrinth;
        king.transform.SetParent(slotChest[labyrinth.idPlayerSlot].transform);

        Sequence resetBoardSequence = DOTween.Sequence();
        //resetBoardSequence.Append(king.transform.DOShakePosition(0.6f, 0.05f));
        //resetBoardSequence.AppendInterval(0.8f);
        // resetBoardSequence.Append(king.transform.DOJump(slotChest[labyrinth.idPlayerSlot].position, 1, 1, 1.1f));
        // resetBoardSequence.Join(king.transform.DOPunchRotation(new Vector3(20, 20, 20), 1.1f, 1));
        resetBoardSequence.Append(king.transform.DOLocalMove(Vector3.zero, 0.8f));
        resetBoardSequence.AppendInterval(0.5f);
        resetBoardSequence.AppendCallback(() => canNewInput = true);
    }



    // -------------------------------------- Interface -------------------------------------- //

    public void OnItemHover()
    {
        GUIManager.instance.EnableUseGUI(true);
    }

    public void OnItemExit()
    {
        GUIManager.instance.EnableUseGUI(false);
    }

    public void OnInteract()
    {
        if (!isOpen)
        {
            GUIManager.instance.EnableUseGUI(false);
            PlayerControllerProto2.enablePlayerMovement = false;
            vcam.SetActive(true);
            isOpen = true;
        }
    }

    public void OnReturn()
    {
        vcam.SetActive(false);
        PlayerControllerProto2.enablePlayerMovement = true;
        isOpen = false;
    }

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        if (action == Vector2.zero)
            return;

        MovePlayerOnGrid(action.normalized, true);
    }

    public void OnRightShoulder() { }

    public void OnHoldReturn() { }
}
