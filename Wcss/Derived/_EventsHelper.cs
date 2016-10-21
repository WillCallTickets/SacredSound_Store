using System;

namespace Wcss
{

	/// <summary>
	/// Summary description for EventsHelper.
	/// </summary>
	public class _EventsHelper
	{
		public _EventsHelper()
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
					_Error.LogException(ex);
				}
			}
		}
	}
}
