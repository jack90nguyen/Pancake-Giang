using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Onetez.Core.Libs;
using Onetez.Core.DbContext;
using Onetez.Dal.Models;
using Onetez.Dal.EntityClasses;
using RestSharp;
using Newtonsoft.Json;

namespace Onetez.Web.Modules
{
  public class PancakeApi
  {
    private static ConfigsEntity ConfigInfo = DbConfig.Get(1);
    private static string APIUrl = ConfigInfo.PancakeApiUrl;


    public static List<ProductsModel.DataProduct> ProductList(ShopModel shop)
    {
      string api_url = APIUrl + $"/shops/{shop.id}/variations?api_key={shop.api_key}&page_size=1000";

      //Create SSL/TLS secure channel
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      //Call API
      var client = new RestClient(api_url);
      var request = new RestRequest(Method.GET);
      request.AddHeader("Accept", "application/json");
      request.AddHeader("content-type", "application/json");
      IRestResponse response = client.Execute(request);

      string code = response.StatusCode.ToString();
      string content = response.Content;

      if (code == "OK")
      {
        var data = JsonConvert.DeserializeObject<ProductsModel.Rootobject>(content);

        return data.data;
      }
      else
      {
        return null;
      }
    }



    public static List<OrderDataModel> OrderList(ShopModel shop, int page_number, out string error)
    {
      int page_size = 500;
      error = string.Empty;

      string api_url = APIUrl + $"/shops/{shop.id}/orders?api_key={shop.api_key}&page_number={page_number}&page_size=" + page_size;

      //Create SSL/TLS secure channel
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      //Call API
      var client = new RestClient(api_url);
      var request = new RestRequest(Method.GET);
      request.AddHeader("Accept", "application/json");
      request.AddHeader("content-type", "application/json");
      IRestResponse response = client.Execute(request);

      string code = response.StatusCode.ToString();
      string content = response.Content;

      if (code == "OK")
      {
        try
        {
          //content = content.Replace("+07:00", "");

          var orderObject = JsonConvert.DeserializeObject<OrderObject>(content);

          if (orderObject.data != null)
            return orderObject.data;
          else
          {
            error = response.Content;

            return null;
          }
        }
        catch (Exception ex)
        {
          error = ex.Message;

          return null;
        }
      }
      else
      {
        error = response.Content;

        return null;
      }
    }


    public static OrderCreateResult.DataOrder CreateOrder(ShopModel shop, OrderModel data, out string message, out string error)
    {
      string jsonData = JsonConvert.SerializeObject(data);

      string api_url = APIUrl + $"/shops/{shop.id}/orders?api_key={shop.api_key}";

      //Create SSL/TLS secure channel
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      //Call API
      var client = new RestClient(api_url);
      var request = new RestRequest(Method.POST);
      request.AddHeader("Accept", "application/json");
      request.AddHeader("content-type", "application/json");
      request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
      IRestResponse response = client.Execute(request);

      string code = response.StatusCode.ToString();
      string content = response.Content;

      if (code == "Created")
      {
        var orderCreate = JsonConvert.DeserializeObject<OrderCreateResult.Rootobject>(content);

        if (orderCreate != null)
        {
          message = null;
          error = null;

          return orderCreate.data;
        }
        else
        {
          message = "Created | Không lấy được OrderId";
          error = content;

          return null;
        }
      }
      else
      {
        message = code + " | Không thể tạo đơn";
        error = content;

        return null;
      }
    }
  }
}
