
Task.Factory.StartNew(WhatTypeOfThreadAmI).Wait();

Task.Delay(500);

Task.Factory.StartNew(WhatTypeOfThreadAmI,TaskCreationOptions.LongRunning).Wait();

static void WhatTypeOfThreadAmI()
{
  Console.WriteLine("I'm a {0} thread" ,
                    Thread.CurrentThread.IsThreadPoolThread ? "Thread Pool" : "Custom");
}