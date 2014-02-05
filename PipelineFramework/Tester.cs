using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pipeline
{
    public interface IMyTest
    {
        void ExecuteAction();
    }

    public class MyList<T> : List<T>
    {
        public T Channel { get; set; } // probably won't work

        public void Execute(Action<T> executeAction)
        {
            this.ForEach(item => { executeAction.Invoke(item); });
        }
    }
    
    public class Tester
    {
        private void MethodName()
        {
            MyList<IMyTest> obj = new MyList<IMyTest>();
        }
    }
}
