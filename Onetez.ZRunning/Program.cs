using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace Onetez.ZRunning
{
  internal class Program
  {
    static async Task Main(string[] args)
    {
      Console.Title = "Pancake POS Tools - by Jack Nguyen";

      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("::::: Pancake POS Tools - by Jack Nguyen :::::\n");
      
      Console.Write("Enter domain: ");
      string domain = Console.ReadLine().Replace("Enter domain: ", "");
      if (!domain.StartsWith("http"))
        domain = "http://" + domain;

      Console.Write("Auto create (y/n): ");
      bool create = Console.ReadLine().Replace("Auto create (y/n): ", "") == "y" ? true : false;

      await GetSheets(domain, create);

      Console.ReadKey();
    }


    private static async Task GetSheets(string domain, bool create)
    {
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine($"\nGET {domain.ToUpper()} - {string.Format("{0:yyyy-MM-dd, HH:mm:ss}", DateTime.Now)}");

      var link = domain + (create ? "/APIv1/Sheet/GetList" : "/APIv1/Sheet/RefreshData");
      var client = new RestClient(link);
      var request = new RestRequest(Method.GET);
      var response = await client.ExecuteAsync(request);

      if(!string.IsNullOrEmpty(response.Content))
      {
        try
        {
          if (create)
          {
            var results = JsonConvert.DeserializeObject<List<Models.Sheet>>(response.Content);
            if (results != null)
            {
              if (results.Count > 0)
              {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"SHEETS: {results.Count} new order");

                // Tự động tạo đơn
                if (create)
                {
                  int count = 0;
                  for (int i = 0; i < results.Count; i++)
                  {
                    await Task.Delay(1000);
                    var item = results[i];
                    if (await CreateOrder(domain, item, i))
                      count++;
                  }
                  Console.ForegroundColor = ConsoleColor.Green;
                  Console.WriteLine($"\n:::::::::: FINISH {count}/{results.Count} ::::::::::\n");
                }
              }
              else
              {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("SHEETS: No new order");
              }
            }
            else
            {
              Console.ForegroundColor = ConsoleColor.Red;
              Console.WriteLine("SHEETS: Error or No new order");
            }
          }
          else
          {
            var results = JsonConvert.DeserializeObject<Models.Refresh>(response.Content);
            if (results.status)
            {
              var count = results.message.Replace("Tìm thấy ", "").Replace(" đơn mới, tải lại trang để xem", "");
              Console.ForegroundColor = ConsoleColor.Green;
              Console.WriteLine($"SHEETS: {count} new order");
            }
            else
            {
              Console.ForegroundColor = ConsoleColor.Yellow;
              Console.WriteLine("SHEETS: No new order");
            }
          }
        }
        catch (Exception ex)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("ERROR: " + ex.Message);
        }
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("SHEETS: No new order");
      }

      // Tạm nghĩ
      Console.ForegroundColor = ConsoleColor.Yellow;
      if(7 <= DateTime.Now.Hour && DateTime.Now.Hour <= 22)
      {
        Console.WriteLine("Delay 10 minutes");
        await Task.Delay(TimeSpan.FromMinutes(10));
      }
      else
      {
        Console.WriteLine("Delay 1 hours");
        await Task.Delay(TimeSpan.FromHours(1));
        Console.Clear();
      }

      // Quét lại
      await GetSheets(domain, create);
    }

    private static async Task<bool> CreateOrder(string domain, Models.Sheet sheet, int index)
    {
      var link = domain + "/APIv1/Sheet/CreateOrder?id=" + sheet.Id;
      var client = new RestClient(link);
      var request = new RestRequest(Method.POST);
      var response = await client.ExecuteAsync(request);

      try
      {
        var results = JsonConvert.DeserializeObject<Models.Create>(response.Content);
        if(results != null && results.status)
        {
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine($"{index}. {sheet.Phone} → DONE");
          return true;
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"{index}. {sheet.Phone} → ERROR");
        }
      }
      catch (Exception ex)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{index}. {sheet.Phone} → ERROR: " + ex.Message);
      }

      return false;
    }
  }

}