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

namespace Onetez.Dal
{

	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Ads.
	/// </summary>
	public enum AdsFieldIndex:int
	{
		///<summary>Id. </summary>
		Id,
		///<summary>Day. </summary>
		Day,
		///<summary>Product. </summary>
		Product,
		///<summary>Cost. </summary>
		Cost,
		///<summary>Rate. </summary>
		Rate,
		///<summary>ShopId. </summary>
		ShopId,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Colors.
	/// </summary>
	public enum ColorsFieldIndex:int
	{
		///<summary>Id. </summary>
		Id,
		///<summary>Name. </summary>
		Name,
		///<summary>Color. </summary>
		Color,
		///<summary>Type. </summary>
		Type,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Configs.
	/// </summary>
	public enum ConfigsFieldIndex:int
	{
		///<summary>Id. </summary>
		Id,
		///<summary>GoogleServiceAccount. </summary>
		GoogleServiceAccount,
		///<summary>GoogleApplicationName. </summary>
		GoogleApplicationName,
		///<summary>PancakeApiUrl. </summary>
		PancakeApiUrl,
		///<summary>IsAuto. </summary>
		IsAuto,
		///<summary>SheetCategory. </summary>
		SheetCategory,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Customers.
	/// </summary>
	public enum CustomersFieldIndex:int
	{
		///<summary>Id. </summary>
		Id,
		///<summary>Name. </summary>
		Name,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Email. </summary>
		Email,
		///<summary>IsDelete. </summary>
		IsDelete,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Orders.
	/// </summary>
	public enum OrdersFieldIndex:int
	{
		///<summary>Id. </summary>
		Id,
		///<summary>ShopId. </summary>
		ShopId,
		///<summary>OrderId. </summary>
		OrderId,
		///<summary>BillName. </summary>
		BillName,
		///<summary>BillPhone. </summary>
		BillPhone,
		///<summary>Product. </summary>
		Product,
		///<summary>ShipCode. </summary>
		ShipCode,
		///<summary>ShipLogs. </summary>
		ShipLogs,
		///<summary>ShipUpdate. </summary>
		ShipUpdate,
		///<summary>ShipPhone. </summary>
		ShipPhone,
		///<summary>ShipInStock. </summary>
		ShipInStock,
		///<summary>ShipStatus. </summary>
		ShipStatus,
		///<summary>ShopLogs. </summary>
		ShopLogs,
		///<summary>ShopUpdate. </summary>
		ShopUpdate,
		///<summary>Complain. </summary>
		Complain,
		///<summary>Status. </summary>
		Status,
		///<summary>UserHandling. </summary>
		UserHandling,
		///<summary>LastUpdate. </summary>
		LastUpdate,
		///<summary>PartnerId. </summary>
		PartnerId,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Products.
	/// </summary>
	public enum ProductsFieldIndex:int
	{
		///<summary>Id. </summary>
		Id,
		///<summary>VariationId. </summary>
		VariationId,
		///<summary>DisplayId. </summary>
		DisplayId,
		///<summary>ProductId. </summary>
		ProductId,
		///<summary>ProductDisplayId. </summary>
		ProductDisplayId,
		///<summary>ProductName. </summary>
		ProductName,
		///<summary>Price. </summary>
		Price,
		///<summary>Fields. </summary>
		Fields,
		///<summary>ShopId. </summary>
		ShopId,
		///<summary>SheetCode. </summary>
		SheetCode,
		///<summary>ShipName. </summary>
		ShipName,
		///<summary>Quantity. </summary>
		Quantity,
		///<summary>Weight. </summary>
		Weight,
		///<summary>IsCombo. </summary>
		IsCombo,
		///<summary>ParentId. </summary>
		ParentId,
		///<summary>Discount. </summary>
		Discount,
		///<summary>Size. </summary>
		Size,
		///<summary>Color. </summary>
		Color,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Sheets.
	/// </summary>
	public enum SheetsFieldIndex:int
	{
		///<summary>Id. </summary>
		Id,
		///<summary>ShopId. </summary>
		ShopId,
		///<summary>StatusId. </summary>
		StatusId,
		///<summary>OrderId. </summary>
		OrderId,
		///<summary>Date. </summary>
		Date,
		///<summary>Name. </summary>
		Name,
		///<summary>Phone. </summary>
		Phone,
		///<summary>Address. </summary>
		Address,
		///<summary>Product. </summary>
		Product,
		///<summary>Link. </summary>
		Link,
		///<summary>Error. </summary>
		Error,
		///<summary>Size. </summary>
		Size,
		///<summary>Color. </summary>
		Color,
		///<summary>Note. </summary>
		Note,
		///<summary>ProductOther. </summary>
		ProductOther,
		///<summary>ProcessId. </summary>
		ProcessId,
		///<summary>ProcessLog. </summary>
		ProcessLog,
		///<summary>ProcessDate. </summary>
		ProcessDate,
		///<summary>ProcessCall. </summary>
		ProcessCall,
		///<summary>ProcessNote. </summary>
		ProcessNote,
		///<summary>ProcessProduct. </summary>
		ProcessProduct,
		///<summary>UserId. </summary>
		UserId,
		///<summary>Revenue. </summary>
		Revenue,
		///<summary>CancelDate. </summary>
		CancelDate,
		///<summary>AppleDate. </summary>
		AppleDate,
		///<summary>Location. </summary>
		Location,
		///<summary>Category. </summary>
		Category,
		///<summary>Category2. </summary>
		Category2,
		///<summary>Category3. </summary>
		Category3,
		///<summary>ProcessCode. </summary>
		ProcessCode,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Shops.
	/// </summary>
	public enum ShopsFieldIndex:int
	{
		///<summary>Id. </summary>
		Id,
		///<summary>Name. </summary>
		Name,
		///<summary>ApiKey. </summary>
		ApiKey,
		///<summary>WarehouseId. </summary>
		WarehouseId,
		///<summary>WarehouseInfo. </summary>
		WarehouseInfo,
		///<summary>SpreadsheetId. </summary>
		SpreadsheetId,
		///<summary>SpreadsheetTab. </summary>
		SpreadsheetTab,
		///<summary>SheetColumns. </summary>
		SheetColumns,
		///<summary>OrderPage. </summary>
		OrderPage,
		///<summary>ProductOtherInNote. </summary>
		ProductOtherInNote,
		///<summary>ProductErrorToNote. </summary>
		ProductErrorToNote,
		///<summary>ProductFindByName. </summary>
		ProductFindByName,
		///<summary>ProductToOrderEmpty. </summary>
		ProductToOrderEmpty,
		/// <summary></summary>
		AmountOfFields
	}


	/// <summary>
	/// Index enum to fast-access EntityFields in the IEntityFields collection for the entity: Users.
	/// </summary>
	public enum UsersFieldIndex:int
	{
		///<summary>UserId. </summary>
		UserId,
		///<summary>Username. </summary>
		Username,
		///<summary>Password. </summary>
		Password,
		///<summary>Email. </summary>
		Email,
		///<summary>Name. </summary>
		Name,
		///<summary>Avatar. </summary>
		Avatar,
		///<summary>RoleId. </summary>
		RoleId,
		///<summary>Online. </summary>
		Online,
		///<summary>IsAds. </summary>
		IsAds,
		///<summary>IsReport. </summary>
		IsReport,
		/// <summary></summary>
		AmountOfFields
	}





	/// <summary>
	/// Enum definition for all the entity types defined in this namespace. Used by the entityfields factory.
	/// </summary>
	public enum EntityType:int
	{
		///<summary>Ads</summary>
		AdsEntity,
		///<summary>Colors</summary>
		ColorsEntity,
		///<summary>Configs</summary>
		ConfigsEntity,
		///<summary>Customers</summary>
		CustomersEntity,
		///<summary>Orders</summary>
		OrdersEntity,
		///<summary>Products</summary>
		ProductsEntity,
		///<summary>Sheets</summary>
		SheetsEntity,
		///<summary>Shops</summary>
		ShopsEntity,
		///<summary>Users</summary>
		UsersEntity
	}




	#region Custom ConstantsEnums Code
	
	// __LLBLGENPRO_USER_CODE_REGION_START CustomUserConstants
	// __LLBLGENPRO_USER_CODE_REGION_END
	#endregion

	#region Included code

	#endregion
}


