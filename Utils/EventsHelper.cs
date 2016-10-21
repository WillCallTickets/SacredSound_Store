using System;

namespace Utils
{

	/// <summary>
	/// Summary description for EventsHelper.
	/// </summary>
	public class EventsHelper
	{
		public EventsHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void FireEvent(Delegate del, params object[] args)
		{
			if(del == null)
				return;

			Delegate[] delegates = del.GetInvocationList();
			foreach(Delegate sink in delegates)
			{				
				try
				{
					sink.DynamicInvoke(args);
				}
				catch(Exception ex)
				{
                    throw ex;
				}
			}
		}
	}
}
