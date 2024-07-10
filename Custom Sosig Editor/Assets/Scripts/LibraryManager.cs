using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    public static LibraryManager instance;
    public static GenericButton selectedItem;
    public GameObject libraryMenu;
    private GenericButton selectedButton;

    public Transform itemContent;
    private List<GenericButton> itemButtons = new List<GenericButton>();
    public GameObject buttonPrefab;

    private bool sortOrder = false;

    private void Awake()
    {
        instance = this;
    }

    public void OpenSosigLibrary(GenericButton button)
    {
        selectedButton = button;
        libraryMenu.SetActive(true);
        SetupLibrary(ManagerUI.sosigs, true);
    }

    public void OpenWeaponsLibrary(GenericButton button)
    {
        selectedButton = button;
        libraryMenu.SetActive(true);
        SetupLibrary(ManagerUI.weapons, false);
    }

    public void OpenAccessoriesLibrary(GenericButton button)
    {
        selectedButton = button;
        libraryMenu.SetActive(true);
        SetupLibrary(ManagerUI.accessories, false);
    }

    public void CloseLibrary(bool empty, GenericButton selectedItem)
    {
        if (empty)
            LibraryManager.selectedItem = null;

        if (selectedButton)
        {
            selectedButton.image.sprite = selectedItem.image.sprite;
            selectedButton.inputField.text = selectedItem.description;
        }
        selectedButton = null;

        libraryMenu.SetActive(false);
    }

    public void SetupLibrary(List<Sprite> sprites, bool idPrefix)
    {
        for (int i = itemButtons.Count - 1; i >= 0; i--)
        {
            Destroy(itemButtons[i].gameObject);
        }
        itemButtons.Clear();

        for (int i = 0; i < sprites.Count; i++)
        {
            GenericButton btn = Instantiate(buttonPrefab.gameObject, itemContent).GetComponent<GenericButton>();
            btn.description = sprites[i].name;
            btn.text.text = sprites[i].name;
            btn.image.sprite = sprites[i];
            btn.gameObject.SetActive(true);

            if(idPrefix)
                btn.id = int.Parse(GetInitialNumber(sprites[i].name));

            itemButtons.Add(btn);
        }
    }

    public void SortByName()
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            if(sortOrder)
                itemButtons[i].transform.SetAsFirstSibling();
            else
                itemButtons[i].transform.SetAsLastSibling();
        }

        sortOrder = !sortOrder;
    }

    public static string GetInitialNumber(string input)
    {
        // Regular expression to match initial numbers at the start of the string
        string pattern = @"^\d+";

        // Find match
        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, pattern);

        // Return the matched value if found, otherwise null
        return match.Success ? match.Value : null;
    }
}
