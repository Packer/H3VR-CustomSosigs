using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Global
{
    public static Sprite GetSpriteByName(ItemType type ,string description)
    {
        Sprite foundSprite = null;
        switch (type)
        {
            default:
            case ItemType.Sosigs:
                foundSprite = ManagerUI.sosigs.Find(x => x.name == description);
                break;
            case ItemType.Weapons:
                foundSprite = ManagerUI.weapons.Find(x => x.name == description);
                break;
            case ItemType.Accessories:
                foundSprite = ManagerUI.accessories.Find(x => x.name == description);
                break;
        }

        if(foundSprite != null)
            return foundSprite;
        return ManagerUI.DefaultSprite();

    }

    public static List<string> GenericButtonsToStringList(GenericButton[] inputs)
    {
        List<string> collection = new List<string>();
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].inputField.text == "")
                collection.Add("");
            else
                collection.Add(inputs[i].inputField.text);
        }
        return collection;
    }
    public static List<float> GenericButtonsToFloatList(GenericButton[] inputs)
    {
        List<float> collection = new List<float>();
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].inputField.text == "")
                collection.Add(0);
            else
                collection.Add(float.Parse(inputs[i].inputField.text));
        }
        return collection;
    }
    public static List<float> GenericButtonsXToFloatList(GenericButton[] inputs)
    {
        List<float> collection = new List<float>();
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].inputFieldX.text == "")
                collection.Add(0);
            else
                collection.Add(float.Parse(inputs[i].inputFieldX.text));
        }
        return collection;
    }

    public static GenericButton SetupCollectionButton(string item, ItemType type, Transform content, int index = -1)
    {
        GameObject prefab;
        List<Sprite> collection;

        switch (type)
        {
            default:
            case ItemType.Sosigs:
                prefab = ManagerUI.instance.sosigCollectionPrefab;
                collection = ManagerUI.sosigs;
                break;
            case ItemType.Weapons:
                prefab = ManagerUI.instance.weaponsCollectionPrefab;
                collection = ManagerUI.weapons;
                break;
            case ItemType.Accessories:
                prefab = ManagerUI.instance.accessoriesCollectionPrefab;
                collection = ManagerUI.accessories;
                break;
        }

        GenericButton button = GameObject.Instantiate(prefab, content).GetComponent<GenericButton>();
        button.gameObject.SetActive(true);

        if(button.transform.parent && button.transform.parent.childCount >= 2)
            button.transform.SetSiblingIndex(button.transform.parent.childCount - 2);
        button.inputField.SetTextWithoutNotify(item);
        button.index = index;

        //Populate Image
        if (item != "")
        {
            Sprite thumbnail = collection.Find(x => x.name == item);
            button.image.sprite = thumbnail;
        }

        return button;
    }

    public static List<GenericButton> SetupCollection(List<string> items, ItemType type, Transform content)
    {
        List<GenericButton> buttons = new List<GenericButton>();
        for (int i = 0; i < items.Count; i++)
        {
            buttons.Add(SetupCollectionButton(items[i], type, content, i));
        }
        return buttons;
    }

    internal static class ObjectCloner
    {
        public static T Clone<T>(T obj)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(buffer, obj);
                buffer.Position = 0;
                T temp = (T)formatter.Deserialize(buffer);
                return temp;
            }
        }
    }
}

public enum ItemType
{
    Sosigs = 0,
    Weapons = 1,
    Accessories = 2,
    Items = 2
}