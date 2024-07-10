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
    public InputField searchInput;
    public Text libraryTitle;

    public Transform itemContent;
    private List<GenericButton> itemButtons = new List<GenericButton>();
    public GameObject buttonPrefab;

    private bool sortOrder = false;

    private string lastLoaded;

    private void Awake()
    {
        instance = this;
    }

    public void OpenSosigLibrary(GenericButton button)
    {
        selectedButton = button;
        libraryMenu.SetActive(true);
        StartCoroutine(SetupLibrary(ManagerUI.sosigs, true, "Sosig Library"));
    }

    public void OpenWeaponsLibrary(GenericButton button)
    {
        selectedButton = button;
        libraryMenu.SetActive(true);
        StartCoroutine(SetupLibrary(ManagerUI.weapons, false, "Weapons Library"));
        //SetupLibrary(ManagerUI.weapons, false, "Weapons Library");
    }

    public void OpenAccessoriesLibrary(GenericButton button)
    {
        selectedButton = button;
        libraryMenu.SetActive(true);
        StartCoroutine(SetupLibrary(ManagerUI.accessories, false, "Accessories Library"));
    }

    public void CloseLibrary()
    {
        CloseLibrary(null);
    }

    public void CloseLibrary(GenericButton selectedItem)
    {
        if (selectedItem == null)
            LibraryManager.selectedItem = null;

        if (selectedButton && selectedItem != null)
        {
            selectedButton.image.sprite = selectedItem.image.sprite;
            //Notify button of value change so it saves
            selectedButton.inputField.text = selectedItem.description;
            selectedButton.inputField.ActivateInputField();
        }
        selectedButton = null;

        libraryMenu.SetActive(false);
    }

    public static IEnumerator SetupLibrary(List<Sprite> sprites, bool idPrefix, string title)
    {
        ManagerUI.instance.loadingScreen.SetActive(true);
        instance.libraryTitle.text = title;
        yield return null;

        if (instance.lastLoaded == title)
        {
            ManagerUI.instance.loadingScreen.SetActive(false);
            yield break;
        }
        else
            instance.lastLoaded = title;


        for (int i = instance.itemButtons.Count - 1; i >= 0; i--)
        {
            Destroy(instance.itemButtons[i].gameObject);
        }
        instance.itemButtons.Clear();

        for (int i = 0, x = 0; i < sprites.Count; i++, x++)
        {
            GenericButton btn = Instantiate(instance.buttonPrefab.gameObject, instance.itemContent).GetComponent<GenericButton>();
            btn.description = sprites[i].name;
            btn.text.text = sprites[i].name;
            btn.image.sprite = sprites[i];
            btn.gameObject.SetActive(true);

            if(idPrefix)
                btn.id = int.Parse(GetInitialNumber(sprites[i].name));

            instance.itemButtons.Add(btn);

            if (x == 100)
            {
                x = 0;
                yield return null;
            }
        }

        ManagerUI.instance.loadingScreen.SetActive(false);
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

    public void SearchName()
    {
        if (searchInput.text == "")
        {
            for (int i = 0; i < itemButtons.Count; i++)
            {
                itemButtons[i].gameObject.SetActive(true);
            }
            return;
        }

        for (int i = 0; i < itemButtons.Count; i++)
        {
            if(itemButtons[i].description.Contains(searchInput.text, System.StringComparison.OrdinalIgnoreCase))
                itemButtons[i].gameObject.SetActive(true);
            else
                itemButtons[i].gameObject.SetActive(false);
        }

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
