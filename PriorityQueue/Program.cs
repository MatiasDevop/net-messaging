using System.Diagnostics;

var cts = new CancellationTokenSource();

var normalQueue = new Queue<string>();
var goldQueue = new Queue<string>();
var platinumQueue = new Queue<string>();

//Normal process
//normalQueue.Enqueue("Mike");
//platinumQueue.Enqueue("Nick");
//goldQueue.Enqueue("Kim");
//goldQueue.Enqueue("Brooke");
//platinumQueue.Enqueue("Jhon");
//normalQueue.Enqueue("Poke");

//while (!cts.IsCancellationRequested)
//{
//    await Task.Delay(1000);
//	if (platinumQueue.Count > 0)
//	{
//		var platinumItem = platinumQueue.Dequeue();
//		Console.WriteLine($"Platinium: {platinumItem}");
//		continue;
//	}

//    if (goldQueue.Count > 0)
//    {
//        var goldItem = goldQueue.Dequeue();
//        Console.WriteLine($"Platinium: {goldItem}");
//        continue;
//    }

//    if (normalQueue.Count > 0)
//    {
//        var normalItem = normalQueue.Dequeue();
//        Console.WriteLine($"Platinium: {normalItem}");
//        continue;
//    }
//}

//Do this instead to prioritize queues
//var queue = new PriorityQueue<string, int>();
var queue = new PriorityQueue<string, (Status, long)>(StatusComparer.Instance);

queue.Enqueue("Mike", (Status.Normal, Stopwatch.GetTimestamp()));
queue.Enqueue("Nick", (Status.Platinum, Stopwatch.GetTimestamp()));
queue.Enqueue("Kim", (Status.Gold, Stopwatch.GetTimestamp()));
queue.Enqueue("Brooke", (Status.Gold, Stopwatch.GetTimestamp()));
queue.Enqueue("Jhon", (Status.Platinum, Stopwatch.GetTimestamp()));
queue.Enqueue("Poke", (Status.Normal, Stopwatch.GetTimestamp()));

while (!cts.IsCancellationRequested)
{
	await Task.Delay(1000);
	if (queue.Count > 0)
	{
		var item = queue.Dequeue();
		Console.WriteLine($": {item}");
	}
}

enum Status
{
	Normal,
	Gold,
	Platinum,
}
record User(string Name);

class StatusComparer : IComparer<(Status, long)>
{
	//optimize this
	public static StatusComparer Instance { get; } = new();
	private StatusComparer() { }
    public int Compare((Status, long) x, (Status, long) y)
    {
		if (x.Item1 == y.Item1)
		{
			return x.CompareTo(y);
		}
		return y.CompareTo(x);
    }
}
