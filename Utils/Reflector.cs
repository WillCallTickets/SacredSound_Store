using System;
using System.Reflection;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Summary description for Reflector.
	/// </summary>
	public class Reflector
	{
		private Reflector(){}

        public enum Direction
        {
            Ascending = 0,
            Descending = 1,
        }

        //http://pavangayakwad.blogspot.com/
        public class CompareEntities<T> : IComparer<T>
        {
            private Direction _sortDirection;
            public Direction SortDirection
            {
                get { return _sortDirection; }
                set { _sortDirection = value; }
            }

            private string _propertyName;
            public string PropertyName
            {
                get { return _propertyName; }
                set { _propertyName = value; }
            }

            public CompareEntities( Direction dir, string fieldName)
            {
                SortDirection = dir;
                PropertyName = fieldName;
            }

            #region IComparer<T> Members

            public int Compare(T x, T y)
            {
                if (typeof(T).GetProperty(PropertyName) == null)
                    throw new Exception(String.Format("Given property is not part of the type {0}", PropertyName));

                object objX = typeof(T).GetProperty(PropertyName).GetValue(x, null);
                object objY = typeof(T).GetProperty(PropertyName).GetValue(y, null);

                int retVal = default(int);
                if(SortDirection == Direction.Ascending)
                    retVal = ((IComparable)objX).CompareTo((IComparable)objY);
                else if (SortDirection == Direction.Descending)
                    retVal = ((IComparable)objX).CompareTo((IComparable)objY) * -1;

                return retVal;
            }

            #endregion
        }

        //public class CompareEntitiesByPropertyIndex<T> : IComparer<T>
        //{
        //    CompareEntities<T> _comparer = null;
        //    public CompareEntitiesByPropertyIndex(Direction dir, int index)
        //    {
        //        _comparer = new CompareEntities<T>(dir,
        //            typeof(T).GetProperties()[index].Name);
        //    }

        //    #region IComparer<T> Members
        //    public int Compare(T x, T y)
        //    {
        //        return _comparer.Compare(x, y);
        //    }
        //    #endregion
        //}

//        public static void ChangeDisplayOrder(object currentObject, bool moveUp)
//        {
////			int currentOrder = (int)EvaluateExpression(currentObject, "DisplayOrder");
////
////			type objType = currentObject.GetType();
////			type colType = coll.GetType();
////
////			
////
////			foreach(object obj in coll)
////			{
////				if(obj == currentObject)
////				{					
////					//EvaluateExpression(obj, "DisplayOrder") = (moveUp) ? currentOrder + 1 : currentOrder - 1;
////				}
////
////			}
//        }

		/// <summary>
		/// This is an internal method and should not be used.
		/// </summary>
		/// <remarks>
		/// Constructs a an object of the given type.
		/// </remarks>
		/// <param name="t">Type to construct</param>
		/// <returns>A new object of the given type</returns> 
		public static object ConstructNoParams( System.Type t)
		{
			return t.GetConstructor( new Type[0] ).Invoke( new object[0] );
		}

		public static object EvaluateExpression(object objToEvaluate, string expression)
		{
			if (RequiresTraversal(expression))
			{
				string nextPropertyToEval = ParseNextExpression( ref expression );
				object nextObject = EvaluateExpression(objToEvaluate, nextPropertyToEval);
				return EvaluateExpression( nextObject, expression);
			}
			else if (expression.Length > 0)
			{
				PropertyInfo pi = objToEvaluate.GetType().GetProperty(expression);
				if (pi != null)
					return pi.GetValue(objToEvaluate, null);
				else
				{
					FieldInfo fi = objToEvaluate.GetType().GetField(expression);
					return fi.GetValue(objToEvaluate);
				}
			}
			else
			{
				return objToEvaluate;
			}
		}
        public static void AssignToExpression(object objToEvaluate, string expression, object value)
        {
            if (expression.Length > 0)
            {
                PropertyInfo pi = objToEvaluate.GetType().GetProperty(expression);
                if (pi != null)
                {
                    pi.SetValue(objToEvaluate, value, null);
                }
                else
                {
                    FieldInfo fi = objToEvaluate.GetType().GetField(expression);
                    fi.SetValue(objToEvaluate, value);
                }
            }
        }

		private static bool	RequiresTraversal( string expression )
		{
			if (expression.IndexOf(".") > -1) return true;
			return false;
		}

		private	static string ParseNextExpression(ref string expression)
		{
			string nextExpr = expression.Substring(0, expression.IndexOf("."));
			expression = expression.Substring( expression.IndexOf(".") + 1);

			return nextExpr;
		}
	}
}
