using System;
using System.Collections.Generic;


public class Node
{
    public int id;
    public string name;
    public int waitTime;
    public Node next;
    public DateTime EnqueuedAt;

    public Node(int id, string name, int waitTime)
    {
        this.id = id;
        this.name = name;
        this.waitTime = waitTime;
        this.next = null!;
        this.EnqueuedAt = DateTime.Now;
    }

    public int GetRemainingTime()
    {
        int elapsed = (int)(DateTime.Now - EnqueuedAt).TotalSeconds;
        int remaining = waitTime - elapsed;
        return remaining > 0 ? remaining : 0;
    }
}
