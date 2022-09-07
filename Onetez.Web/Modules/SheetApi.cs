using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using Onetez.Core.DbContext;
using Onetez.Dal.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Onetez.Dal.EntityClasses;

namespace Onetez.Web.Modules
{
  public class SheetApi
  {
    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static SheetsService service;
    static string sheetOrder = "Orders";

    private static void SheetsCredential()
    {
      var ConfigInfo = DbConfig.Get(1);
      var ApplicationName = ConfigInfo.GoogleApplicationName;
      var pathSecret = AppDomain.CurrentDomain.BaseDirectory + @"\GoogleSecret\GoogleSecret.json";

      GoogleCredential credential;
      using (var stream = new FileStream(pathSecret, FileMode.Open, FileAccess.Read))
      {
        credential = GoogleCredential.FromStream(stream)
            .CreateScoped(Scopes);
      }

      // Create Google Sheets API service.
      service = new SheetsService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });
    }


    public static List<SheetModel> GetSheetData(ShopModel shop, out string error)
    {
      error = string.Empty;

      var sheetData = new List<SheetModel>();

      var SheetId = shop.spreadsheet_id;
      var SheetTab = shop.spreadsheet_tab;
      var sheetColumns = shop.sheet_columns;

      // Không có sheet thì bỏ qua
      if (string.IsNullOrEmpty(SheetTab))
        return sheetData;

      // Create Google Sheets API service.
      SheetsCredential();

      try
      {
        // Get Sheet data
        var range = $"{SheetTab}";
        //var range = $"{SheetTab}!A:K";
        var request = service.Spreadsheets.Values.Get(SheetId, range);

        var response = request.Execute();
        var values = response.Values;
        if (values != null && values.Count > 0)
        {
          for (int i = 1; i < values.Count; i++)
          {
            var row = values[i];

            // Phải có đủ số cột mới ghi nhận
            if (row.Count > sheetColumns.product
              && !string.IsNullOrEmpty(row[sheetColumns.date].ToString())
              && !string.IsNullOrEmpty(row[sheetColumns.phone].ToString()))
            {
              try
              {
                // Kiểm tra đã lưu vào Tool hay chưa
                // Nếu rồi thì không import vào Tool nữa
                if (sheetColumns.save > 0 && row.Count > sheetColumns.save)
                {
                  if (!string.IsNullOrEmpty(row[sheetColumns.save].ToString()))
                    continue;
                }

                var date = DateTime.Now.AddDays(-10);
                var dateParse = DateTime.Now;
                var dateStr = row[sheetColumns.date].ToString();

                // Chuẩn hóa thời gian
                if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy HH:mm:ss",
                      CultureInfo.InvariantCulture, DateTimeStyles.None, out dateParse))
                  date = dateParse;
                else if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy H:mm:ss",
                      CultureInfo.InvariantCulture, DateTimeStyles.None, out dateParse))
                  date = dateParse;
                else if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy",
                      CultureInfo.InvariantCulture, DateTimeStyles.None, out dateParse))
                  date = dateParse;
                else if (DateTime.TryParse(dateStr, out dateParse))
                  date = dateParse;

                var name = row[sheetColumns.name].ToString().Trim();
                var phone = row[sheetColumns.phone].ToString().Trim();
                var product = row[sheetColumns.product].ToString().Replace("  ", " ").Trim();
                var link = sheetColumns.link > 0 && row.Count > sheetColumns.link ? row[sheetColumns.link].ToString() : "";
                //var note = sheetColumns.note > 0 && row.Count > sheetColumns.note ? row[sheetColumns.note].ToString() : "";
                //var size = sheetColumns.size > 0 && row.Count > sheetColumns.size ? row[sheetColumns.size].ToString().Trim().ToLower() : "";
                //var color = sheetColumns.color > 0 && row.Count > sheetColumns.color ? row[sheetColumns.color].ToString().Trim().ToLower() : "";

                // Lấy địa chỉ
                var address = string.Empty;
                if(row.Count >= sheetColumns.address + 4)
                {
                  int col = sheetColumns.address;
                  address = $"{row[col]}|{row[col+1]}|{row[col + 2]}|{row[col + 3]}|{row[col + 4]}";
                }

                var model = new SheetModel()
                {
                  Id = DbConfig.GenerateId(),
                  ShopId = shop.id,
                  Date = date,
                  DateStr = dateStr + string.Format(" → {0:yyyy-MM-dd HH:mm:ss}", date),
                  Name = name,
                  Phone = phone,
                  Address = address,
                  Product = product,
                  Link = link,
                  Size = "",
                  Color = ""
                  //Others = others,
                  //Note = note,
                };

                sheetData.Add(model);
              }
              catch (Exception ex)
              {
                // Lỗi không đọc được column Sheet
                error += row[0] + " = " + ex.Message;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        // Lỗi không đọc được Sheet
        error += ex.Message;
      }


      return sheetData;
    }


    public static string CreateRow(ShopModel shop, SheetsEntity sheet, out string error)
    {
      error = string.Empty;

      // Create Google Sheets API service.
      SheetsCredential();

      var result = string.Empty;
      var SheetId = shop.api_key;
      try
      {
        var row = ConvertSheetRow(shop, sheet);
        var valueRange = new ValueRange();
        valueRange.Values = new List<IList<object>> { row };

        var range = $"{sheetOrder}!A:K";
        var request = service.Spreadsheets.Values.Append(valueRange, SheetId, range);
        request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        var response = request.Execute();

        result = response.Updates.UpdatedRange;
      }
      catch (Exception ex)
      {
        error = ex.Message;
      }

      return result.Replace($"{sheetOrder}!", "");
    }


    public static string UpdateRow(ShopModel shop, SheetsEntity sheet, out string error)
    {
      error = string.Empty;


      // Create Google Sheets API service.
      SheetsCredential();

      var result = string.Empty;
      var SheetId = shop.api_key;
      try
      {
        var row = ConvertSheetRow(shop, sheet);
        var valueRange = new ValueRange();
        valueRange.Values = new List<IList<object>> { row };

        var range = $"{sheetOrder}!{sheet.OrderId}";

        var request = service.Spreadsheets.Values.Update(valueRange, SheetId, range);
        request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        var response = request.Execute();

        result = response.UpdatedRange;
      }
      catch (Exception ex)
      {
        error = ex.Message;
      }

      return result.Replace($"{sheetOrder}!", "");
    }


    /// <summary>
    /// Chuyển SheetsEntity thành SheetRow
    /// </summary>
    public static List<object> ConvertSheetRow(ShopModel shop, SheetsEntity sheet)
    {
      //var staff = DbUser.Get(sheet.UserId);
      var address = ConvertAddress(sheet.Location);
      var product = DbProduct.GetBySheetInfo(sheet, shop.product_find_by_name);

      return new List<object>() {
          "",
          string.Format("{0:yyyy/MM/dd, HH:mm}", sheet.Date),
          sheet.Name,
          sheet.Phone,
          sheet.Product,
          product != null ? product.ProductDisplayId : sheet.ProcessCode,
          address.state,
          address.city,
          address.locality,
          address.address,
          address.pin,
          //product != null ? product.ProductName : sheet.Product,
          //product != null ? product.Quantity : 1,
          //sheet.Revenue,
          //staff != null ? staff.Name : sheet.UserId,
          //string.Format("{0:yyyy/MM/dd, HH:mm}", sheet.ProcessCall)
        };
    }


    /// <summary>
    /// Chuyển string thành Address
    /// </summary>
    public static SheetModel.AddressInfo ConvertAddress(string address)
    {
      var data = address.Split('|');

      var result = new SheetModel.AddressInfo();
      if(data.Length > 0)
        result.state = data[0].Trim();
      if(data.Length > 1)
        result.city = data[1].Trim();
      if(data.Length > 2)
        result.locality = data[2].Trim();
      if(data.Length > 3)
        result.address = data[3].Trim();
      if(data.Length > 4)
        result.pin = data[4].Trim();

      return result;
    }
  }
}
