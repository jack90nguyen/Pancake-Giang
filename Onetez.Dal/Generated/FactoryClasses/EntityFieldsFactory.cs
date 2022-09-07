﻿///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: Wednesday, August 31, 2022 08:57:25
// Code is generated using templates: SD.TemplateBindings.SharedTemplates.NET20
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Data;
using Onetez.Dal;
using Onetez.Dal.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Dal.FactoryClasses
{
	/// <summary>
	/// Generates IEntityFields instances for different kind of Entities. 
	/// This class is generated. Do not modify.
	/// </summary>
	public partial class EntityFieldsFactory
	{
		/// <summary>
		/// Private CTor, no instantiation possible.
		/// </summary>
		private EntityFieldsFactory()
		{
		}


		/// <summary>General factory entrance method which will return an EntityFields object with the format generated by the factory specified</summary>
		/// <param name="relatedEntityType">The type of entity the fields are for</param>
		/// <returns>The IEntityFields instance requested</returns>
		public static IEntityFields CreateEntityFieldsObject(Onetez.Dal.EntityType relatedEntityType)
		{
			IEntityFields fieldsToReturn=null;
			IInheritanceInfoProvider inheritanceProvider = InheritanceInfoProviderSingleton.GetInstance();
			IFieldInfoProvider fieldProvider = FieldInfoProviderSingleton.GetInstance();
			IPersistenceInfoProvider persistenceProvider = PersistenceInfoProviderSingleton.GetInstance();
			switch(relatedEntityType)
			{
				case Onetez.Dal.EntityType.AdsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "AdsEntity");
					break;
				case Onetez.Dal.EntityType.ColorsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "ColorsEntity");
					break;
				case Onetez.Dal.EntityType.ConfigsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "ConfigsEntity");
					break;
				case Onetez.Dal.EntityType.CustomersEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "CustomersEntity");
					break;
				case Onetez.Dal.EntityType.OrdersEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "OrdersEntity");
					break;
				case Onetez.Dal.EntityType.ProductsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "ProductsEntity");
					break;
				case Onetez.Dal.EntityType.SheetsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "SheetsEntity");
					break;
				case Onetez.Dal.EntityType.ShopsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "ShopsEntity");
					break;
				case Onetez.Dal.EntityType.UsersEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, persistenceProvider, "UsersEntity");
					break;
			}
			return fieldsToReturn;
		}
		
		/// <summary>General method which will return an array of IEntityFieldCore objects, used by the InheritanceInfoProvider. Only the fields defined in the entity are returned, no inherited fields.</summary>
		/// <param name="entityName">the name of the entity to get the fields for. Example: "CustomerEntity"</param>
		/// <returns>array of IEntityFieldCore fields, defined in the entity with the name specified</returns>
		internal static IEntityFieldCore[] CreateFields(string entityName)
		{
			IFieldInfoProvider fieldProvider = FieldInfoProviderSingleton.GetInstance();
			IPersistenceInfoProvider persistenceProvider = PersistenceInfoProviderSingleton.GetInstance();
			return fieldProvider.GetEntityFieldsArray(entityName, persistenceProvider);
		}



		#region Included Code

		#endregion
	}
}