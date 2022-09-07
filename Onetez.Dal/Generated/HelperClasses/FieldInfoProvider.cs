///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: Wednesday, August 31, 2022 08:57:24
// Code is generated using templates: SD.TemplateBindings.SharedTemplates.NET20
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Dal.HelperClasses
{
	
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>
	/// Singleton implementation of the FieldInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.
	/// </summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the FieldInfoProviderBase class is threadsafe.</remarks>
	internal sealed class FieldInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IFieldInfoProvider _providerInstance = new FieldInfoProviderCore();
		#endregion
		
		/// <summary>private ctor to prevent instances of this class.</summary>
		private FieldInfoProviderSingleton()
		{
		}

		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static FieldInfoProviderSingleton()
		{
		}

		/// <summary>Gets the singleton instance of the FieldInfoProviderCore</summary>
		/// <returns>Instance of the FieldInfoProvider.</returns>
		public static IFieldInfoProvider GetInstance()
		{
			return _providerInstance;
		}
	}

	/// <summary>Actual implementation of the FieldInfoProvider. Used by singleton wrapper.</summary>
	internal class FieldInfoProviderCore : FieldInfoProviderBase
	{
		/// <summary>Initializes a new instance of the <see cref="FieldInfoProviderCore"/> class.</summary>
		internal FieldInfoProviderCore()
		{
			Init();
		}

		/// <summary>Method which initializes the internal datastores.</summary>
		private void Init()
		{
			base.InitClass( (9 + 0));
			InitAdsEntityInfos();
			InitColorsEntityInfos();
			InitConfigsEntityInfos();
			InitCustomersEntityInfos();
			InitOrdersEntityInfos();
			InitProductsEntityInfos();
			InitSheetsEntityInfos();
			InitShopsEntityInfos();
			InitUsersEntityInfos();

			base.ConstructElementFieldStructures(InheritanceInfoProviderSingleton.GetInstance());
		}

		/// <summary>Inits AdsEntity's FieldInfo objects</summary>
		private void InitAdsEntityInfos()
		{
			base.AddElementFieldInfo("AdsEntity", "Id", typeof(System.Int64), true, false, true, false,  (int)AdsFieldIndex.Id, 0, 0, 19);
			base.AddElementFieldInfo("AdsEntity", "Day", typeof(System.DateTime), false, false, false, false,  (int)AdsFieldIndex.Day, 0, 0, 0);
			base.AddElementFieldInfo("AdsEntity", "Product", typeof(System.String), false, false, false, false,  (int)AdsFieldIndex.Product, 200, 0, 0);
			base.AddElementFieldInfo("AdsEntity", "Cost", typeof(System.Double), false, false, false, false,  (int)AdsFieldIndex.Cost, 0, 0, 38);
			base.AddElementFieldInfo("AdsEntity", "Rate", typeof(System.Double), false, false, false, false,  (int)AdsFieldIndex.Rate, 0, 0, 38);
			base.AddElementFieldInfo("AdsEntity", "ShopId", typeof(System.Int32), false, false, false, false,  (int)AdsFieldIndex.ShopId, 0, 0, 10);
		}
		/// <summary>Inits ColorsEntity's FieldInfo objects</summary>
		private void InitColorsEntityInfos()
		{
			base.AddElementFieldInfo("ColorsEntity", "Id", typeof(System.Int32), true, false, true, false,  (int)ColorsFieldIndex.Id, 0, 0, 10);
			base.AddElementFieldInfo("ColorsEntity", "Name", typeof(System.String), false, false, false, false,  (int)ColorsFieldIndex.Name, 200, 0, 0);
			base.AddElementFieldInfo("ColorsEntity", "Color", typeof(System.String), false, false, false, false,  (int)ColorsFieldIndex.Color, 50, 0, 0);
			base.AddElementFieldInfo("ColorsEntity", "Type", typeof(System.String), false, false, false, false,  (int)ColorsFieldIndex.Type, 50, 0, 0);
		}
		/// <summary>Inits ConfigsEntity's FieldInfo objects</summary>
		private void InitConfigsEntityInfos()
		{
			base.AddElementFieldInfo("ConfigsEntity", "Id", typeof(System.Int32), true, false, true, false,  (int)ConfigsFieldIndex.Id, 0, 0, 10);
			base.AddElementFieldInfo("ConfigsEntity", "GoogleServiceAccount", typeof(System.String), false, false, false, false,  (int)ConfigsFieldIndex.GoogleServiceAccount, 50, 0, 0);
			base.AddElementFieldInfo("ConfigsEntity", "GoogleApplicationName", typeof(System.String), false, false, false, false,  (int)ConfigsFieldIndex.GoogleApplicationName, 50, 0, 0);
			base.AddElementFieldInfo("ConfigsEntity", "PancakeApiUrl", typeof(System.String), false, false, false, false,  (int)ConfigsFieldIndex.PancakeApiUrl, 50, 0, 0);
			base.AddElementFieldInfo("ConfigsEntity", "IsAuto", typeof(System.Boolean), false, false, false, false,  (int)ConfigsFieldIndex.IsAuto, 0, 0, 0);
			base.AddElementFieldInfo("ConfigsEntity", "SheetCategory", typeof(System.String), false, false, false, false,  (int)ConfigsFieldIndex.SheetCategory, 2147483647, 0, 0);
		}
		/// <summary>Inits CustomersEntity's FieldInfo objects</summary>
		private void InitCustomersEntityInfos()
		{
			base.AddElementFieldInfo("CustomersEntity", "Id", typeof(System.String), true, false, false, false,  (int)CustomersFieldIndex.Id, 20, 0, 0);
			base.AddElementFieldInfo("CustomersEntity", "Name", typeof(System.String), false, false, false, false,  (int)CustomersFieldIndex.Name, 50, 0, 0);
			base.AddElementFieldInfo("CustomersEntity", "Phone", typeof(System.String), false, false, false, false,  (int)CustomersFieldIndex.Phone, 50, 0, 0);
			base.AddElementFieldInfo("CustomersEntity", "Email", typeof(System.String), false, false, false, false,  (int)CustomersFieldIndex.Email, 50, 0, 0);
			base.AddElementFieldInfo("CustomersEntity", "IsDelete", typeof(System.Boolean), false, false, false, false,  (int)CustomersFieldIndex.IsDelete, 0, 0, 0);
		}
		/// <summary>Inits OrdersEntity's FieldInfo objects</summary>
		private void InitOrdersEntityInfos()
		{
			base.AddElementFieldInfo("OrdersEntity", "Id", typeof(System.Int64), true, false, true, false,  (int)OrdersFieldIndex.Id, 0, 0, 19);
			base.AddElementFieldInfo("OrdersEntity", "ShopId", typeof(System.Int32), false, false, false, false,  (int)OrdersFieldIndex.ShopId, 0, 0, 10);
			base.AddElementFieldInfo("OrdersEntity", "OrderId", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.OrderId, 200, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "BillName", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.BillName, 200, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "BillPhone", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.BillPhone, 50, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "Product", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.Product, 200, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "ShipCode", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.ShipCode, 50, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "ShipLogs", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.ShipLogs, 2147483647, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "ShipUpdate", typeof(System.DateTime), false, false, false, false,  (int)OrdersFieldIndex.ShipUpdate, 0, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "ShipPhone", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.ShipPhone, 50, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "ShipInStock", typeof(System.DateTime), false, false, false, false,  (int)OrdersFieldIndex.ShipInStock, 0, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "ShipStatus", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.ShipStatus, 200, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "ShopLogs", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.ShopLogs, 2147483647, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "ShopUpdate", typeof(System.DateTime), false, false, false, false,  (int)OrdersFieldIndex.ShopUpdate, 0, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "Complain", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.Complain, 500, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "Status", typeof(System.Int32), false, false, false, false,  (int)OrdersFieldIndex.Status, 0, 0, 10);
			base.AddElementFieldInfo("OrdersEntity", "UserHandling", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.UserHandling, 20, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "LastUpdate", typeof(System.String), false, false, false, false,  (int)OrdersFieldIndex.LastUpdate, 20, 0, 0);
			base.AddElementFieldInfo("OrdersEntity", "PartnerId", typeof(System.Int32), false, false, false, false,  (int)OrdersFieldIndex.PartnerId, 0, 0, 10);
		}
		/// <summary>Inits ProductsEntity's FieldInfo objects</summary>
		private void InitProductsEntityInfos()
		{
			base.AddElementFieldInfo("ProductsEntity", "Id", typeof(System.String), true, false, false, false,  (int)ProductsFieldIndex.Id, 20, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "VariationId", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.VariationId, 36, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "DisplayId", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.DisplayId, 50, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "ProductId", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.ProductId, 36, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "ProductDisplayId", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.ProductDisplayId, 50, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "ProductName", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.ProductName, 200, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "Price", typeof(System.Double), false, false, false, false,  (int)ProductsFieldIndex.Price, 0, 0, 38);
			base.AddElementFieldInfo("ProductsEntity", "Fields", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.Fields, 500, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "ShopId", typeof(System.Int32), false, false, false, false,  (int)ProductsFieldIndex.ShopId, 0, 0, 10);
			base.AddElementFieldInfo("ProductsEntity", "SheetCode", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.SheetCode, 200, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "ShipName", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.ShipName, 200, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "Quantity", typeof(System.Int32), false, false, false, false,  (int)ProductsFieldIndex.Quantity, 0, 0, 10);
			base.AddElementFieldInfo("ProductsEntity", "Weight", typeof(System.Int32), false, false, false, false,  (int)ProductsFieldIndex.Weight, 0, 0, 10);
			base.AddElementFieldInfo("ProductsEntity", "IsCombo", typeof(System.Boolean), false, false, false, false,  (int)ProductsFieldIndex.IsCombo, 0, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "ParentId", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.ParentId, 20, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "Discount", typeof(System.Double), false, false, false, false,  (int)ProductsFieldIndex.Discount, 0, 0, 38);
			base.AddElementFieldInfo("ProductsEntity", "Size", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.Size, 100, 0, 0);
			base.AddElementFieldInfo("ProductsEntity", "Color", typeof(System.String), false, false, false, false,  (int)ProductsFieldIndex.Color, 100, 0, 0);
		}
		/// <summary>Inits SheetsEntity's FieldInfo objects</summary>
		private void InitSheetsEntityInfos()
		{
			base.AddElementFieldInfo("SheetsEntity", "Id", typeof(System.String), true, false, false, false,  (int)SheetsFieldIndex.Id, 20, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "ShopId", typeof(System.Int32), false, false, false, false,  (int)SheetsFieldIndex.ShopId, 0, 0, 10);
			base.AddElementFieldInfo("SheetsEntity", "StatusId", typeof(System.Int32), false, false, false, false,  (int)SheetsFieldIndex.StatusId, 0, 0, 10);
			base.AddElementFieldInfo("SheetsEntity", "OrderId", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.OrderId, 20, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Date", typeof(System.DateTime), false, false, false, false,  (int)SheetsFieldIndex.Date, 0, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Name", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Name, 500, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Phone", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Phone, 50, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Address", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Address, 500, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Product", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Product, 500, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Link", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Link, 500, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Error", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Error, 500, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Size", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Size, 100, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Color", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Color, 100, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Note", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Note, 500, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "ProductOther", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.ProductOther, 2147483647, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "ProcessId", typeof(System.Int32), false, false, false, false,  (int)SheetsFieldIndex.ProcessId, 0, 0, 10);
			base.AddElementFieldInfo("SheetsEntity", "ProcessLog", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.ProcessLog, 2147483647, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "ProcessDate", typeof(System.DateTime), false, false, false, false,  (int)SheetsFieldIndex.ProcessDate, 0, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "ProcessCall", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)SheetsFieldIndex.ProcessCall, 0, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "ProcessNote", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.ProcessNote, 500, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "ProcessProduct", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.ProcessProduct, 200, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "UserId", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.UserId, 20, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Revenue", typeof(System.Double), false, false, false, false,  (int)SheetsFieldIndex.Revenue, 0, 0, 38);
			base.AddElementFieldInfo("SheetsEntity", "CancelDate", typeof(System.DateTime), false, false, false, false,  (int)SheetsFieldIndex.CancelDate, 0, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "AppleDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)SheetsFieldIndex.AppleDate, 0, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Location", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Location, 2147483647, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Category", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Category, 200, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Category2", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Category2, 200, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "Category3", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.Category3, 200, 0, 0);
			base.AddElementFieldInfo("SheetsEntity", "ProcessCode", typeof(System.String), false, false, false, false,  (int)SheetsFieldIndex.ProcessCode, 50, 0, 0);
		}
		/// <summary>Inits ShopsEntity's FieldInfo objects</summary>
		private void InitShopsEntityInfos()
		{
			base.AddElementFieldInfo("ShopsEntity", "Id", typeof(System.Int32), true, false, false, false,  (int)ShopsFieldIndex.Id, 0, 0, 10);
			base.AddElementFieldInfo("ShopsEntity", "Name", typeof(System.String), false, false, false, false,  (int)ShopsFieldIndex.Name, 200, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "ApiKey", typeof(System.String), false, false, false, false,  (int)ShopsFieldIndex.ApiKey, 50, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "WarehouseId", typeof(System.String), false, false, false, false,  (int)ShopsFieldIndex.WarehouseId, 50, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "WarehouseInfo", typeof(System.String), false, false, false, false,  (int)ShopsFieldIndex.WarehouseInfo, 1000, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "SpreadsheetId", typeof(System.String), false, false, false, false,  (int)ShopsFieldIndex.SpreadsheetId, 50, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "SpreadsheetTab", typeof(System.String), false, false, false, false,  (int)ShopsFieldIndex.SpreadsheetTab, 50, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "SheetColumns", typeof(System.String), false, false, false, false,  (int)ShopsFieldIndex.SheetColumns, 2147483647, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "OrderPage", typeof(System.Int32), false, false, false, false,  (int)ShopsFieldIndex.OrderPage, 0, 0, 10);
			base.AddElementFieldInfo("ShopsEntity", "ProductOtherInNote", typeof(System.Boolean), false, false, false, false,  (int)ShopsFieldIndex.ProductOtherInNote, 0, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "ProductErrorToNote", typeof(System.Boolean), false, false, false, false,  (int)ShopsFieldIndex.ProductErrorToNote, 0, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "ProductFindByName", typeof(System.Boolean), false, false, false, false,  (int)ShopsFieldIndex.ProductFindByName, 0, 0, 0);
			base.AddElementFieldInfo("ShopsEntity", "ProductToOrderEmpty", typeof(System.String), false, false, false, false,  (int)ShopsFieldIndex.ProductToOrderEmpty, 500, 0, 0);
		}
		/// <summary>Inits UsersEntity's FieldInfo objects</summary>
		private void InitUsersEntityInfos()
		{
			base.AddElementFieldInfo("UsersEntity", "UserId", typeof(System.String), true, false, false, false,  (int)UsersFieldIndex.UserId, 20, 0, 0);
			base.AddElementFieldInfo("UsersEntity", "Username", typeof(System.String), false, false, false, false,  (int)UsersFieldIndex.Username, 200, 0, 0);
			base.AddElementFieldInfo("UsersEntity", "Password", typeof(System.String), false, false, false, false,  (int)UsersFieldIndex.Password, 32, 0, 0);
			base.AddElementFieldInfo("UsersEntity", "Email", typeof(System.String), false, false, false, false,  (int)UsersFieldIndex.Email, 200, 0, 0);
			base.AddElementFieldInfo("UsersEntity", "Name", typeof(System.String), false, false, false, false,  (int)UsersFieldIndex.Name, 200, 0, 0);
			base.AddElementFieldInfo("UsersEntity", "Avatar", typeof(System.String), false, false, false, false,  (int)UsersFieldIndex.Avatar, 500, 0, 0);
			base.AddElementFieldInfo("UsersEntity", "RoleId", typeof(System.Int32), false, false, false, false,  (int)UsersFieldIndex.RoleId, 0, 0, 10);
			base.AddElementFieldInfo("UsersEntity", "Online", typeof(System.DateTime), false, false, false, false,  (int)UsersFieldIndex.Online, 0, 0, 0);
			base.AddElementFieldInfo("UsersEntity", "IsAds", typeof(System.Boolean), false, false, false, false,  (int)UsersFieldIndex.IsAds, 0, 0, 0);
			base.AddElementFieldInfo("UsersEntity", "IsReport", typeof(System.Boolean), false, false, false, false,  (int)UsersFieldIndex.IsReport, 0, 0, 0);
		}
		
	}
}




