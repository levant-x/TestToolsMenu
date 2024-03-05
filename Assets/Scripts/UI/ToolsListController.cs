using System;
using System.Collections.Generic;
using UnityEngine;

public class ToolsListController : MonoBehaviour
{
    private readonly List<ToolTemplate> _toolTemplates = new();
    private ToolTemplate _currentTool = null;
    private ToolType _currentTypeFilter = Enum.Parse<ToolType>(DEFAULT_TYPE);
    private int _currentBonusFilter = -1; // show all
    private Func<Tool, bool> filterByType;
    private Func<Tool, bool> filterByStatus;
    private const string DEFAULT_TYPE = "Armor";

    [SerializeField] DynamicButtonsListController bonusFilter;
    [SerializeField] GameObject buttonDelete;

    public GameObject toolPrefab;
    

    public void PickItem(GameObject item)
    {
        item.transform.SetParent(gameObject.transform, false); // just move from scene to inventory
        var itemTemplate = item.GetComponent<ToolTemplate>();

        _toolTemplates.Add(itemTemplate);
        bonusFilter.InscopeIndex((int)itemTemplate.Spec.Bonus);
        itemTemplate.OnClick += Select;
        itemTemplate.OnInactivate += Unselect;
    }

    void Start()
    {
        bonusFilter.OnClick += FilterListByBonus;
        filterByType = (Tool spec) => spec.Type == _currentTypeFilter;
        filterByStatus = (Tool spec) => (int)spec.Bonus == _currentBonusFilter || _currentBonusFilter < 0;
    }

    void OnEnable()
    {
        ApplyFilters();
    }

    public void OnDeleteClick()
    {
        bonusFilter.UnscopeIndex((int)_currentTool.Spec.Bonus);
        _toolTemplates.Remove(_currentTool);
        _currentTool.OnClick -= Select;
        _currentTool.OnInactivate -= Unselect;
        Destroy(_currentTool.gameObject);
        _currentTool = null;
    }

    public void FilterListByType(string toolType)
    {
        _currentTypeFilter = Enum.Parse<ToolType>(toolType);
        ApplyFilters();
    }

    private void FilterListByBonus(int index)
    {
        _currentBonusFilter = index;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        bool allFilters(Tool tool) => filterByType(tool) && filterByStatus(tool);
        foreach (var toolTmpl in _toolTemplates) toolTmpl.gameObject.SetActive(allFilters(toolTmpl.Spec));
    }

    private void Select(GameObject tool)
    {
        _currentTool = tool.GetComponent<ToolTemplate>();
        buttonDelete.SetActive(true);
    }

    private void Unselect(GameObject tool)
    {
        _currentTool = null;
        buttonDelete.SetActive(false);
    }
}