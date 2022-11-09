using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Disck : MonoBehaviour, IInteractible
{
    // [SerializeField] private GameObject GUIhover;
    // [SerializeField] private GameObject disck;

    [SerializeField] private GameObject vcam;

    [SerializeField] private GameObject spinPart;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Material greenMat;

    [Header ("Accessibility")]
    [SerializeField] private CodeSection[] combination;
    [SerializeField] private float speed;
    [SerializeField] [Range(0, 10)] private float errorMargin;

    private List<CodeSection> combinationSelected = new List<CodeSection>();
    private ROTATION rotationDisck = ROTATION.UNKNOWN;
    private float angle;
    private float lastJoystickAngle = 0;

    bool isOpen = false;
    bool init = false;

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        if (!isOpen || joystick.magnitude < 0.7)
        {
            init = false;
            return;
        }

        float stickRotationInDeg = Mathf.Rad2Deg * Mathf.Atan2(joystick.x, joystick.y);
        if (stickRotationInDeg < 0)
            stickRotationInDeg += 360;

        float deltaAngle = stickRotationInDeg - lastJoystickAngle;

        if (!init || (init && (Mathf.Abs(deltaAngle) < 3 || Mathf.Abs(deltaAngle) > 180))) {
            lastJoystickAngle = stickRotationInDeg;
            init = true;
            return;
        }

        ROTATION rotationJoystick;
        if (lastJoystickAngle < stickRotationInDeg)
            rotationJoystick = ROTATION.CLOCKWISE;
        else
            rotationJoystick = ROTATION.COUNTER_CLOCKWISE;

        if (rotationDisck == ROTATION.UNKNOWN)
            rotationDisck = rotationJoystick;
        else if (rotationDisck != rotationJoystick)
        {
            Debug.Log("Changement de sens");
            combinationSelected.Add(new CodeSection(rotationDisck, angle));
            rotationDisck = rotationJoystick;
        }

        angle = (angle + deltaAngle * speed * Time.deltaTime) % 360;
        if (angle < 0)
            angle += 360;

        spinPart.transform.rotation = Quaternion.Euler(angle, 270, 270);

        lastJoystickAngle = stickRotationInDeg;
    }

    public void OnInteract()
    {
        if (!isOpen)
        {
            GUIManager.instance.EnableUseGUI(false);
            vcam.SetActive(true);
            PlayerControllerProto2.enablePlayerMovement = false;
            isOpen = true;
        }
        else
        {
            combinationSelected.Add(new CodeSection(rotationDisck, angle));
            bool pass = true;
            if (combinationSelected.Count <= combination.Length + 1)
            {
                for (int i = 0; i < combinationSelected.Count; i++)
                {
                    if (i < combination.Length)
                    {
                        if (combinationSelected[i].rotation == combination[i].rotation && combinationSelected[i].degrees + errorMargin > combination[i].degrees && combinationSelected[i].degrees - errorMargin < combination[i].degrees)
                            continue;
                    }
                    else
                    {
                        if (combinationSelected[i].degrees + errorMargin > combination[i - 1].degrees && combinationSelected[i].degrees - errorMargin < combination[i - 1].degrees)
                            continue;
                    }

                    pass = false;
                    break;
                }
            }
            else
                pass = false;

            if (pass)
            {
                Debug.Log("You Pass");
                indicator.GetComponent<MeshRenderer>().material = greenMat;
                StartCoroutine(PanelComplet());
            }
            else
            {
                Debug.Log("You don't pass");
                angle = 0;
                rotationDisck = ROTATION.UNKNOWN;
                spinPart.transform.rotation = Quaternion.Euler(angle, 270, 270);
                combinationSelected.Clear();
            }
        }
    }

    public void OnItemExit()
    {
        GUIManager.instance.EnableUseGUI(false);
    }

    public void OnItemHover()
    {
        GUIManager.instance.EnableUseGUI(true);
    }

    public void OnReturn()
    {
        isOpen = false;
        GUIManager.instance.EnableUseGUI(false);
        vcam.SetActive(false);
        PlayerControllerProto2.enablePlayerMovement = true;
    }

    [System.Serializable]
    struct CodeSection
    {
        public ROTATION rotation;
        [Range(0, 360)] public float degrees;

        public CodeSection(ROTATION _rot, float _deg)
        {
            rotation = _rot;
            degrees = _deg;
        }
    }

    private enum ROTATION
    {
        UNKNOWN,
        CLOCKWISE,
        COUNTER_CLOCKWISE
    }

    private IEnumerator PanelComplet()
    {
        vcam.SetActive(false);
        gameObject.layer = 0;
        yield return new WaitForSeconds(2);
        PlayerControllerProto2.enablePlayerMovement = true;
    }
}
