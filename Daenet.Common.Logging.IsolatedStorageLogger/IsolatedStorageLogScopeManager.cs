using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daenet.Common.Logging.IsolatedStorageLogger
{
    internal class IsolatedStorageLogScopeManager
    {
        internal static readonly AsyncLocal<List<DisposableScope>> m_AsyncSopes = new AsyncLocal<List<DisposableScope>>();
        private object m_State;

        public object Current
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in m_AsyncSopes.Value)
                {
                    sb.Append($"/{item}");
                }

                return sb.ToString();
            }
        }

        public IsolatedStorageLogScopeManager(object state)
        {
            m_AsyncSopes.Value = new List<DisposableScope>();
            m_State = state;
        }

        internal IDisposable Push(object state)
        {
            lock ("scope")
            {
                var newScope = new DisposableScope(state.ToString(), this);

                m_AsyncSopes.Value.Add(newScope);

                return newScope;
            }
        }
        public override string ToString()
        {
            return m_State?.ToString();
        }

        internal class DisposableScope : IDisposable
        {
            #region Member Variables
            private IsolatedStorageLogScopeManager m_ScopeMgr;
            private string m_ScopeName;
            #endregion

            #region Public Methods
            public DisposableScope(string scopeName, IsolatedStorageLogScopeManager scopeMgr)
            {
                this.m_ScopeMgr = scopeMgr;
                this.m_ScopeName = scopeName;
            }
            public void Dispose()
            {
                lock ("scope")
                {
                    var me = m_AsyncSopes.Value.FirstOrDefault(s => s == this);
                    if(me == null)
                    {
                        throw new InvalidOperationException("This sould never happen!");
                    }

                    m_AsyncSopes.Value.Remove(me);
                }
            }

            public override string ToString()
            {
                return m_ScopeName;
            }
            #endregion
        }
    }
}
