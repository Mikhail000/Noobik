using UnityEngine;  

[CreateAssetMenu]
public class MenuButtonConfig : ScriptableObject
{
    [field: Header("Input Data")]
    [field:Space]
    
    [field:SerializeField] public Color AvailableColor { get; private set; }
    [field:SerializeField] public Color HightlightColor { get; private set; }
    [field:SerializeField] public Color DisableColor { get; private set; }
    [field:SerializeField] public Color PressedColor { get; private set; }
    
    private static MenuButtonConfig _instance = default;

    public static MenuButtonConfig Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<MenuButtonConfig>(nameof(MenuButtonConfig));
            return _instance;
        }
    } 
}
