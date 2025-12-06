using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    private UIDocument UIDoc;
    private Label m_healthLabel;
    private VisualElement m_healthBarMask;
    
    void Start()
    {
        GameManager.instance.player.onHeathChange.AddListener(HealthChanged);
        UIDoc = GetComponent<UIDocument>();
        m_healthLabel = UIDoc.rootVisualElement.Q<Label>("HealthLabel");
        m_healthBarMask = UIDoc.rootVisualElement.Q<VisualElement>("HealthBarMask");
        HealthChanged();
        
    }

    void HealthChanged()
    {
        float currentHealth = GameManager.instance.player.Health;
        float maxHealth = GameManager.instance.player.MaxHeath;

        float healthRatio = currentHealth / maxHealth;
        float healthPercent = Mathf.Lerp(0, 100, healthRatio);
        m_healthBarMask.style.width = Length.Percent(healthPercent);
        float rounded = Mathf.Round(currentHealth);
        m_healthLabel.text = $"{rounded}/{maxHealth}";
    }
}
