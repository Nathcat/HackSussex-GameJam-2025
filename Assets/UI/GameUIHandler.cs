using UnityEngine;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public UIDocument UIDoc;
    private Label m_healthLabel;
    private VisualElement m_healthBarMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_healthLabel = UIDoc.rootVisualElement.Q<Label>("HealthLabel");
        m_healthBarMask = UIDoc.rootVisualElement.Q<VisualElement>("HealthBarMask");
        maxHealth = 100;
        currentHealth = 100;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > 0)
        {
            currentHealth = currentHealth - (float)0.1;
            HealthChanged();
        }
        else
        { 
        }
        
    }

    void HealthChanged()
    {
        float healthRatio = currentHealth / maxHealth;
        float healthPercent = Mathf.Lerp(0, 100, healthRatio);
        m_healthBarMask.style.width = Length.Percent(healthPercent);
        float rounded = Mathf.Round(currentHealth);
        m_healthLabel.text = $"{rounded}/{maxHealth}";
    }
}
