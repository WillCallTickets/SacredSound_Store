using System;

namespace Jobs
{
	/// <summary>
	/// Summary description for Job.
	/// </summary>
	abstract public class Job
	{
		public Guid JobId = Guid.NewGuid();
		
		abstract public bool DoJob();

		abstract public void CleanUp();
	}
}
