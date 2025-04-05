using UnityEngine;

public class RowButtonManager : MonoBehaviour
{
    public GameObject numberGridPanel;  // Reference to the NumberGridPanel

    // Function to toggle the visibility of the NumberGridPanel
    public void ToggleNumberGrid()
    {
        numberGridPanel.SetActive(!numberGridPanel.activeSelf);  // Toggle visibility
    }

    // Function to close the NumberGridPanel when the close button is clicked
    public void CloseNumberGrid()
    {
        numberGridPanel.SetActive(false);  // Hide the number grid
    }
}
