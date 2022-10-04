using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [Header("Variables")]
    public int totalTasks;
    public int currentTask;
    public int totalCompleted;

    [Header("UI")]
    public GameObject tasksPanel;
    public GameObject taskPrefab;

    [Header("Manager")]
    public Task[] tasks;

    private Text[] taskContainer;

    private void Start()
    {
        ManageTasks();
        taskContainer = new Text[totalTasks];
        CreateTask();
    }

    public void Update()
    {
        ManageTasks();
        DisplayTasks();
    }
    private void ManageTasks()
    {
        totalTasks = tasks.Length;
        totalCompleted = 5;

        for (int i = 0; i < totalTasks; i++)
        {            
            if (!tasks[i].taskCompleted)
            {
                continue;
            }
            totalCompleted=i+1;
        }

        currentTask = totalCompleted + 1;
    }

    private void CreateTask()
    {
        RectTransform rectTransform = tasksPanel.GetComponent<RectTransform>();

        for(int i = 0; i < tasks.Length; i++)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.height + 50);

            GameObject task = Instantiate(taskPrefab);
            task.transform.SetParent(tasksPanel.transform);

            taskContainer[i] = task.GetComponent<Text>();
        }
    }

    private void DisplayTasks()
    {
        for(int i = 0; i < tasks.Length; i++)
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

            taskContainer[i].text = i + 1 + ". " + tasks[i].taskName + " : " + status;
        }
    }
}
