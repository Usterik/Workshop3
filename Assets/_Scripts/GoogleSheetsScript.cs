using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class GoogleSheetsScript : MonoBehaviour
{
    public bool IsInLevel;

    public EnemyDragon Dragon;

    private static int levelNumber;

    private float[] speedValues = new float[10];
    private float[] timeBetweenEggDropsValues = new float[10];
    private float[] leftRightDistanceValues = new float[10];
    private float[] chanceDirectionValues = new float[10];

    void Start()
    {
        StartCoroutine(GoogleSheets());
    }

    IEnumerator GoogleSheets()
    {
        UnityWebRequest curentResp = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1QYeL6PetjGICtcbMK6Q1grr1gF1ekiL6-fVtgmuNKO8/values/BigDigital GameLab?key=AIzaSyAvxYBlRZvQMHgMSC1VtTLLUhGM268NIy4");
        yield return curentResp.SendWebRequest();
        string rawResp = curentResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);
        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;

            if (selectRow.Count == 43)
                for(int i = 0; i < 10; i++)
                {
                    speedValues[i] = float.Parse(selectRow[i]);
                    timeBetweenEggDropsValues[i] = float.Parse(selectRow[i + 11]);
                    leftRightDistanceValues[i] = float.Parse(selectRow[i + 22]);
                    chanceDirectionValues[i] = float.Parse(selectRow[i + 33]);
                }
        }

        if (IsInLevel)
        {
            Dragon.speed = speedValues[levelNumber];
            Dragon.timeBetweenEggDrops = timeBetweenEggDropsValues[levelNumber];
            Dragon.leftRightDistance = leftRightDistanceValues[levelNumber];
            Dragon.chanceDirection = chanceDirectionValues[levelNumber];
        }
    }

    public void SetLevelNumber(int number)
    {
        levelNumber = number;
    }
}
