using TMPro;
using UnityEngine;

public class Stats : Singletone<Stats>
{
    [SerializeField] TMP_Text _buildingCountText;

    public static int BuildingCount = 2;

    public static void AddBuilding(int i)
    {
        BuildingCount += i;
        print("added to building count" + 2);
    }

    public void UpdateText()
    {
        _buildingCountText.text = BuildingCount.ToString();
    }
}
