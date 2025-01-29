using System;
using System.Collections.Concurrent;
using System.Threading;

public class TaskProcessor
{
    private readonly BlockingCollection<Action> _taskQueue = new();
    private readonly Thread[] _workerThreads;
    private bool _isRunning = true;

    public TaskProcessor(int numberOfThreads)
    {
        if (numberOfThreads <= 0)
            throw new ArgumentException("Number of threads must be greater than zero.", nameof(numberOfThreads));

        _workerThreads = new Thread[numberOfThreads];

        for (int i = 0; i < numberOfThreads; i++)
        {
            _workerThreads[i] = new Thread(ProcessTasks)
            {
                IsBackground = true
            };
            _workerThreads[i].Start();
        }
    }

    public void EnqueueTask(Action task)
    {
        if (!_isRunning)
            throw new InvalidOperationException("TaskProcessor is no longer running.");

        _taskQueue.Add(task);
    }

    public void Stop()
    {
        _isRunning = false;
        _taskQueue.CompleteAdding();

        foreach (Thread thread in _workerThreads)
        {
            if (thread.IsAlive)
                thread.Join(); // Wait for all threads to finish
        }
    }

    private void ProcessTasks()
    {
        while (_isRunning || !_taskQueue.IsCompleted)
        {
            try
            {
                Action task = _taskQueue.Take(); // Blocks until a task is available
                task.Invoke();
            }
            catch (InvalidOperationException)
            {
                // Handle case where queue is empty and CompleteAdding is called
                break;
            }
        }
    }
}
