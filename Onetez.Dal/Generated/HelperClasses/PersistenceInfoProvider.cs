///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: Wednesday, August 31, 2022 08:57:25
// Code is generated using templates: SD.TemplateBindings.SqlServerSpecific.NET20
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Data;

using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Dal.HelperClasses
{
	/// <summary>
	/// Singleton implementation of the PersistenceInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.
	/// </summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the PersistenceInfoProviderBase class is threadsafe.</remarks>
	internal sealed class PersistenceInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IPersistenceInfoProvider _providerInstance = new PersistenceInfoProviderCore();
		#endregion
		
		/// <summary>private ctor to prevent instances of this class.</summary>
		private PersistenceInfoProviderSingleton()
		{
		}

		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static PersistenceInfoProviderSingleton()
		{
		}

		/// <summary>Gets the singleton instance of the PersistenceInfoProviderCore</summary>
		/// <returns>Instance of the PersistenceInfoProvider.</returns>
		public static IPersistenceInfoProvider GetInstance()
		{
			return _providerInstance;
		}
	}

	/// <summary>Actual implementation of the PersistenceInfoProvider. Used by singleton wrapper.</summary>
	internal class PersistenceInfoProviderCore : PersistenceInfoProviderBase
	{
		/// <summary>Initializes a new instance of the <see cref="PersistenceInfoProviderCore"/> class.</summary>
		internal PersistenceInfoProviderCore()
		{
			Init();
		}

		/// <summary>Method which initializes the internal datastores with the structure of hierarchical types.</summary>
		private void Init()
		{
			base.InitClass((9 + 0));
			InitAdsEntityMappings();
			InitColorsEntityMappings();
			InitConfigsEntityMappings();
			InitCustomersEntityMappings();
			InitOrdersEntityMappings();
			InitProductsEntityMappings();
			InitSheetsEntityMappings();
			InitShopsEntityMappings();
			InitUsersEntityMappings();

		}


		/// <summary>Inits AdsEntity's mappings</summary>
		private void InitAdsEntityMappings()
		{
			base.AddElementMapping( "AdsEntity", "pancake_v3_fix_giangen", @"dbo", "Ads", 6 );
			base.AddElementFieldMapping( "AdsEntity", "Id", "Id", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AdsEntity", "Day", "Day", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 1 );
			base.AddElementFieldMapping( "AdsEntity", "Product", "Product", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "AdsEntity", "Cost", "Cost", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "AdsEntity", "Rate", "Rate", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "AdsEntity", "ShopId", "ShopId", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
		}
		/// <summary>Inits ColorsEntity's mappings</summary>
		private void InitColorsEntityMappings()
		{
			base.AddElementMapping( "ColorsEntity", "pancake_v3_fix_giangen", @"dbo", "Colors", 4 );
			base.AddElementFieldMapping( "ColorsEntity", "Id", "Id", false, (int)SqlDbType.Int, 0, 0, 10, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 0 );
			base.AddElementFieldMapping( "ColorsEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ColorsEntity", "Color", "Color", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ColorsEntity", "Type", "Type", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits ConfigsEntity's mappings</summary>
		private void InitConfigsEntityMappings()
		{
			base.AddElementMapping( "ConfigsEntity", "pancake_v3_fix_giangen", @"dbo", "Configs", 6 );
			base.AddElementFieldMapping( "ConfigsEntity", "Id", "Id", false, (int)SqlDbType.Int, 0, 0, 10, true, "SCOPE_IDENTITY()", null, typeof(System.Int32), 0 );
			base.AddElementFieldMapping( "ConfigsEntity", "GoogleServiceAccount", "GoogleServiceAccount", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ConfigsEntity", "GoogleApplicationName", "GoogleApplicationName", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ConfigsEntity", "PancakeApiUrl", "PancakeApiUrl", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ConfigsEntity", "IsAuto", "IsAuto", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "ConfigsEntity", "SheetCategory", "SheetCategory", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 5 );
		}
		/// <summary>Inits CustomersEntity's mappings</summary>
		private void InitCustomersEntityMappings()
		{
			base.AddElementMapping( "CustomersEntity", "pancake_v3_fix_giangen", @"dbo", "Customers", 5 );
			base.AddElementFieldMapping( "CustomersEntity", "Id", "Id", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 0 );
			base.AddElementFieldMapping( "CustomersEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "CustomersEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "CustomersEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "CustomersEntity", "IsDelete", "IsDelete", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
		}
		/// <summary>Inits OrdersEntity's mappings</summary>
		private void InitOrdersEntityMappings()
		{
			base.AddElementMapping( "OrdersEntity", "pancake_v3_fix_giangen", @"dbo", "Orders", 19 );
			base.AddElementFieldMapping( "OrdersEntity", "Id", "Id", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OrdersEntity", "ShopId", "ShopId", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "OrdersEntity", "OrderId", "OrderId", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "OrdersEntity", "BillName", "BillName", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "OrdersEntity", "BillPhone", "BillPhone", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "OrdersEntity", "Product", "Product", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "OrdersEntity", "ShipCode", "ShipCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "OrdersEntity", "ShipLogs", "ShipLogs", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "OrdersEntity", "ShipUpdate", "ShipUpdate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 8 );
			base.AddElementFieldMapping( "OrdersEntity", "ShipPhone", "ShipPhone", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "OrdersEntity", "ShipInStock", "ShipInStock", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 10 );
			base.AddElementFieldMapping( "OrdersEntity", "ShipStatus", "ShipStatus", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "OrdersEntity", "ShopLogs", "ShopLogs", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "OrdersEntity", "ShopUpdate", "ShopUpdate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 13 );
			base.AddElementFieldMapping( "OrdersEntity", "Complain", "Complain", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "OrdersEntity", "Status", "Status", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 15 );
			base.AddElementFieldMapping( "OrdersEntity", "UserHandling", "UserHandling", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "OrdersEntity", "LastUpdate", "LastUpdate", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "OrdersEntity", "PartnerId", "PartnerId", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 18 );
		}
		/// <summary>Inits ProductsEntity's mappings</summary>
		private void InitProductsEntityMappings()
		{
			base.AddElementMapping( "ProductsEntity", "pancake_v3_fix_giangen", @"dbo", "Products", 18 );
			base.AddElementFieldMapping( "ProductsEntity", "Id", "Id", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 0 );
			base.AddElementFieldMapping( "ProductsEntity", "VariationId", "VariationId", false, (int)SqlDbType.NVarChar, 36, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ProductsEntity", "DisplayId", "DisplayId", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ProductsEntity", "ProductId", "ProductId", false, (int)SqlDbType.NVarChar, 36, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ProductsEntity", "ProductDisplayId", "ProductDisplayId", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "ProductsEntity", "ProductName", "ProductName", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "ProductsEntity", "Price", "Price", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "ProductsEntity", "Fields", "Fields", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ProductsEntity", "ShopId", "ShopId", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "ProductsEntity", "SheetCode", "SheetCode", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "ProductsEntity", "ShipName", "ShipName", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "ProductsEntity", "Quantity", "Quantity", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 11 );
			base.AddElementFieldMapping( "ProductsEntity", "Weight", "Weight", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "ProductsEntity", "IsCombo", "IsCombo", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 13 );
			base.AddElementFieldMapping( "ProductsEntity", "ParentId", "ParentId", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "ProductsEntity", "Discount", "Discount", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 15 );
			base.AddElementFieldMapping( "ProductsEntity", "Size", "Size", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "ProductsEntity", "Color", "Color", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 17 );
		}
		/// <summary>Inits SheetsEntity's mappings</summary>
		private void InitSheetsEntityMappings()
		{
			base.AddElementMapping( "SheetsEntity", "pancake_v3_fix_giangen", @"dbo", "Sheets", 30 );
			base.AddElementFieldMapping( "SheetsEntity", "Id", "Id", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 0 );
			base.AddElementFieldMapping( "SheetsEntity", "ShopId", "ShopId", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "SheetsEntity", "StatusId", "StatusId", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "SheetsEntity", "OrderId", "OrderId", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "SheetsEntity", "Date", "Date", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 4 );
			base.AddElementFieldMapping( "SheetsEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "SheetsEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "SheetsEntity", "Address", "Address", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "SheetsEntity", "Product", "Product", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "SheetsEntity", "Link", "Link", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "SheetsEntity", "Error", "Error", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "SheetsEntity", "Size", "Size", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "SheetsEntity", "Color", "Color", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "SheetsEntity", "Note", "Note", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "SheetsEntity", "ProductOther", "ProductOther", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "SheetsEntity", "ProcessId", "ProcessId", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 15 );
			base.AddElementFieldMapping( "SheetsEntity", "ProcessLog", "ProcessLog", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "SheetsEntity", "ProcessDate", "ProcessDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 17 );
			base.AddElementFieldMapping( "SheetsEntity", "ProcessCall", "ProcessCall", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 18 );
			base.AddElementFieldMapping( "SheetsEntity", "ProcessNote", "ProcessNote", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "SheetsEntity", "ProcessProduct", "ProcessProduct", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "SheetsEntity", "UserId", "UserId", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "SheetsEntity", "Revenue", "Revenue", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 22 );
			base.AddElementFieldMapping( "SheetsEntity", "CancelDate", "CancelDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 23 );
			base.AddElementFieldMapping( "SheetsEntity", "AppleDate", "AppleDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 24 );
			base.AddElementFieldMapping( "SheetsEntity", "Location", "Location", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 25 );
			base.AddElementFieldMapping( "SheetsEntity", "Category", "Category", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "SheetsEntity", "Category2", "Category2", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 27 );
			base.AddElementFieldMapping( "SheetsEntity", "Category3", "Category3", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 28 );
			base.AddElementFieldMapping( "SheetsEntity", "ProcessCode", "ProcessCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 29 );
		}
		/// <summary>Inits ShopsEntity's mappings</summary>
		private void InitShopsEntityMappings()
		{
			base.AddElementMapping( "ShopsEntity", "pancake_v3_fix_giangen", @"dbo", "Shops", 13 );
			base.AddElementFieldMapping( "ShopsEntity", "Id", "Id", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 0 );
			base.AddElementFieldMapping( "ShopsEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ShopsEntity", "ApiKey", "ApiKey", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ShopsEntity", "WarehouseId", "WarehouseId", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ShopsEntity", "WarehouseInfo", "WarehouseInfo", false, (int)SqlDbType.NVarChar, 1000, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "ShopsEntity", "SpreadsheetId", "SpreadsheetId", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "ShopsEntity", "SpreadsheetTab", "SpreadsheetTab", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "ShopsEntity", "SheetColumns", "SheetColumns", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ShopsEntity", "OrderPage", "OrderPage", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "ShopsEntity", "ProductOtherInNote", "ProductOtherInNote", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "ShopsEntity", "ProductErrorToNote", "ProductErrorToNote", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "ShopsEntity", "ProductFindByName", "ProductFindByName", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 11 );
			base.AddElementFieldMapping( "ShopsEntity", "ProductToOrderEmpty", "ProductToOrderEmpty", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 12 );
		}
		/// <summary>Inits UsersEntity's mappings</summary>
		private void InitUsersEntityMappings()
		{
			base.AddElementMapping( "UsersEntity", "pancake_v3_fix_giangen", @"dbo", "Users", 10 );
			base.AddElementFieldMapping( "UsersEntity", "UserId", "UserId", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 0 );
			base.AddElementFieldMapping( "UsersEntity", "Username", "Username", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "UsersEntity", "Password", "Password", false, (int)SqlDbType.VarChar, 32, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "UsersEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "UsersEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "UsersEntity", "Avatar", "Avatar", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "UsersEntity", "RoleId", "RoleId", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "UsersEntity", "Online", "Online", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 7 );
			base.AddElementFieldMapping( "UsersEntity", "IsAds", "IsAds", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 8 );
			base.AddElementFieldMapping( "UsersEntity", "IsReport", "IsReport", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
		}

	}
}