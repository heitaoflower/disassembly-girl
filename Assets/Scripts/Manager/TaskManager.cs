using System.Collections;

using Utils;

namespace Manager
{
    public class Task
    {
        public bool Running
        {
            get { return task.Running; }
        }

        public bool Paused
        {
            get { return task.Paused; }
        }

        public delegate void FinishedHandler(bool manual);

        public event FinishedHandler Finished = null;

        public Task(IEnumerator c, bool autoStart = true)
        {
            task = TaskManager.CreateTask(c);
            task.Finished += TaskFinished;

            if (autoStart)
            {
                Start();
            }
        }

        public void Start()
        {
            task.Start();
        }

        public void Stop()
        {
            task.Stop();
        }

        public void Pause()
        {
            task.Pause();
        }

        public void Unpause()
        {
            task.Unpause();
        }

        void TaskFinished(bool manual)
        {
            FinishedHandler handler = Finished;

            if (handler != null)
            {
                handler(manual);
            }
        }

        TaskManager.TaskState task = null;
    }

    public class TaskManager : Singleton<TaskManager>
    {
        public class TaskState
        {
            public bool Running
            {
                get { return running; }
            }

            public bool Paused
            {
                get { return paused; }
            }

            public delegate void FinishedHandler(bool manual);

            public event FinishedHandler Finished = null;

            IEnumerator coroutine = null;
            bool running = default(bool);
            bool paused = default(bool);
            bool stopped = default(bool);

            public TaskState(IEnumerator c)
            {
                coroutine = c;
            }

            public void Pause()
            {
                paused = true;
            }

            public void Unpause()
            {
                paused = false;
            }

            public void Start()
            {
                running = true;

                GetInstance().StartCoroutine(CallWrapper());
            }

            public void Stop()
            {
                stopped = true;
                running = false;
            }

            IEnumerator CallWrapper()
            {
                yield return null;

                IEnumerator e = coroutine;

                while (running)
                {
                    if (paused)
                    {
                        yield return null;
                    }
                    else
                    {
                        if (e != null && e.MoveNext())
                        {
                            yield return e.Current;
                        }
                        else
                        {
                            running = false;
                        }
                    }
                }

                FinishedHandler handler = Finished;

                if (handler != null)
                {
                    handler(stopped);
                }
            }
        }

        public static TaskState CreateTask(IEnumerator coroutine)
        {
            return new TaskState(coroutine);
        }

        public override void Initialize()
        {
            base.Initialize();   
        }

        public override void Release()
        {
            base.Release();        
        }
    }
}
