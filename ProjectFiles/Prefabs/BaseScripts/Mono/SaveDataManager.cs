using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class ScoreManager : MonoBehaviour
{
    private string savePath; // ���� � ����� ����������

    public int highScore; // ������� ������ ����� ������

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "scores.json"); // ������� ���� � ����� ����������

        LoadScores(); // ��������� ����������� ���� ������ ��� ������� ����
    }

    private void LoadScores()
    {
        // ���������, ���������� �� ���� ����������
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath); // ������ ���� ����������

            // ������������� ������ �� Json � ������� ������
            highScore = JsonUtility.FromJson<ScoreData>(json).highScore;
        }
        else
        {
            Debug.Log("Huy nahuy");
            highScore = 0; // ���� ���� ���������� �� ����������, ������ ����� ����� 0
        }
    }

    public void SaveScores()
    {
        if (highScore > 0) // ���������, ��� ���� ����� ������
        {
            // ������� ��������� ������ ��� ������������
            ScoreData data = new ScoreData();
            data.highScore = highScore;

            // ����������� ������ � Json ������
            string json = JsonUtility.ToJson(data);

            File.WriteAllText(savePath, json); // ���������� ������ � ���� ����������
        }
    }
    [Serializable]
    public class ScoreData
    {
        public int highScore;
    }
}



