// Don't place your source in a package
// Partially broken
import java.util.*;
import java.lang.*;
import java.io.*;
import java.math.*;

class Worker {
    // Set up the big int in the constructor
    public Worker()
    {
        JobTime = new BigInteger("0");
    }
    
    public void addJobTime(int power)
    {
        this.JobTime = this.JobTime.add((new BigInteger("1")).shiftLeft(power));
    }
    
    public BigInteger checkJobTime(int power)
    {
        return this.JobTime.add((new BigInteger("1")).shiftLeft(power));
    }
    
    public BigInteger currentJobTime()
    {
        return JobTime;
    }
    
    private BigInteger JobTime;
}

// Please name your class Main
class Main {
	public static void main (String[] args) throws java.lang.Exception {
	    Scanner in = new Scanner(System.in);
	    
	    // I hate Java scanners
	    // Get the first line - number of jobs, then number of workers
	    String firstLine = in.nextLine();
	    
	    String[] data = firstLine.split(" ", 0);
	    int jobs = Integer.parseInt(data[0]);
	    int workerCount = Integer.parseInt(data[1]);
	    
	    // Get the second line with all of the jobs that need to be queued
	    String secondLine = in.nextLine();
	    String[] jobTimesStrings = secondLine.split(" ", 0);
	    int[] jobTimes = new int[jobTimesStrings.length];
	    for (int i = 0; i < jobTimesStrings.length; i++)
	    {
	        jobTimes[i] = Integer.parseInt(jobTimesStrings[i]);
	    }
	    
	    // Create the specified number of workers
	    Worker[] workers = new Worker[workerCount];
	    // Allocate all of the workers
	    for (int i = 0; i < workers.length; i++)
	    {
	        workers[i] = new Worker();
	    }
	    
	    // Try and pack as many items as possible into each worker
	    int lastJobIndex = (jobs - 1);
	    
	    // Queue the biggest job to the first worker
	    workers[0].addJobTime(jobTimes[lastJobIndex]);
	    lastJobIndex--;
	    
	    // For now, we're comparing against the first one
	    int largestBenchmark = 0;
	    // Try and pack the jobs into the first available worker
	    int currentWorker = Math.min(1, workerCount - 1);
	    
	    // Loop until there aren't any jobs left
	    while (lastJobIndex >= 0)
	    {
	        // While the new time of the worker would be smaller than the current largest worker
	        while (lastJobIndex >= 0 && workers[currentWorker].checkJobTime(jobTimes[lastJobIndex]).compareTo(workers[largestBenchmark].currentJobTime()) == -1)
	        {
	            // Add it 
	            workers[currentWorker].addJobTime(jobTimes[lastJobIndex]);
	            lastJobIndex--;
	        }
	        
	        // Check to make sure there's more jobs to queue
	        if (lastJobIndex >= 0)
	        {
	            // Find the worker with the smallest load and add it to that one instead
	            // then set that as the largest benchmark
	            int currentSmallestIndex = 0;
	            BigInteger smallestTime = workers[currentSmallestIndex].currentJobTime();
	            for (int i = 0; i < workerCount; i++)
	            {
	                if (workers[i].currentJobTime().compareTo(smallestTime) == -1)
	                {
	                    smallestTime = workers[i].currentJobTime();
	                    currentSmallestIndex = i;
	                }
	            }
	            
	            // Add the next available job
	            workers[currentSmallestIndex].addJobTime(jobTimes[lastJobIndex]);
	            lastJobIndex--;
	            // It's the new largest one
	            largestBenchmark = currentSmallestIndex;
	        }
	    }
	    
	    // Largest one is largestBenchmark
	    BigInteger returnValue = (workers[largestBenchmark]).currentJobTime().remainder(new BigInteger("1000000007"));
	    
	    System.out.print(returnValue.toString());
	}
}