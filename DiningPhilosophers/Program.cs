using System;
using System.Threading;

class Constants{
    public static int timesEaten = 0;
}

public class DiningPhilosophersRunner{
	static void Main(){
		int NUM_PHILOSOPHERS = 5;

		Object[] chopsticks = new Object[NUM_PHILOSOPHERS];
		for(int i = 0; i < NUM_PHILOSOPHERS; i++){
			chopsticks[i] = new Object();
		}

		Philosopher[] philosophers = new Philosopher[NUM_PHILOSOPHERS];
		for(int i = 0; i < NUM_PHILOSOPHERS; i++){
			int left = (i+1) % NUM_PHILOSOPHERS > i ? i : (i+1) % NUM_PHILOSOPHERS;
			int right = left < i ? i : (i+1) % NUM_PHILOSOPHERS;

			// int left = i;
			// int right = (i+1) % NUM_PHILOSOPHERS;

			philosophers[i] = new Philosopher(chopsticks[left], chopsticks[right], i);
		}

		Thread[] threads = new Thread[NUM_PHILOSOPHERS];
		for(int i = 0; i < NUM_PHILOSOPHERS; i++){
			int xi = i;
			threads[i] = new Thread(() => philosophers[xi].Eat());
		}

		for(int i = 0; i < NUM_PHILOSOPHERS; i++){
			Console.WriteLine($"Starting Thread {i}");
			threads[i].Start();
		}

		for(int i = 0; i < NUM_PHILOSOPHERS; i++){
			threads[i].Join();
			Console.WriteLine($"Philosopher {i} ate {philosophers[i].selfEats} times.");
		}

	}
}

public class Philosopher{
	private const int MAX_EATING = 100;
	private readonly Object leftChopstick;
	private readonly Object rightChopstick;
	private readonly int id;
	public int selfEats = 0;

	public Philosopher(Object left, Object right, int id){
		leftChopstick = left;
		rightChopstick = right;
		this.id = id;
	}

	public void Eat(){
		while(Constants.timesEaten < MAX_EATING){
			lock(leftChopstick){
				lock(rightChopstick){
					Console.WriteLine($"{Constants.timesEaten}: Philosopher {id} is eating.");
					Interlocked.Increment(ref Constants.timesEaten);
					Think();
				}
			}
			selfEats++;
		}
	}

	public void Think(){

		Console.WriteLine($"Philosopher {id} is thinking.");

		Thread.Sleep(500);

	}
}
