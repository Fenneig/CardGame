using System.Collections.Generic;
using CardGame.Visual;

namespace CardGame.Commands
{
    public class Command
    {
        public static Queue<Command> CommandQueue = new Queue<Command>();
        public static bool PlayingQueue = false;

        public virtual void AddToQueue()
        {
            CommandQueue.Enqueue(this);
            if (!PlayingQueue) PlayCommandFromQueue();
        }

        public virtual void StartCommandExecution()
        {
            /*
              Набор всех комманд которые нужно выполнить (взять карту, разыграть карту, разыграть эффект заклинания и т.д.) 
              есть 2 варианта распределить их по времени
              1) испольсовать последовательность твинов и вызывать CommandExecutionComplete в OnComplete()
              2) использовать курутины и WaitFor для предоставления задержек и вызывать CommandExecutionComplete в конце курутины
             */
        }

        public static void CommandExecutionComplete()
        {
            if (CommandQueue.Count > 0) PlayCommandFromQueue();
            else PlayingQueue = false;
        }

        private static void PlayCommandFromQueue()
        {
            PlayingQueue = true;
            CommandQueue.Dequeue().StartCommandExecution();
        }

        public static bool CardDrawPending()
        {
            foreach (var command in CommandQueue)
                if (command is DrawACardCommand)
                    return true;

            return false;
        }
    }
}