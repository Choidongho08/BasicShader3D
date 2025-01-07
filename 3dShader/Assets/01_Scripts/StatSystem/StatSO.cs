using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "StatSO", menuName = "SO/StatSO")]
public class StatSO : ScriptableObject, ICloneable
{
    public delegate void ValueChangeHandle(StatSO stat, float currentValue, float previousValue);
    public event ValueChangeHandle OnValueChanged;

    public string statName;
    public string description;
    public string displayName;
    public Sprite icon;

    private float _baseValue, _minValue, _maxValue;

    private Dictionary<string, float> _modifyValueByKey = new Dictionary<string, float>();
    [field : SerializeField] public bool IsPercent { get; private set; }

    private float _modifiedValue = 0;

    public float MaxValue
    {
        get => _maxValue;
        set => _maxValue = value;
    }

    public float MinValue
    {
        get => _minValue;
        set => _minValue = value;
    }

    public float Value => Mathf.Clamp(_baseValue + _modifiedValue, _minValue, _maxValue);
    public bool isMax => Mathf.Approximately(_baseValue, _maxValue);
    public bool isMin  => Mathf.Approximately(_baseValue, _minValue);

    public float BaseValue
    {
        get => _baseValue;
        set 
        {
            float previouseValue = Value;
            _baseValue = Mathf.Clamp(value, MinValue, MaxValue);
            TryInvokeValueChangeEvent(Value, previouseValue);
        }
    }

    private void TryInvokeValueChangeEvent(float value, float previouseValue)
    {
        if(Mathf.Approximately(value, previouseValue) == false) 
            OnValueChanged?.Invoke(this, value, previouseValue);
    }

    public void AddModifier(string key, float value)
    {
        if (_modifyValueByKey.ContainsKey(key)) return;

        float prevValue = Value;

        _modifiedValue += value;
        _modifyValueByKey.Add(key, prevValue);

        TryInvokeValueChangeEvent(Value, prevValue);
    }
    
    public void RemoveModifier(string key)
    {
        if( _modifyValueByKey.TryGetValue(key, out float value))
        {
            float prevValue = Value;

            _modifiedValue -= value;
            _modifyValueByKey.Remove(key);

            TryInvokeValueChangeEvent(Value, prevValue);
        }
    }

    public void ClearModifiers()
    {
        float prevValue = Value;
        _modifyValueByKey.Clear();
        _modifiedValue = 0;
        TryInvokeValueChangeEvent(Value, prevValue);
    }

    public object Clone()
    {
        return Instantiate(this);
    }
}
