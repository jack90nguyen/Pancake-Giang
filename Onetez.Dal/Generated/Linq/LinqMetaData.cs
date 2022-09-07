///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: Wednesday, August 31, 2022 08:57:24
// Code is generated using templates: SD.TemplateBindings.Linq
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

using Onetez.Dal;
using Onetez.Dal.DaoClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.FactoryClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.RelationClasses;

namespace Onetez.Dal.Linq
{
	/// <summary>Meta-data class for the construction of Linq queries which are to be executed using LLBLGen Pro code.</summary>
	public partial class LinqMetaData : ILinqMetaData
	{
		#region Class Member Declarations
		private ITransaction _transactionToUse;
		private FunctionMappingStore _customFunctionMappings;
		private Context _contextToUse;
		#endregion
		
		/// <summary>CTor. Using this ctor will leave the transaction object to use empty. This is ok if you're not executing queries created with this
		/// meta data inside a transaction. If you're executing the queries created with this meta-data inside a transaction, either set the Transaction property
		/// on the IQueryable.Provider instance of the created LLBLGenProQuery object prior to execution or use the ctor which accepts a transaction object.</summary>
		public LinqMetaData() : this(null, null)
		{
		}
		
		/// <summary>CTor. If you're executing the queries created with this meta-data inside a transaction, pass a live ITransaction object to this ctor.</summary>
		/// <param name="transactionToUse">the transaction to use in queries created with this meta-data</param>
		/// <remarks> Be aware that the ITransaction object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public LinqMetaData(ITransaction transactionToUse) : this(transactionToUse, null)
		{
		}
		
		/// <summary>CTor. If you're executing the queries created with this meta-data inside a transaction, pass a live ITransaction object to this ctor.</summary>
		/// <param name="transactionToUse">the transaction to use in queries created with this meta-data</param>
		/// <param name="customFunctionMappings">The custom function mappings to use. These take higher precedence than the ones in the DQE to use.</param>
		/// <remarks> Be aware that the ITransaction object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public LinqMetaData(ITransaction transactionToUse, FunctionMappingStore customFunctionMappings)
		{
			_transactionToUse = transactionToUse;
			_customFunctionMappings = customFunctionMappings;
		}
		
		/// <summary>returns the datasource to use in a Linq query for the entity type specified</summary>
		/// <param name="typeOfEntity">the type of the entity to get the datasource for</param>
		/// <returns>the requested datasource</returns>
		public IDataSource GetQueryableForEntity(int typeOfEntity)
		{
			IDataSource toReturn = null;
			switch((Onetez.Dal.EntityType)typeOfEntity)
			{
				case Onetez.Dal.EntityType.AdsEntity:
					toReturn = this.Ads;
					break;
				case Onetez.Dal.EntityType.ColorsEntity:
					toReturn = this.Colors;
					break;
				case Onetez.Dal.EntityType.ConfigsEntity:
					toReturn = this.Configs;
					break;
				case Onetez.Dal.EntityType.CustomersEntity:
					toReturn = this.Customers;
					break;
				case Onetez.Dal.EntityType.OrdersEntity:
					toReturn = this.Orders;
					break;
				case Onetez.Dal.EntityType.ProductsEntity:
					toReturn = this.Products;
					break;
				case Onetez.Dal.EntityType.SheetsEntity:
					toReturn = this.Sheets;
					break;
				case Onetez.Dal.EntityType.ShopsEntity:
					toReturn = this.Shops;
					break;
				case Onetez.Dal.EntityType.UsersEntity:
					toReturn = this.Users;
					break;
				default:
					toReturn = null;
					break;
			}
			return toReturn;
		}

		/// <summary>returns the datasource to use in a Linq query when targeting AdsEntity instances in the database.</summary>
		public DataSource<AdsEntity> Ads
		{
			get { return new DataSource<AdsEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		/// <summary>returns the datasource to use in a Linq query when targeting ColorsEntity instances in the database.</summary>
		public DataSource<ColorsEntity> Colors
		{
			get { return new DataSource<ColorsEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		/// <summary>returns the datasource to use in a Linq query when targeting ConfigsEntity instances in the database.</summary>
		public DataSource<ConfigsEntity> Configs
		{
			get { return new DataSource<ConfigsEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		/// <summary>returns the datasource to use in a Linq query when targeting CustomersEntity instances in the database.</summary>
		public DataSource<CustomersEntity> Customers
		{
			get { return new DataSource<CustomersEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		/// <summary>returns the datasource to use in a Linq query when targeting OrdersEntity instances in the database.</summary>
		public DataSource<OrdersEntity> Orders
		{
			get { return new DataSource<OrdersEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		/// <summary>returns the datasource to use in a Linq query when targeting ProductsEntity instances in the database.</summary>
		public DataSource<ProductsEntity> Products
		{
			get { return new DataSource<ProductsEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		/// <summary>returns the datasource to use in a Linq query when targeting SheetsEntity instances in the database.</summary>
		public DataSource<SheetsEntity> Sheets
		{
			get { return new DataSource<SheetsEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		/// <summary>returns the datasource to use in a Linq query when targeting ShopsEntity instances in the database.</summary>
		public DataSource<ShopsEntity> Shops
		{
			get { return new DataSource<ShopsEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		/// <summary>returns the datasource to use in a Linq query when targeting UsersEntity instances in the database.</summary>
		public DataSource<UsersEntity> Users
		{
			get { return new DataSource<UsersEntity>(_transactionToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		#region Class Property Declarations
		/// <summary> Gets / sets the ITransaction to use for the queries created with this meta data object.</summary>
		/// <remarks> Be aware that the ITransaction object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public ITransaction TransactionToUse
		{
			get { return _transactionToUse;}
			set { _transactionToUse = value;}
		}

		/// <summary>Gets or sets the custom function mappings to use. These take higher precedence than the ones in the DQE to use</summary>
		public FunctionMappingStore CustomFunctionMappings
		{
			get { return _customFunctionMappings; }
			set { _customFunctionMappings = value; }
		}
		
		/// <summary>Gets or sets the Context instance to use for entity fetches.</summary>
		public Context ContextToUse
		{
			get { return _contextToUse;}
			set { _contextToUse = value;}
		}
		#endregion
	}
}