using System.Collections;
using System.Collections.Generic;

namespace Misc
{
    public class ImmutableStack<T> : IEnumerable<T>
    {
        private readonly Stack<T> internalStack;

        public ImmutableStack()
        {
            internalStack = new Stack<T>();
        }

        public ImmutableStack(Stack<T> stack)
        {
            internalStack = stack;
        }

        public ImmutableStack<T> Push(T t)
        {
            var newStack = internalStack.Clone();
            newStack.Push(t);
            return new ImmutableStack<T>(newStack);
        }

        public ImmutableStack<T> Pop(out T t)
        {
            var newStack = internalStack.Clone();
            t = newStack.Pop();
            return new ImmutableStack<T>(newStack);
        }

        public T Peek()
        {
            return internalStack.Peek();
        }

        public int Count => internalStack.Count;

        public bool IsEmpty => internalStack.Count == 0;

        public IEnumerator<T> GetEnumerator()
        {
            return internalStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class StackExt
    {
        public static Stack<T> Clone<T>(this Stack<T> s)
        {
            return new Stack<T>(s.ToArray());
        }
    }
}