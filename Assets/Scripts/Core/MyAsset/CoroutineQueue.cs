using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CoroutineQueue {
	public Action<IEnumerator> NotifyOnJobStarted;
	public Action<IEnumerator> NotifyOnJobComplete;
	public Action NotifyOnQueueComplete;

	private int _concurrent = 0;
	public int concurrent{
		get{ 
			return _concurrent;
		}
		set{
			_concurrent = value;
			CheckAndAdd();
		}
	}

	private Queue<IEnumerator> coroutines = new Queue<IEnumerator>();
	private List<CM_Job> jobs = new List<CM_Job>();

	private bool _isRunning;
	public bool isRunning{
		get{ 
			return _isRunning;
		}
		private set{ 
			_isRunning = value;
		}
	}

	public void Stop(){
		if(isRunning){
			isRunning = false;
			if(jobs.Count > 0){
				for(int i = 0;i < jobs.Count;i++){
					jobs [i].Pause ();
				}
			}
		}
	}
		
	public void Start(){
		if (!isRunning) {
			isRunning = true;
			if (jobs.Count > 0) {
				for (int i = 0; i < jobs.Count; i++) {
					if (!jobs [i].running) {
						jobs [i].Start ();
					}
					if (jobs [i].paused) {
						jobs [i].Resume ();
					}
				}
			}
		}
	}

	public virtual void AddJob(IEnumerator coroutine){
		coroutines.Enqueue (coroutine);
		CheckAndAdd ();
	}
		
	private void CheckAndAdd(){
		if(jobs.Count < concurrent){
			int num = concurrent - jobs.Count;
			for(int i = 0;i < num;i++){
				if (coroutines.Count > 0) {
					IEnumerator coroutine = coroutines.Dequeue ();
					CM_Job job = CM_Job.Make(coroutine).NotifyOnJobComplete(notifyOnJobComplete);
					if(isRunning){
						job.Start ();
					}
					jobs.Add (job);
					if(NotifyOnJobStarted != null){
						NotifyOnJobStarted (coroutine);
					}
				} else {
					break;
				}
			}
		}
	}

	private void notifyOnJobComplete(object sender,CM_JobEventArgs args){
		if(NotifyOnJobComplete != null){
			NotifyOnJobComplete (args.job.coroutine);
		}
		jobs.Remove (args.job);
		args.job.RemoveNotifyOnJobComplete (notifyOnJobComplete);
		args.job.Kill ();

		CheckAndAdd ();

		if(jobs.Count == 0 && coroutines.Count == 0){
			if(NotifyOnQueueComplete != null){
				NotifyOnQueueComplete ();
			}
			Stop ();
		}
	}
}

