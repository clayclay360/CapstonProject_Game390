using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [Header("Variables")]
    public int maxTasks;
    public int totalTasks;
    public int currentTask;
    public int totalCompleted;

    [Header("UI")]
    public GameObject tasksPanel;
    public GameObject taskPrefab;

    [Header("Manager")]
    public Task[] tasks;

    private Dictionary<int,Text> taskContainer = new Dictionary<int, Text>();

    public void Update()
    {
        if (GameManager.gameStarted)
        {
            ManageTasks();
            DisplayTasks();
        }
    }
    private void ManageTasks()
    {
        totalCompleted = 0;

        for (int i = 0; i < totalTasks; i++)
        {
            if (!tasks[i].taskCompleted)
            {
                continue;
            }
            totalCompleted =i+1;
        }

        currentTask = totalCompleted + 1;
    }

    public void CreateTask()
    {
        RectTransform rectTransform = tasksPanel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.height + 50);
        GameObject task = Instantiate(taskPrefab, tasksPanel.transform);
        taskContainer.Add(totalTasks, task.GetComponent<Text>()); 
        totalTasks++;
    }

    private void DisplayTasks()
    {
        for (int i = 0; i < totalTasks; i++)
        {
            string status = "";

            if (tasks[i].taskCompleted)
            {
                status = "Completed";
            }
            else
            {
                status = "UnCompleted";
            }

            taskContainer[i].text = (i + 1) + ". " + tasks[i].taskName + " : " + status;
        }
    }
}
