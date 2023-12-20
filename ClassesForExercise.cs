using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassesForExercise
{
    public class Battery
    {
        const int MAX_CAPACITY = 1000;
        private static Random r = new Random();
        //Add events to the class to notify upon threshhold reached and shut down!
        #region events
        #endregion

        public delegate void ProgressEventHandler(object sender, int percent);
        public event ProgressEventHandler ReachThreshold;

        public delegate void TaskDoneEventHandler(object sender);
        public event TaskDoneEventHandler ShutDown;

        private int Threshold { get; }
        public int Capacity { get; set; }
        public int Percent
        {
            get
            {
                return 100 * Capacity / MAX_CAPACITY;
            }
        }
        public Battery()
        {
            Capacity = MAX_CAPACITY;
            Threshold = 400;
        }

        public void Usage()
        {
            Capacity -= r.Next(50, 150);
            //Add calls to the events based on the capacity and threshhold
            #region Fire Events
            #endregion;

            if (Capacity <= 0)
            {
                if (ShutDown != null)
                {
                    ShutDown(this);
                }
            }
            else if (Capacity <= Threshold)
            {
                if (ReachThreshold != null)
                {
                    ReachThreshold(this, 100 * Capacity / MAX_CAPACITY);
                }
            }
        }

    }

    class ElectricCar
    {

        static void Main(string[] args)
        {
            //ElectricCar c = new ElectricCar(1);
            //c.StartEngine();

            Console.WriteLine("Start! Thread:" + Thread.CurrentThread.ManagedThreadId);
            Task[] tasksArr = new Task[20];
            ElectricCar[] ecarsArr = new ElectricCar[20];

            for (int i = 0; i < ecarsArr.Length; i++)
            {
                ecarsArr[i] = new ElectricCar(i);
                tasksArr[i] = Task.Run(ecarsArr[i].StartEngine);
            }

            Task d = Task.WhenAll(tasksArr);
            d.Wait();
            Console.WriteLine("All Cars are shut down! " +Thread.CurrentThread.ManagedThreadId);
        }

        public Battery Bat { get; set; }
        private int id;

        //Add event to notify when the car is shut down
        public event Action OnCarShutDown;
        

        public ElectricCar(int id)
        {
            this.id = id;
            Bat = new Battery();
            #region Register to battery events
            #endregion
            Bat.ReachThreshold += Progress;
            Bat.ShutDown += BatteryFinished;
            OnCarShutDown += CarShutDown;
        }

        private void Bat_ShutDown(object sender)
        {
            throw new NotImplementedException();
        }

        public void StartEngine()
        {
            while (Bat.Capacity > 0)
            {
                Console.WriteLine($"{this} {Bat.Percent}% Thread: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Bat.Usage();
            }
        }

        //Add code to Define and implement the battery event implementations
        #region events implementation
        #endregion

        public static void Progress(object sender, int percent)
        {
            if (sender is Battery)
            {
                Battery battery = (Battery)sender;
                Console.WriteLine("Warning ! The car has low battery, please charge - battery is " + percent + "%");
            }
        }

        public static void BatteryFinished(object sender)
        {
            if (sender is Battery)
            {
                Battery battery = (Battery)sender;
                Console.WriteLine("battery is dead !!");
            }
        }

        public void CarShutDown()
        {
            if (this.Bat.Capacity <= 0)
            {
                Console.WriteLine("The car is shut down !!");
            }
        }

        public override string ToString()
        {
            return $"Car: {id}";
        }

    }

}
