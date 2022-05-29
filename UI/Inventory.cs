using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    [SerializeField] private Image image;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject contentPrefab;
    private Sprite sprite;
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    public void Display()
    {
        foreach (Transform transform in container)
        {
            GameObject.Destroy(transform.gameObject);
        }
        image.enabled = false;
        var items = ItemHolderController.Instance.GetKeyItems();
        bool isFirst = true;
        foreach (Item item in items.Keys)
        {

            var content = Instantiate(contentPrefab, container);
            Debug.Log("inventory: " + content.transform.Find("Thumbnail").gameObject.GetComponent<Image>());
            content.transform.Find("Thumbnail").gameObject.GetComponent<Image>().sprite = item.GetIcon();
            // 始めのアイテムを選択された状態にする。
            if (isFirst)
            {
                content.transform.Find("Frame").gameObject.SetActive(true);
                image.sprite = item.GetIcon();
                image.enabled = true;
                isFirst = false;
            }
        }
        gameObject.SetActive(true);

    }
    public void Hide()
    {
        gameObject.SetActive(false);

    }
}
