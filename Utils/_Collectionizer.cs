using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Linq;

namespace Utils
{
    

    public class _Collectionizer
    {
        public interface IOrderable<T>
        {
            bool DeleteFromCollection(int idx);
            T ReorderItem(int idx, string direction);
            T AddToCollection(List<Pair> argList);
            T InsertAtOrdinalInExistingCollection(int idx, int newOrdinalIndex);
        }

        #region ADD
        
        public static T AddItemToOrderedCollection<T>(List<T> list, List<Pair> args) where T : new()
        {
            return AddItemToOrderedCollection<T>(list, args, false);
        }
        public static T AddItemToOrderedCollection<T>(List<T> list, List<Pair> args, bool addAtStart) where T : new()
        {
            return AddItemToOrderedCollection<T>(list, args, addAtStart, "IDisplayOrder");
        }
        private static T AddItemToOrderedCollection<T>(List<T> list, List<Pair> args, bool addAtStart, string ordinalColumnName) where T : new()
        {
            T newItem = new T();
            
            if(list.Count > 1)
                list.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));

            if (addAtStart)
            {
                //move all items up one and then stick at start
                IncrementHigherOrderItems(list, default(T), ordinalColumnName);
                Utils.Reflector.AssignToExpression(newItem, ordinalColumnName, 0);
            }
            else
            {
                //get max value of idisplayorder and add one
                T lastItem = GetObjectInSequenceAtOrdinal(list, list.Count - 1, ordinalColumnName);
                
                int highestOrdinal = (lastItem != null) ? (int)Utils.Reflector.EvaluateExpression(lastItem, ordinalColumnName) : -1;

                Utils.Reflector.AssignToExpression(newItem, ordinalColumnName, highestOrdinal + 1);
            }

            foreach (Pair p in args)
                Utils.Reflector.AssignToExpression(newItem, p.First.ToString(), p.Second);

            SaveObject(newItem);
            list.Add(newItem);

            return newItem;
        }

        #endregion

        #region DELETE

        /// <summary>
        /// A return value of true indicates that the collection has been correctly updated
        /// </summary>
        public static bool DeleteFromOrderedCollection<T>(List<T> list, int idx)
        {
            return DeleteFromOrderedCollection<T>(list, idx, "IDisplayOrder");
        }
        private static bool DeleteFromOrderedCollection<T>(List<T> list, int idx, string ordinalColumnName) 
        {
            //ensure the object is in the collection
            T existingObject = list.Find(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, "Id") == idx); } );

            if (existingObject != null)
            {
                list.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));

                int existingIndex = list.FindIndex(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, "Id") == idx); });

                //the order of deletion is important here
                //we must first delete the object before reassigning display order values to the remaining elements
                Type b = existingObject.GetType().BaseType;
                b.InvokeMember("Delete", System.Reflection.BindingFlags.InvokeMethod,
                   null, null, new object[] { idx });

                //remove the existing object from the collection //this.Remove(entity);
                list.Remove(existingObject);

                //decrement higher order elements only if the element is not the last element                
                DecrementHigherOrderItems(list, existingObject, ordinalColumnName);

                return true;
            }
            
            return false;
        }

        #endregion

        #region REORDERING

        private static List<T> GetHigherOrderedItems<T>(List<T> list, T baseItem, string ordinalColumnName)
        {
            int baseOrdinal = (baseItem != null) ? (int)Utils.Reflector.EvaluateExpression(baseItem, ordinalColumnName) : -1;

            return list.FindAll(delegate(T match) { return (int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) > baseOrdinal; });
        }

        private static void DecrementHigherOrderItems<T>(List<T> list, T baseItem, string ordinalColumnName)
        {
            //get a collection of higher order items
            List<T> higherOrdered = GetHigherOrderedItems(list, baseItem, ordinalColumnName);

            //sort ascending to allow for db constraint
            if (higherOrdered.Count > 1)
                higherOrdered.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));

            //decrement the higher order items
            foreach (T listItem in higherOrdered)
            {
                int newOrdinal = (int)Utils.Reflector.EvaluateExpression(listItem, ordinalColumnName) - 1;
                SaveValue(listItem, ordinalColumnName, newOrdinal);
            }
        }
        private static void IncrementHigherOrderItems<T>(List<T> list, T baseItem, string ordinalColumnName)
        {
            //get a collection of higher order items
            List<T> higherOrdered = GetHigherOrderedItems(list, baseItem, ordinalColumnName);

            //sort descending to allow for db constraint
            if (higherOrdered.Count > 1)
                higherOrdered.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Descending, ordinalColumnName));

            //decrement the higher order items
            foreach (T listItem in higherOrdered)
            {
                int newOrdinal = (int)Utils.Reflector.EvaluateExpression(listItem, ordinalColumnName) + 1;
                SaveValue(listItem, ordinalColumnName, newOrdinal);
            }
        }

        public static T ReorderOrderedCollection<T>(List<T> list, int idx, string direction)
        {
            return ReorderOrderedCollection<T>(list, idx, direction, "IDisplayOrder");
        }
        private static T ReorderOrderedCollection<T>(List<T> list, int idx, string direction, string ordinalColumnName)
        {
            list.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));

            int existingIndex = list.FindIndex(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, "Id") == idx); });

            if (existingIndex != -1)
            {
                T existingObject = list[existingIndex];

                int existingValue = (int)Utils.Reflector.EvaluateExpression(existingObject, ordinalColumnName);

                //can't move up from zero
                if (direction.ToLower().Equals("up") && existingIndex > 0)
                {
                    T previousObject = list[existingIndex - 1];

                    if (previousObject != null)
                    {
                        int swap = (int)Utils.Reflector.EvaluateExpression(previousObject, ordinalColumnName);

                        SaveValue(existingObject, ordinalColumnName, -1);
                        SaveValue(previousObject, ordinalColumnName, existingValue);
                        SaveValue(existingObject, ordinalColumnName, swap);
                    }
                }
                else if (direction.ToLower().Equals("down") && existingIndex != (list.Count - 1))//count is 1 based
                {
                    T nextObject = list[existingIndex - 1];                    

                    if (nextObject != null)
                    {
                        int swap = (int)Utils.Reflector.EvaluateExpression(nextObject, ordinalColumnName);

                        SaveValue(existingObject, ordinalColumnName, -1);
                        SaveValue(nextObject, ordinalColumnName, existingValue);
                        SaveValue(existingObject, ordinalColumnName, swap);
                    }
                }

                return existingObject;
            }

            return default(T);
        }

        private static T GetObjectInSequenceAtOrdinal<T>(List<T> list, int ordinal, string ordinalColumnName)
        {
            if (list.Count == 0)
                return default(T);

            if(list.Count > 1)
                list.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));                

            //if the ordinal is zero - get the zero element
            T last = list.Skip(ordinal).FirstOrDefault();

            return last;
        }

        /// <summary>
        /// given the zero-based index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="idx"></param>
        /// <param name="newOrdinalIndex"></param>
        /// <param name="ordinalColumnName"></param>
        /// <returns></returns>
        public static T InsertAtOrdinalInExistingCollection<T>(List<T> list, int idx, int newOrdinalIndex)
        {
            return InsertAtOrdinalInExistingCollection<T>(list, idx, newOrdinalIndex, "IDisplayOrder");        
        }
        private static T InsertAtOrdinalInExistingCollection<T>(List<T> list, int idx, int newOrdinalIndex, string ordinalColumnName)
        {
            try
            {
                if (newOrdinalIndex > -1)
                {
                    //ensure the object is in the collection
                    list.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));

                    int moveIndex = list.FindIndex(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, "Id") == idx); ; });
                    T moveObject = list[moveIndex];

                    //if we are moving the item up in the list
                    //get the subset of items in between the original and new ordinal
                    //increment items up by one - but do in reverse order
                    if (moveIndex > newOrdinalIndex)//moving up
                    {
                        int previousOrdinal = -1;
                        int nextOrdinal = int.MaxValue;

                        //determine low end cutoff
                        if (newOrdinalIndex > 0)
                        {
                            T previous = list[newOrdinalIndex - 1];
                            previousOrdinal = (int)Utils.Reflector.EvaluateExpression(previous, ordinalColumnName);
                        }

                        //determine high end cutoff
                        if (moveIndex < list.Count - 1)//if the moving object is not last
                        {
                            T next = list[moveIndex + 1];
                            nextOrdinal = (int)Utils.Reflector.EvaluateExpression(next, ordinalColumnName);
                        }
                        

                        //remove move from list and save off moveObject so it doesnt interefere in db constraint
                        list.RemoveAt(moveIndex);
                        SaveValue(moveObject, ordinalColumnName, -10000);

                        List<T> subset = new List<T>(list.FindAll(delegate(T match)
                        {
                            return ((int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) > previousOrdinal && 
                                (int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) < nextOrdinal);
                        }));

                        if (subset.Count > 1)
                            subset.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Descending, ordinalColumnName));

                        //increment the subset
                        foreach (T listItem in subset)
                            SaveValue(listItem, ordinalColumnName, (int)Utils.Reflector.EvaluateExpression(listItem, ordinalColumnName) + 1);

                        //resave the object that was moving and reinsert into list at the new index
                        SaveValue(moveObject, ordinalColumnName, previousOrdinal + 1);
                        list.Insert(newOrdinalIndex, moveObject);
                    }
                    else //moving down
                    {
                        T existing = list[newOrdinalIndex];
                        int existingIndex = (int)Utils.Reflector.EvaluateExpression(existing, ordinalColumnName);

                        //remove move from list and save off moveObject so it doesnt interefere in db constraint
                        list.RemoveAt(moveIndex);
                        SaveValue(moveObject, ordinalColumnName, -10000);

                        List<T> subset = new List<T>(list.FindAll(delegate(T match)
                        {
                            return (int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) <= existingIndex &&
                                (int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) > moveIndex;
                        }));

                        if (subset.Count > 1)
                            subset.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));

                        //decrement the higher order items
                        foreach (T listItem in subset)
                            SaveValue(listItem, ordinalColumnName, (int)Utils.Reflector.EvaluateExpression(listItem, ordinalColumnName) - 1);

                        //resave the object that was moving and reinsert into list at the new index
                        SaveValue(moveObject, ordinalColumnName, existingIndex);
                        list.Insert(newOrdinalIndex, moveObject);

                    }

                    return moveObject;
                }
            }
            catch (Exception ex)
            {
                string g = ex.Message;
            }
                
            return default(T);
        }

        #endregion

        #region SAVE

        private static void SaveValue(object existing, string columnName, int value)
        {
            Utils.Reflector.AssignToExpression(existing, columnName, value);
            SaveObject(existing);
        }
        private static void SaveObject(object item)
        {
            item.GetType().InvokeMember("Save", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
        }

        #endregion
    }
}
