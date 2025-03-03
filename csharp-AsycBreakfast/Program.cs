﻿using System.Diagnostics;

using Services;
using Models;

namespace AsyncBreakfast;
class Program
{
    static async Task Main(string[] args)
    {

        var theMethod = "同步方法";
        Console.WriteLine($"开始执行   {theMethod} ====================================================");
        var stopwatch = Stopwatch.StartNew();
        new MakeBreakfast().MakeBreakfastSync();
        Console.WriteLine($" {theMethod} 共耗费时间 : {stopwatch.Elapsed}{Environment.NewLine}");

        theMethod = "顺序的异步方法";
        Console.WriteLine($"开始  {theMethod} ====================================================");
        var stopwatch2 = Stopwatch.StartNew();
        await new MakeBreakfast().MakeBreakfastFackAsync();
        Console.WriteLine($" {theMethod} 共耗费时间 : {stopwatch2.Elapsed}{Environment.NewLine}");

        theMethod = "有规划的异步方法";
        Console.WriteLine($"开始执行 {theMethod} ====================================================");
        var stopwatch3 = Stopwatch.StartNew();
        await new MakeBreakfast().MakeBreakfastAsync();
        Console.WriteLine($"{theMethod}  共耗费时间 : {stopwatch3.Elapsed}{Environment.NewLine}");


        theMethod = "组合的异步方法";
        Console.WriteLine($"开始执行 {theMethod} ====================================================");
        var stopwatch4 = Stopwatch.StartNew();
        await new MakeBreakfast().MakeBreakfastGroupAsync();
        Console.WriteLine($"{theMethod}  共耗费时间 : {stopwatch4.Elapsed}{Environment.NewLine}");


        theMethod = "优化的异步方法";
        Console.WriteLine($"开始执行 {theMethod} ====================================================");
        var stopwatch5 = Stopwatch.StartNew();
        await new MakeBreakfast().MakeBreakfastOptimizationAsync();
        Console.WriteLine($"{theMethod}  共耗费时间 : {stopwatch5.Elapsed}{Environment.NewLine}");


    }

    
}



public class MakeBreakfast
{
/// <summary>
    /// 优化的异步方法: 代码同时监视多个任务，只在需要时才执行操作。
    /// </summary>
    public async Task MakeBreakfastOptimizationAsync()
    {
        Coffee coffee =  CookMession.PourCoffee();
        Console.WriteLine("coffee is ready");

        Task<Egg> eggTask = CookMession.FryEggsAsync(2);  //返回Task<Egg>类，因此使用此类变量接收;任务已经开始。
        Task<Bacon> baconTask = CookMession.FryBaconAsync(3);
        Task<Toast> toastTask = CookMession.ToastBreadApplyJamAndApplyButterAsync(2);

        var breakfastTasks = new List<Task> {eggTask, baconTask, toastTask};

        while(breakfastTasks.Count > 0)
        {
           Task finishedTask = await Task.WhenAny(breakfastTasks); // 获取任务列表中最先完成的任务
           if(finishedTask == eggTask)
           {Console.WriteLine("eggs is ready");}
           else if (finishedTask==baconTask)
           {Console.WriteLine("bacon is ready");}
           else if (finishedTask == toastTask)
           {Console.WriteLine("Toast is ready!");}
           breakfastTasks.Remove(finishedTask);
        }

        Juice oj = CookMession.PourOJ();
        Console.WriteLine("Orange Juice is ready!");

        Console.WriteLine("Breakfast is ready!");
    }


    /// <summary>
    /// 组合的异步方法
    /// </summary>
    public async Task MakeBreakfastGroupAsync()
    {
        Coffee coffee =  CookMession.PourCoffee();
        Console.WriteLine("coffee is ready");

        Task<Egg> eggTask = CookMession.FryEggsAsync(2);  //返回Task<Egg>类，因此使用此类变量接收;任务已经开始。
        Task<Bacon> baconTask = CookMession.FryBaconAsync(3);
        Task<Toast> toastTask = CookMession.ToastBreadApplyJamAndApplyButterAsync(2); //采取组合的异步方法

        Toast toast = await toastTask;  //await之后，即等待之后返回Task<Egg>任务泛型之中定义的变量，也是异步方法返回的变量
        Console.WriteLine("Toast is ready!");

        Egg egg = await eggTask;  //接收eggTask任务返回的egg,任务完成后即可返回
        Console.WriteLine("eggs is ready");

        Bacon bacon = await baconTask;
        Console.WriteLine("bacon is ready");


        Juice oj = CookMession.PourOJ();
        Console.WriteLine("Orange Juice is ready!");

        Console.WriteLine("Breakfast is ready!");
    }


    /// <summary>
    /// 使用了异步，但任务是顺序也是根据规划启动，节省时间；
    /// 同时，这样的改动对GUI程序很重要，原因是主线程被释放出来，界面可以响应用户。
    /// </summary>
    public async Task MakeBreakfastAsync()
    {
        Coffee coffee =  CookMession.PourCoffee();
        Console.WriteLine("coffee is ready");

        Task<Egg> eggTask = CookMession.FryEggsAsync(2);  //返回Task<Egg>类，因此使用此类变量接收;任务已经开始。
        Task<Bacon> baconTask = CookMession.FryBaconAsync(3);
        Task<Toast> toastTask = CookMession.ToastBreadAsync(2);


        //此时三个项目按照顺序接收，缺点是下面顺序开始的方法接收完才能接收之后的方法
        //toastTask相关的两个方法，不是异步方法，会耗用时间，之后才能接收其他方法
        Toast toast = await toastTask;  //await之后，即等待之后返回Task<Egg>任务泛型之中定义的变量，也是异步方法返回的变量
        CookMession.ApplyJam(toast);
        CookMession.ApplyButter(toast);
        Console.WriteLine("Toast is ready!");

        Egg egg = await eggTask;  //接收eggTask任务返回的egg,任务完成后即可返回
        Console.WriteLine("eggs is ready");

        Bacon bacon = await baconTask;
        Console.WriteLine("bacon is ready");


        Juice oj = CookMession.PourOJ();
        Console.WriteLine("Orange Juice is ready!");

        Console.WriteLine("Breakfast is ready!");
    }


    /// <summary>
    /// 虽然使用了异步，但任务是顺序启动；这和普通的任务顺序启动相同；
    /// 但这样的改动对GUI程序很重要，原因是主线程被释放出来，界面可以响应用户。
    /// </summary>
    public async Task MakeBreakfastFackAsync()
    {
        Coffee coffee =  CookMession.PourCoffee();
        Console.WriteLine("coffee is ready");

        Egg egg = await CookMession.FryEggsAsync(2);
        Console.WriteLine("eggs is ready");

        Bacon bacon = await CookMession.FryBaconAsync(3);
        Console.WriteLine("bacon is ready");


        Toast toast = await CookMession.ToastBreadAsync(2);
        CookMession.ApplyJam(toast);
        CookMession.ApplyButter(toast);
        Console.WriteLine("Toast is ready!");

        Juice oj = CookMession.PourOJ();
        Console.WriteLine("Orange Juice is ready!");

        Console.WriteLine("Breakfast is ready!");
    }


    /// <summary>
    /// 按照普通流程的顺序完成早餐任务
    /// </summary>
    public void MakeBreakfastSync()
    {   
        Coffee coffee =  CookMession.PourCoffee();
        Console.WriteLine("coffee is ready");

        Egg egg = CookMession.FryEggs(2);
        Console.WriteLine("eggs is ready");

        Bacon bacon = CookMession.FryBacon(3);
        Console.WriteLine("bacon is ready");

        Toast toast = CookMession.ToastBread(2);
        CookMession.ApplyJam(toast);
        CookMession.ApplyButter(toast);
        Console.WriteLine("Toast is ready!");

        Juice oj = CookMession.PourOJ();
        Console.WriteLine("Orange Juice is ready!");

        Console.WriteLine("Breakfast is ready!");
    }
}

