using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{

    public static List<GenericButton> SetupCollection(string[] items, ItemType type, Transform content)
    {
        switch (type)
        {
            case ItemType.Sosigs:
                return SetupCollection(items, ManagerUI.instance.sosigCollectionPrefab, content, ManagerUI.sosigs);
            case ItemType.Weapons:
                return SetupCollection(items, ManagerUI.instance.weaponsCollectionPrefab, content, ManagerUI.weapons);
            case ItemType.Accessories:
                return SetupCollection(items, ManagerUI.instance.accessoriesCollectionPrefab, content, ManagerUI.accessories);
        }

        return SetupCollection(items, ManagerUI.instance.sosigCollectionPrefab, content, ManagerUI.sosigs);
    }

    public static List<GenericButton> SetupCollection(string[] items, GameObject prefab, Transform content, List<Sprite> collection)
    {
        List<GenericButton> buttons = new List<GenericButton>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == "")
                continue;

            GenericButton button = GameObject.Instantiate(prefab,content).GetComponent<GenericButton>();
            button.inputField.SetTextWithoutNotify(items[i]);

            //Populate Image
            Sprite thumbnail = collection.Find(x => x.name == items[i]);
            button.image.sprite = thumbnail;
            buttons.Add(button);
        }
        return buttons;
    }
}

public enum ItemType
{
    Sosigs = 0,
    Weapons = 1,
    Accessories = 2
}