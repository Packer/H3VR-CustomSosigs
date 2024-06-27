using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericButton : MonoBehaviour
{
    public int index = -1;
    public string description;
    public int id = -1;
    public Text text;
    public Image image;

    public void SelectLibraryItem()
    {
        LibraryManager.selectedItem = this;
        LibraryManager.instance.CloseLibrary(false);
    }
}
