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
    public InputField inputField;
    public InputField inputFieldX;
    public InputField inputFieldY;
    public InputField inputFieldZ;

    public void SelectLibraryItem()
    {
        LibraryManager.selectedItem = this;
        LibraryManager.instance.CloseLibrary(this);
    }

    public void TrashLinkSpawnButton()
    {
        ConfigTemplateUI.instance.RemoveLinkSpawn(this);
    }

    public void TrashOutfitButton()
    {
        OutfitConfigUI.instance.RemoveAccessory(this);
    }

    public void TrashWeaponButton()
    {
        SosigEnemyTemplateUI.instance.RemoveWeapon(this);
    }

    public void UpdateSosigThumbnail()
    {
        image.sprite = Global.GetSpriteByName(ItemType.Sosigs, inputField.text);
    }

    public void UpdateWeaponThumbnail()
    {
        image.sprite = Global.GetSpriteByName(ItemType.Weapons, inputField.text);
    }

    public void UpdateAccessoryThumbnail()
    {
        image.sprite = Global.GetSpriteByName(ItemType.Accessories, inputField.text);
    }
    public void UpdateTextureThumbnail()
    {
        image.sprite = Global.GetSpriteByName(ItemType.Textures, inputField.text);
    }
}
