using System.Collections.Generic;
using UnityEngine;

namespace _MAIN.Scripts.Core.Dialogue.Managers
{
    public class ConversationQueue
    {
        private Queue<Conversation> _conversationQueue = new();
        public Conversation Top => _conversationQueue.Peek();

        public void Enqueue(Conversation conversation) => _conversationQueue.Enqueue(conversation);

        public void EnqueuePriority(Conversation conversation)
        {
            var queue = new Queue<Conversation>();
            queue.Enqueue(conversation);

            while (_conversationQueue.Count > 0)
                queue.Enqueue(_conversationQueue.Dequeue());

            _conversationQueue = queue;
        }

        public void Dequeue()
        {
            if (_conversationQueue.Count > 0)
                _conversationQueue.Dequeue();
        }

        public bool IsEmpty() => _conversationQueue.Count == 0;
        
        public void Clear() => _conversationQueue.Clear();
    }
}
