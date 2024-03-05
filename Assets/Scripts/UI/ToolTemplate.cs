using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTemplate : MonoBehaviour, IIdentifiable, IPointerClickHandler
{
    static readonly Dictionary<string, Tool> keyToToolSpecs;
    const string SPECS_PATH = "Assets/Resources/Specs/equips.json";
    const string BONUS_ICONS_PATH = "Sprites/Equips/BonusIcons";

    private string _key = null;
    private int _count = 0;
    private bool _isSelected = false;
    private Tool _spec;

    [SerializeField] Image imgToolTexture;
    [SerializeField] Image imgBonusIconTexture;
    [SerializeField] GameObject imgOverlay;
    [SerializeField] TextMeshProUGUI txBonusRateLabel;
    [SerializeField] TextMeshProUGUI txCountLabel;

    public event Action<GameObject> OnClick;
    public event Action<GameObject> OnInactivate;

    public static string[] Keys { get => keyToToolSpecs.Keys.ToArray(); }

    static ToolTemplate()
    {
        using var file = File.OpenRead(SPECS_PATH);
        var freader = new StreamReader(file);
        var specs = JsonConvert.DeserializeObject<Tool[]>(freader.ReadToEnd());
        keyToToolSpecs = specs.ToDictionary(tool => tool.Key);
    }

    public int Count 
    { 
        get => _count; 
        set 
        {
            _count = value; 
            txCountLabel.text = $"X{_count}";
        }
    }

    public string Key 
    { 
        get => _key; 
        set 
        {
            var wasInit = _key == null;
            _key = value; 
            if (wasInit) SetupIntrinsicData();
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            imgOverlay.SetActive(_isSelected);
        }
    }

    public Tool Spec { get => _spec; }

    void OnDisable()
    {
        OnInactivate?.Invoke(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(gameObject);
    }

    private void SetupIntrinsicData()
    {
        _spec = keyToToolSpecs[_key];
        var toolTexture = (Texture2D)Resources.Load(_spec.TexturePath.Replace(".png", string.Empty));
        imgToolTexture.sprite = Sprite.Create(toolTexture, new Rect(0, 0, toolTexture.width, toolTexture.height), new Vector2(.5f, .5f));

        var statusIcon = Resources.LoadAll(BONUS_ICONS_PATH)[(int)_spec.Bonus + 1];
        imgBonusIconTexture.sprite = (Sprite)statusIcon;

        txBonusRateLabel.text = $"X{_spec.BonusRate}";
    }
}
