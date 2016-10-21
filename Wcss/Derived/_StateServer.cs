using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WillCallSubSonic.Derived
{
    public partial class _StateServer
    {
        private static ReaderWriterLockSlim _slimLock = new ReaderWriterLockSlim();

        private object _mainCollection = null;
        public object MainCollection
        {
            get
            {
                try
                {
                    _slimLock.EnterUpgradeableReadLock();
                    if (_mainCollection == null)
                    {
                        _slimLock.EnterWriteLock();

                        _mainCollection = HydrateMain();//create and hydrate the main collection

                        _subCollectionA = HydrateSubA();
                        _subCollectionB = HydrateSubB();
                    }

                    return _mainCollection;
                }
                finally
                {
                    if (_slimLock.IsWriteLockHeld)
                        _slimLock.ExitWriteLock();
                    if (_slimLock.IsUpgradeableReadLockHeld)
                        _slimLock.ExitUpgradeableReadLock();
                    if (_slimLock.IsReadLockHeld)
                        _slimLock.ExitReadLock();
                }
            }
        }

        private object _subCollectionA = null;
        public object SubCollectionA
        {
            get
            {
                object wait = MainCollection;

                return _subCollectionA;
            }
        }
        private object _subCollectionB = null;
        public object SubCollectionB
        {
            get
            {
                object wait = MainCollection;

                return _subCollectionB;
            }
        }

        private object HydrateMain()
        {
            return new object();
        }

        private object HydrateSubA()
        {
            //loop thru main collection and construct sub collection
            return new object();
        }

        private object HydrateSubB()
        {
            //loop thru main collection and construct sub collection
            return new object();
        }
    }
}

