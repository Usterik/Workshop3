# АНАЛИЗ ДАННЫХ И ИСКУССТВЕННЫЙ ИНТЕЛЛЕКТ [in GameDev]
Отчет по лабораторной работе #3 выполнил:
- Устинов Эрик Константинович
- ФО-220007

Отметка о выполнении заданий (заполняется студентом):

| Задание | Выполнение | Баллы |
| ------ | ------ | ------ |
| Задание 1 | * | 60 |
| Задание 2 | * | 20 |
| Задание 3 | * | 20 |

знак "*" - задание выполнено; знак "#" - задание не выполнено;

Работу проверили:
- к.т.н., доцент Денисов Д.В.
- к.э.н., доцент Панов М.А.
- ст. преп., Фадеев В.О.

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

Структура отчета

- Данные о работе: название работы, фио, группа, выполненные задания.
- Цель работы.
- Задание 1.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Задание 2.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Задание 3.
- Код реализации выполнения задания. Визуализация результатов выполнения (если применимо).
- Выводы.
- ✨Magic ✨

## Цель работы
Разработать оптимальный баланс для десяти уровней игры Dragon Picker

## Задание 1
### Предложите вариант изменения найденных переменных для 10 уровней в игре. Визуализируйте изменение уровня сложности в таблице.

Единственный скрипт, предусматривающий переменные для баланса - EnemyDragon. Вот эти переменные:
- Скорость (дракона),
- Время между падением яиц,
- Минимальная дистанция до края,
- Шанс изменения движения.

Ссылка на таблицу с данными и их визуализацией https://docs.google.com/spreadsheets/d/1QYeL6PetjGICtcbMK6Q1grr1gF1ekiL6-fVtgmuNKO8/edit#gid=0

## Задание 2
### Создайте 10 сцен на Unity с изменяющимся уровнем сложности.

![image](https://github.com/Usterik/Workshop3/assets/149312199/37f42f8c-812d-4057-84bb-1c6256fdf362)


## Задание 3
### Решение в 80+ баллов должно заполнять google-таблицу данными из Python. В Python данные также должны быть визуализированы.

- Вот код, в котором хранятся значения для каждого уровня.

```py

import gspread
import numpy as np
from matplotlib import pyplot as plt 

gc = gspread.service_account(filename='workshop3-404306-6fa7ec08afa0.json')
sh = gc.open("Workshop3")

levels = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

def LoadInSheet(values, tableIndices):
    for i in range(len(values)):
        sh.sheet1.update(tableIndices[i], values[i])
        print(tableIndices[i], values[i])
            
def CreateGraph(array, title):
    plt.plot(levels, array)  
    plt.title(title)
    plt.show()

speed = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
speedTableIndices = ["A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3", "I3", "J3"]
LoadInSheet(speed, speedTableIndices)
CreateGraph(speed, "Dragon Speed")

timeBetweenEggDrops = [1, 0.95, 0.9, 0.85, 0.8, 0.75, 0.7, 0.65, 0.6, 0.55]
timeBetweenEggDropsTableIndices = ["L3", "M3", "N3", "O3", "P3", "Q3", "R3", "S3", "T3", "U3"]
LoadInSheet(timeBetweenEggDrops, timeBetweenEggDropsTableIndices)
CreateGraph(timeBetweenEggDrops, "Time between egg drop")

leftRightDistance = [10, 9.5, 9, 8.5, 8, 7.5, 7, 6.5, 6, 5.5]
leftRightDistanceTableIndices = ["W3", "X3", "Y3", "Z3", "AA3", "AB3", "AC3", "AD3", "AE3", "AF3"]
LoadInSheet(leftRightDistance, leftRightDistanceTableIndices)
CreateGraph(leftRightDistance, "Left Right distance")

chanceDirection = [0.1, 0.125, 0.15, 0.175, 0.2, 0.225, 0.25, 0.275, 0.3, 0.325]
chanceDirectionTableIndices = ["AH3", "AI3", "AJ3", "AK3", "AL3", "AM3", "AN3", "AO3", "AP3", "AQ3"]
LoadInSheet(chanceDirection, chanceDirectionTableIndices)
CreateGraph(chanceDirection, "Chance direction")

```

Программа, которая передаёт данные из таблицы в уровень:

```cs

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


```


## Выводы

Я изучил прототип игры Dragon Picker, выявил значения, пригодные для изменения уровня сложности, реализовал перенос данных из Python в Google Sheets и из Google Sheets в Unity, а также визуалировала эти данные.

| Plugin | README |
| ------ | ------ |
| Dropbox | [plugins/dropbox/README.md][PlDb] |
| GitHub | [plugins/github/README.md][PlGh] |
| Google Drive | [plugins/googledrive/README.md][PlGd] |
| OneDrive | [plugins/onedrive/README.md][PlOd] |
| Medium | [plugins/medium/README.md][PlMe] |
| Google Analytics | [plugins/googleanalytics/README.md][PlGa] |

## Powered by

**BigDigital Team: Denisov | Fadeev | Panov**
