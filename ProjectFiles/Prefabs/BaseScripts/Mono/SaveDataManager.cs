using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class ScoreManager : MonoBehaviour
{
    private string savePath; // Путь к файлу сохранения

    public int highScore; // Текущий рекорд очков игрока

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "scores.json"); // Создаем путь к файлу сохранения

        LoadScores(); // Загружаем сохраненные очки игрока при запуске игры
    }

    private void LoadScores()
    {
        // Проверяем, существует ли файл сохранения
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath); // Читаем файл сохранения

            // Десериализуем данные из Json в текущий рекорд
            highScore = JsonUtility.FromJson<ScoreData>(json).highScore;
        }
        else
        {
            Debug.Log("Huy nahuy");
            highScore = 0; // Если файл сохранения не существует, рекорд будет равен 0
        }
    }

    public void SaveScores()
    {
        if (highScore > 0) // Проверяем, что есть новый рекорд
        {
            // Создаем экземпляр класса для сериализации
            ScoreData data = new ScoreData();
            data.highScore = highScore;

            // Сериализуем данные в Json формат
            string json = JsonUtility.ToJson(data);

            File.WriteAllText(savePath, json); // Записываем данные в файл сохранения
        }
    }
    [Serializable]
    public class ScoreData
    {
        public int highScore;
    }
}



