using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{
    [Header("ARRAYS MUST BE SAME SIZE")]
    public int[] BuildLimitsForLevel;//Limit objects for each level
    private List<int> OriginalLimitsForLevel = new List<int>();
    public GameObject[] BuildingPrefabs;//Objects build-able by player
    public Sprite[] BuildingIcons;
    private int rotatecount = 0;//toggles between angles for rotatable objects

    public string[] MessageToShow;
    public GameObject MessagePanelPrefab;

    public GameObject UIObjectToHide, binObject, ButtonPrefab, BuildingTabPanel;
    //UIObject to hide = Build canvas
    //binObject = UI bin button
    //Button prefab = UI button for each object
    //Building tab panel = Panel for building

    private GameObject placingObject;//Current object being moved and placed
    private ValidateBuild placingValidate;

    public Texture2D NormalCursor, RedCursor;

    private List<Text> buttonTexts = new List<Text>();//Values on buttons which display limits
    private bool CanBeRotated = false, placingTeleporter;
    private int BuildingCounter = 0,currentNumber;

    public static bool ValidateBuildCount = true;

    private void Start()
    {
        //Populate building panel with all the objects which can be built
        int count = 0;
        foreach (GameObject g in BuildingPrefabs)
        {
            if (BuildLimitsForLevel[count] == 0) {
                count++;
                buttonTexts.Add(null);
                continue; }

            GameObject button = Instantiate(ButtonPrefab, transform.position, transform.rotation);
            button.transform.SetParent(BuildingTabPanel.transform);
            button.GetComponent<Image>().sprite = BuildingIcons[count];            
            button.GetComponent<Image>().preserveAspect = true;
            button.GetComponentInChildren<Text>().text = "x" + BuildLimitsForLevel[count];
            button.GetComponent<ButtonAction>().myID = count;
            button.GetComponent<ButtonAction>().Message = MessageToShow[count];
            button.GetComponent<ButtonAction>().MessageWindow = MessagePanelPrefab;

            buttonTexts.Add(button.GetComponentInChildren<Text>());

            count++;
        }

        //Store the levels original limits
        count = 0;
        foreach (int a in BuildLimitsForLevel)
        {
            OriginalLimitsForLevel.Add(BuildLimitsForLevel[count]);
            count++;
        }
    }

    //Spawn object into scene
    public void constructObject(int objectNumber)
    {
        if(BuildLimitsForLevel[objectNumber] > 0)
        {
            //Spawn prefab
            placingObject = Instantiate(BuildingPrefabs[objectNumber], transform.position, transform.rotation);
            placingObject.name = BuildingPrefabs[objectNumber].name + BuildingCounter;
            placingValidate= placingObject.GetComponentInChildren<ValidateBuild>();
            if (placingObject.GetComponent<PlaceTeleporter>())
            {
                placingObject.transform.position = Vector3.zero;
                placingObject = placingObject.GetComponent<PlaceTeleporter>().A;
                placingTeleporter = true;
            }
            else
            {
                placingObject.GetComponentInChildren<ValidateBuild>().ActivateValidator();
                CanBeRotated = placingObject.GetComponent<CanBeRotated>().canBeRotated;
            }

            currentNumber = objectNumber;
            UIObjectToHide.SetActive(false);
            binObject.SetActive(true);
            rotatecount = 0;

        }
    }

    private void Update()
    {
        if (!placingObject)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 mousePoint =GetMousePosition();
                mousePoint.z = -23;
                RaycastHit hit = new RaycastHit();
                //RaycastHit hit = Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                Debug.Log("Ray from:" + mousePoint);
                Ray ray = new Ray(mousePoint, Vector3.forward);

                LayerMask someMask = ~0;
                Physics.Raycast(ray, out hit, 100f, someMask);

                if (hit.collider)
                {
                    Debug.Log("Move: " + hit.transform.name);

                    if (hit.transform.parent)
                    {
                        if (hit.transform.parent.tag == "USEROBJECT")
                        {
                            Debug.Log("Reactive moving");
                            placingObject = hit.transform.parent.gameObject;
                            placingValidate = hit.transform.parent.GetComponentInChildren<ValidateBuild>();
                            placingValidate.ActivateValidator();
                            UIObjectToHide.SetActive(false);
                            binObject.SetActive(true);
                            currentNumber = 900;
                        }
                        else
                        {
                            Debug.Log("Failed wrong tag!");
                        }
                    }
                    else
                    {
                        Debug.Log("Failed no parent! " + hit.transform.name);
                    }
                }
            }

            return;
        }

        //Rotate objects if allowed
        if (CanBeRotated)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                int[] options = new int[] { 0, -90 };
                rotatecount++;
                if (rotatecount >= options.Length) { rotatecount = 0; }

                placingObject.transform.rotation = Quaternion.Euler(0, 0, options[rotatecount]);
            }
        }

        //Move object
        Vector3 MousePos = GetMousePosition();

        int x = Mathf.RoundToInt(MousePos.x);
        int y = Mathf.RoundToInt(MousePos.y);
        Vector3 CorrectedPosition = new Vector3(x, y,0);
        CorrectedPosition += placingValidate.PlacingOffset;

        placingObject.transform.position = CorrectedPosition;

        //Place object
        if(Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {           

            if (placingTeleporter)
            {
                if (!placingObject.GetComponentInParent<PlaceTeleporter>().FirstPlaced)
                {
                    Debug.Log("First placed");
                    placingObject.GetComponentInParent<PlaceTeleporter>().FirstPlaced = true;
                    placingObject = placingObject.GetComponentInParent<PlaceTeleporter>().B;
                    placingObject.SetActive(true);
                }
                else
                {
                    Debug.Log("Second placed");
                    BuildLimitsForLevel[currentNumber]--;
                    placingObject = null;
                    placingTeleporter = false;
                    BuildingCounter++;
                    SwitchMenu();
                }
            }
            else if (ValidateBuildCount)
            {
                if (currentNumber != 900)
                {
                    BuildLimitsForLevel[currentNumber]--;
                    //TODO bug where not all button are there
                    buttonTexts[currentNumber].text = "x" + BuildLimitsForLevel[currentNumber];
                }

                placingObject.GetComponentInChildren<ValidateBuild>().DisableValidator();
                placingObject = null;
                BuildingCounter++;
                SwitchMenu();
            }
            
        }

        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            if (ValidateBuildCount)
            {
                Cursor.SetCursor(NormalCursor, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(RedCursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    //Get mouse position in world
    private Vector3 GetMousePosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    //Destroy placing object
    public void BinIt()
    {
        Debug.Log("BinObj");
        if (placingObject.transform.parent)
        {
            Destroy(placingObject.transform.parent);
        }
        else
        {
            Destroy(placingObject);
        }

        SwitchMenu();
    }

    //Switch menu from placing to build
    private void SwitchMenu()
    {
        binObject.SetActive(false);
        UIObjectToHide.SetActive(true);
    }

    //Update limits on building objects
    public void UpdateBuildingIcons()
    {
        int count = 0;
        foreach(int b in BuildLimitsForLevel)
        {
            if (b == 0)
            {
                count++;
                continue;
            }
            buttonTexts[count].text = "x" + BuildLimitsForLevel[count];
            count++;
        }
    }

    //Reet to default level limits
    public void ResetLimits()
    {
        int count = 0;
        foreach (int a in BuildLimitsForLevel)
        {
            BuildLimitsForLevel[count] = OriginalLimitsForLevel[count];
            count++;
        }

        UpdateBuildingIcons();
    }
}
