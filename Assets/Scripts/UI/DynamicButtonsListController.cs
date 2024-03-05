using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonsListController : MonoBehaviour
{
    private readonly List<int> _scope = new();

    [SerializeField] GameObject parent;
    [SerializeField] GameObject buttonPrefab;

    public string IconsPath;

    public event Action<int> OnClick;

    public void InscopeIndex(int index)
    {
        if (_scope.Contains(index)) return;
        
        _scope.Add(index);
        ScopeContent();
    }

    public void UnscopeIndex(int index)
    {
        _scope.Remove(index);
        ScopeContent();
    }

    public void InvokeShowAll()
    {
        OnClick?.Invoke(-1);
    }

    void Start()
    {
        var icons = Resources.LoadAll(IconsPath);
        for (int i = 1; i < icons.Length; i++) CreateIconButton(icons[i], i - 1);
    }

    void OnEnable()
    {
        ScopeContent();
    }

    private void ScopeContent()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("All filter")) continue;
            
            int index = int.Parse(child.name);
            child.gameObject.SetActive(_scope.Contains(index));
        }
    }

    private void HandleClick(int index)
    {
        OnClick?.Invoke(index);
    }

    private void CreateIconButton(UnityEngine.Object icon, int index)
    {
        var button = Instantiate(buttonPrefab);
        button.name = index.ToString();
        button.transform.SetParent(transform, false);
        button.transform.SetAsFirstSibling();

        var buttonImage = button.GetComponent<Image>();
        buttonImage.sprite = (Sprite)icon;
        buttonImage.type = Image.Type.Simple;

        var buttonComponent = button.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => HandleClick(index));
    }
}
