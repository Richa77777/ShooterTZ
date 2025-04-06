using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    
    public void UpdateHealthDisplay(int health)
    {
        _healthText.text = health.ToString();
    }
}
