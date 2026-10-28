using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class SquadSettingsConfig : ScriptableObject
    {
        public List<ColorData> squadColors;

        public ColorData GetRandomColor()
        {
            return squadColors[UnityEngine.Random.Range(0, squadColors.Count)];
        }

        public ColorData GetRandomColorWithException(ColorType exceptionColorType)
        {
            List<ColorData> filteredColors = squadColors.FindAll(colorData => colorData.colorType != exceptionColorType);
            return filteredColors[UnityEngine.Random.Range(0, filteredColors.Count)];
        }
    }

    public enum ColorType
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    [Serializable]
    public class ColorData
    {
        public ColorType colorType;
        public Color color;
    }
}
