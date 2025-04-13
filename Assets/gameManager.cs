using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class gameManager : MonoBehaviour
{
    [Header ("Texts")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI pushsesText;
    public TextMeshProUGUI movesText;
    public int playerMoves;
    public int playerPushes;
    public TextMeshProUGUI actualLevel;
    public float time;
    public Slider sliderControl;
    public Camera cam;
    private GizmoGrid gizmoGrid;
    public TMP_InputField widthInput;
    public TMP_InputField heightInput;

    private void Start()
    {
        cam.orthographicSize = sliderControl.value;
        sliderControl.onValueChanged.AddListener(CameraControl);
        gizmoGrid = FindAnyObjectByType<GizmoGrid>();

        widthInput.text = gizmoGrid.width.ToString();
        heightInput.text = gizmoGrid.height.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        TimeControl();
        movesText.text = playerMoves.ToString();
        pushsesText.text = playerPushes.ToString();
    }

    void CameraControl(float value)
    {
        cam.orthographicSize = value;
    }
    public void ConfirmGrid()
    {
        if (int.TryParse(widthInput.text, out int newWidth))
        {
            gizmoGrid.width = newWidth;
        }

        if (int.TryParse(heightInput.text, out int newHeight))
        {
            gizmoGrid.height = newHeight;
        }

        gizmoGrid.RefreshGrid();
    }
    public void TimeControl()
    {
        time += Time.deltaTime;

        int seconds = Mathf.FloorToInt(time % 60);
        int minutes = Mathf.FloorToInt((time / 60) % 60);
        int hours = Mathf.FloorToInt((time / 3600) % 24);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
