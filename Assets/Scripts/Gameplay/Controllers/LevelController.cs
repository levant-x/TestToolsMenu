using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject cnvMenu;
    [SerializeField] int minSpawnCount = 15;
    [SerializeField] int maxSpawnCount = 25;

    public ToolsListController toolsListController;

    void Start()
    {
        SetupTools();
    }

    void Update()
    {
        if (!Input.GetKeyUp(KeyCode.Tab)) return;
        ToggleMenu();
    }

    private void SetupTools()
    {
        for (int i = 0; i < Random.Range(minSpawnCount, maxSpawnCount); i++) SetupTool();
    }

    private void SetupTool()
    {
        var thisBounds = ((RectTransform)transform).rect;
        float x = thisBounds.width / 2 - 100;
        float y = thisBounds.height / 2 - 100;

        var testTool = Instantiate(toolsListController.toolPrefab);
        testTool.transform.position = new Vector3(Random.Range(-x, x), Random.Range(-y, y));

        var testToolTemplate = testTool.GetComponent<ToolTemplate>();
        testToolTemplate.Key = ToolTemplate.Keys[Random.Range(0, ToolTemplate.Keys.Length)];
        testToolTemplate.Count = Random.Range(1, 20);
        testToolTemplate.OnClick += HandleClickOnToolTempl;
        
        testTool.transform.SetParent(transform, false);
        testTool.transform.SetAsFirstSibling(); // to be hidden behind overlay
    }

    private void ToggleMenu()
    {
        cnvMenu.SetActive(!cnvMenu.activeSelf);
    }

    private void HandleClickOnToolTempl(GameObject tool)
    {
        toolsListController.PickItem(tool);
        tool.GetComponent<ToolTemplate>().OnClick -= HandleClickOnToolTempl;
    }
}
