using System;
using System.IO;

namespace Wcss
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class _Depender
	{
		public _Depender()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void UpdateDependency(string fileName)
		{
			try
			{
				FileStream fs = null;
				StreamWriter sw = null;

				lock(typeof(_Depender))
				{
					fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
					sw = new StreamWriter(fs);
					sw.WriteLine(DateTime.Now.ToString());					
				}

				if (sw != null) sw.Close();
				if (fs != null) fs.Close();
			}
			catch(Exception ex)
			{
				_Error.LogException(ex);
				
				System.Diagnostics.Debug.WriteLine( ex.Message );
				System.Diagnostics.Debug.WriteLine( ex.StackTrace );
			}
		}
	}
}
