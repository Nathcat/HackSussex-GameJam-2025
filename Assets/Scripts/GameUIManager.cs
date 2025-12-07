using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    private UIDocument UIDoc;
    private Label m_healthLabel;
    private Label m_manaLabel;
    private Label m_staminaLabel;
    private VisualElement m_healthBarMask;
    private VisualElement m_manaBarMask;
    private VisualElement m_staminaBarMask;

    void Start()
    {
        GameManager.instance.player.onHeathChange.AddListener(HealthChanged);
        UIDoc = GetComponent<UIDocument>();
        m_healthLabel = UIDoc.rootVisualElement.Q<Label>("HealthLabel");
        m_healthBarMask = UIDoc.rootVisualElement.Q<VisualElement>("HealthBarMask");
        m_manaLabel = UIDoc.rootVisualElement.Q<Label>("ManaLabel");
        m_manaBarMask = UIDoc.rootVisualElement.Q<VisualElement>("ManaBarMask");
        HealthChanged();
        ManaChanged();
    }

    void HealthChanged()
    {
        float currentHealth = GameManager.instance.player.health;
        float maxHealth = GameManager.instance.player.maxHealth;

        float healthRatio = currentHealth / maxHealth;
        float healthPercent = Mathf.Lerp(0, 100, healthRatio);
        m_healthBarMask.style.width = Length.Percent(healthPercent);
        float rounded = Mathf.Round(currentHealth);
        m_healthLabel.text = $"{rounded}/{maxHealth}";
    }

    void ManaChanged()
    {
        float currentMana = GameManager.instance.player.Mana;
        float maxMana = GameManager.instance.player.MaxMana;

        float manaRatio = currentMana / maxMana;
        float manaPercent = Mathf.Lerp(0, 100, manaRatio);
        m_manaBarMask.style.width = Length.Percent(manaPercent);
        float rounded = Mathf.Round(currentMana);
        m_manaLabel.text = $"{rounded}/{maxMana}";
    }
}
