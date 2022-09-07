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
using System.Collections.Generic;
using Onetez.Dal.HelperClasses;

using Onetez.Dal.EntityClasses;
using Onetez.Dal.RelationClasses;
using Onetez.Dal.DaoClasses;
using Onetez.Dal.CollectionClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Dal.FactoryClasses
{
	
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>general base class for the generated factories</summary>
	[Serializable]
	public partial class EntityFactoryBase : EntityFactoryCore
	{
		private string _entityName;
		private Onetez.Dal.EntityType _typeOfEntity;
		
		/// <summary>CTor</summary>
		/// <param name="entityName">Name of the entity.</param>
		/// <param name="typeOfEntity">The type of entity.</param>
		public EntityFactoryBase(string entityName, Onetez.Dal.EntityType typeOfEntity)
		{
			_entityName = entityName;
			_typeOfEntity = typeOfEntity;
		}

		/// <summary>Creates a new entity instance using the GeneralEntityFactory in the generated code, using the passed in entitytype value</summary>
		/// <param name="entityTypeValue">The entity type value of the entity to create an instance for.</param>
		/// <returns>new IEntity instance</returns>
		public override IEntity CreateEntityFromEntityTypeValue(int entityTypeValue)
		{
			return GeneralEntityFactory.Create((Onetez.Dal.EntityType)entityTypeValue);
		}
		
		/// <summary>Creates, using the generated EntityFieldsFactory, the IEntityFields object for the entity to create. </summary>
		/// <returns>Empty IEntityFields object.</returns>
		public override IEntityFields CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(_typeOfEntity);
		}

		/// <summary>Creates the relations collection to the entity to join all targets so this entity can be fetched. </summary>
		/// <param name="objectAlias">The object alias to use for the elements in the relations.</param>
		/// <returns>null if the entity isn't in a hierarchy of type TargetPerEntity, otherwise the relations collection needed to join all targets together to fetch all subtypes of this entity and this entity itself</returns>
		public override IRelationCollection CreateHierarchyRelations(string objectAlias) 
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetHierarchyRelations(_entityName, objectAlias);
		}

		/// <summary>This method retrieves, using the InheritanceInfoprovider, the factory for the entity represented by the values passed in.</summary>
		/// <param name="fieldValues">Field values read from the db, to determine which factory to return, based on the field values passed in.</param>
		/// <param name="entityFieldStartIndexesPerEntity">indexes into values where per entity type their own fields start.</param>
		/// <returns>the factory for the entity which is represented by the values passed in.</returns>
		public override IEntityFactory GetEntityFactory(object[] fieldValues, Dictionary<string, int> entityFieldStartIndexesPerEntity)
		{
			IEntityFactory toReturn = (IEntityFactory)InheritanceInfoProviderSingleton.GetInstance().GetEntityFactory(_entityName, fieldValues, entityFieldStartIndexesPerEntity);
			if(toReturn == null)
			{
				toReturn = this;
			}
			return toReturn;
		}
						
		/// <summary>Creates a new entity collection for the entity of this factory.</summary>
		/// <returns>ready to use new entity collection, typed.</returns>
		public override IEntityCollection CreateEntityCollection()
		{
			return GeneralEntityCollectionFactory.Create(_typeOfEntity);
		}
		
		/// <summary>returns the name of the entity this factory is for, e.g. "EmployeeEntity"</summary>
		public override string ForEntityName 
		{ 
			get { return _entityName; }
		}
	}
	
	/// <summary>Factory to create new, empty AdsEntity objects.</summary>
	[Serializable]
	public partial class AdsEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public AdsEntityFactory() : base("AdsEntity", Onetez.Dal.EntityType.AdsEntity) { }

		/// <summary>Creates a new, empty AdsEntity object.</summary>
		/// <returns>A new, empty AdsEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new AdsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAds
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AdsEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAdsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}
	
	/// <summary>Factory to create new, empty ColorsEntity objects.</summary>
	[Serializable]
	public partial class ColorsEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public ColorsEntityFactory() : base("ColorsEntity", Onetez.Dal.EntityType.ColorsEntity) { }

		/// <summary>Creates a new, empty ColorsEntity object.</summary>
		/// <returns>A new, empty ColorsEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new ColorsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewColors
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ColorsEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewColorsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}
	
	/// <summary>Factory to create new, empty ConfigsEntity objects.</summary>
	[Serializable]
	public partial class ConfigsEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public ConfigsEntityFactory() : base("ConfigsEntity", Onetez.Dal.EntityType.ConfigsEntity) { }

		/// <summary>Creates a new, empty ConfigsEntity object.</summary>
		/// <returns>A new, empty ConfigsEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new ConfigsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewConfigs
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ConfigsEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewConfigsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}
	
	/// <summary>Factory to create new, empty CustomersEntity objects.</summary>
	[Serializable]
	public partial class CustomersEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public CustomersEntityFactory() : base("CustomersEntity", Onetez.Dal.EntityType.CustomersEntity) { }

		/// <summary>Creates a new, empty CustomersEntity object.</summary>
		/// <returns>A new, empty CustomersEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new CustomersEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewCustomers
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new CustomersEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewCustomersUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}
	
	/// <summary>Factory to create new, empty OrdersEntity objects.</summary>
	[Serializable]
	public partial class OrdersEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public OrdersEntityFactory() : base("OrdersEntity", Onetez.Dal.EntityType.OrdersEntity) { }

		/// <summary>Creates a new, empty OrdersEntity object.</summary>
		/// <returns>A new, empty OrdersEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new OrdersEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrders
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OrdersEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrdersUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}
	
	/// <summary>Factory to create new, empty ProductsEntity objects.</summary>
	[Serializable]
	public partial class ProductsEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public ProductsEntityFactory() : base("ProductsEntity", Onetez.Dal.EntityType.ProductsEntity) { }

		/// <summary>Creates a new, empty ProductsEntity object.</summary>
		/// <returns>A new, empty ProductsEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new ProductsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewProducts
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ProductsEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewProductsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}
	
	/// <summary>Factory to create new, empty SheetsEntity objects.</summary>
	[Serializable]
	public partial class SheetsEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public SheetsEntityFactory() : base("SheetsEntity", Onetez.Dal.EntityType.SheetsEntity) { }

		/// <summary>Creates a new, empty SheetsEntity object.</summary>
		/// <returns>A new, empty SheetsEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new SheetsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSheets
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new SheetsEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSheetsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}
	
	/// <summary>Factory to create new, empty ShopsEntity objects.</summary>
	[Serializable]
	public partial class ShopsEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public ShopsEntityFactory() : base("ShopsEntity", Onetez.Dal.EntityType.ShopsEntity) { }

		/// <summary>Creates a new, empty ShopsEntity object.</summary>
		/// <returns>A new, empty ShopsEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new ShopsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShops
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShopsEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}
	
	/// <summary>Factory to create new, empty UsersEntity objects.</summary>
	[Serializable]
	public partial class UsersEntityFactory : EntityFactoryBase {
		/// <summary>CTor</summary>
		public UsersEntityFactory() : base("UsersEntity", Onetez.Dal.EntityType.UsersEntity) { }

		/// <summary>Creates a new, empty UsersEntity object.</summary>
		/// <returns>A new, empty UsersEntity object.</returns>
		public override IEntity Create() {
			IEntity toReturn = new UsersEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUsers
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UsersEntity instance and will set the Fields object of the new IEntity instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields object for the new IEntity to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields object) IEntity object</returns>
		public override IEntity Create(IEntityFields fields) {
			IEntity toReturn = Create();
			toReturn.Fields = fields;
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUsersUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}

		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new entity collection objects</summary>
	[Serializable]
	public partial class GeneralEntityCollectionFactory
	{
		/// <summary>Creates a new entity collection</summary>
		/// <param name="typeToUse">The entity type to create the collection for.</param>
		/// <returns>A new entity collection object.</returns>
		public static IEntityCollection Create(Onetez.Dal.EntityType typeToUse)
		{
			switch(typeToUse)
			{
				case Onetez.Dal.EntityType.AdsEntity:
					return new AdsCollection();
				case Onetez.Dal.EntityType.ColorsEntity:
					return new ColorsCollection();
				case Onetez.Dal.EntityType.ConfigsEntity:
					return new ConfigsCollection();
				case Onetez.Dal.EntityType.CustomersEntity:
					return new CustomersCollection();
				case Onetez.Dal.EntityType.OrdersEntity:
					return new OrdersCollection();
				case Onetez.Dal.EntityType.ProductsEntity:
					return new ProductsCollection();
				case Onetez.Dal.EntityType.SheetsEntity:
					return new SheetsCollection();
				case Onetez.Dal.EntityType.ShopsEntity:
					return new ShopsCollection();
				case Onetez.Dal.EntityType.UsersEntity:
					return new UsersCollection();
				default:
					return null;
			}
		}		
	}
	
	/// <summary>Factory to create new, empty Entity objects based on the entity type specified. Uses entity specific factory objects</summary>
	[Serializable]
	public partial class GeneralEntityFactory
	{
		/// <summary>Creates a new, empty Entity object of the type specified</summary>
		/// <param name="entityTypeToCreate">The entity type to create.</param>
		/// <returns>A new, empty Entity object.</returns>
		public static IEntity Create(Onetez.Dal.EntityType entityTypeToCreate)
		{
			IEntityFactory factoryToUse = null;
			switch(entityTypeToCreate)
			{
				case Onetez.Dal.EntityType.AdsEntity:
					factoryToUse = new AdsEntityFactory();
					break;
				case Onetez.Dal.EntityType.ColorsEntity:
					factoryToUse = new ColorsEntityFactory();
					break;
				case Onetez.Dal.EntityType.ConfigsEntity:
					factoryToUse = new ConfigsEntityFactory();
					break;
				case Onetez.Dal.EntityType.CustomersEntity:
					factoryToUse = new CustomersEntityFactory();
					break;
				case Onetez.Dal.EntityType.OrdersEntity:
					factoryToUse = new OrdersEntityFactory();
					break;
				case Onetez.Dal.EntityType.ProductsEntity:
					factoryToUse = new ProductsEntityFactory();
					break;
				case Onetez.Dal.EntityType.SheetsEntity:
					factoryToUse = new SheetsEntityFactory();
					break;
				case Onetez.Dal.EntityType.ShopsEntity:
					factoryToUse = new ShopsEntityFactory();
					break;
				case Onetez.Dal.EntityType.UsersEntity:
					factoryToUse = new UsersEntityFactory();
					break;
			}
			IEntity toReturn = null;
			if(factoryToUse != null)
			{
				toReturn = factoryToUse.Create();
			}
			return toReturn;
		}		
	}
	
	/// <summary>Class which is used to obtain the entity factory based on the .NET type of the entity. </summary>
	[Serializable]
	public static class EntityFactoryFactory
	{
#if CF
		/// <summary>Gets the factory of the entity with the Onetez.Dal.EntityType specified</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>factory to use or null if not found</returns>
		public static IEntityFactory GetFactory(Onetez.Dal.EntityType typeOfEntity)
		{
			return GeneralEntityFactory.Create(typeOfEntity).GetEntityFactory();
		}
#else
		private static Dictionary<Type, IEntityFactory> _factoryPerType = new Dictionary<Type, IEntityFactory>();

		/// <summary>Initializes the <see cref="EntityFactoryFactory"/> class.</summary>
		static EntityFactoryFactory()
		{
			Array entityTypeValues = Enum.GetValues(typeof(Onetez.Dal.EntityType));
			foreach(int entityTypeValue in entityTypeValues)
			{
				IEntity dummy = GeneralEntityFactory.Create((Onetez.Dal.EntityType)entityTypeValue);
				_factoryPerType.Add(dummy.GetType(), dummy.GetEntityFactory());
			}
		}

		/// <summary>Gets the factory of the entity with the .NET type specified</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>factory to use or null if not found</returns>
		public static IEntityFactory GetFactory(Type typeOfEntity)
		{
			IEntityFactory toReturn = null;
			_factoryPerType.TryGetValue(typeOfEntity, out toReturn);
			return toReturn;
		}

		/// <summary>Gets the factory of the entity with the Onetez.Dal.EntityType specified</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>factory to use or null if not found</returns>
		public static IEntityFactory GetFactory(Onetez.Dal.EntityType typeOfEntity)
		{
			return GetFactory(GeneralEntityFactory.Create(typeOfEntity).GetType());
		}
#endif
	}
	
	/// <summary>Element creator for creating project elements from somewhere else, like inside Linq providers.</summary>
	public class ElementCreator : ElementCreatorBase, IElementCreator
	{
		/// <summary>Gets the factory of the Entity type with the Onetez.Dal.EntityType value passed in</summary>
		/// <param name="entityTypeValue">The entity type value.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		public IEntityFactory GetFactory(int entityTypeValue)
		{
			return (IEntityFactory)this.GetFactoryImpl(entityTypeValue);
		}

		/// <summary>Gets the factory of the Entity type with the .NET type passed in</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		public IEntityFactory GetFactory(Type typeOfEntity)
		{
			return (IEntityFactory)this.GetFactoryImpl(typeOfEntity);
		}

		/// <summary>Creates a new resultset fields object with the number of field slots reserved as specified</summary>
		/// <param name="numberOfFields">The number of fields.</param>
		/// <returns>ready to use resultsetfields object</returns>
		public IEntityFields CreateResultsetFields(int numberOfFields)
		{
			return new ResultsetFields(numberOfFields);
		}
		
		/// <summary>Gets an instance of the TypedListDAO class to execute dynamic lists and projections.</summary>
		/// <returns>ready to use typedlistDAO</returns>
		public IDao GetTypedListDao()
		{
			return new TypedListDAO();
		}
		
		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand)
		{
			return new DynamicRelation(leftOperand);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperand">The right operand.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand, JoinHint joinType, DerivedTableDefinition rightOperand, IPredicate onClause)
		{
			return new DynamicRelation(leftOperand, joinType, rightOperand, onClause);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperandEntityName">Name of the entity, which is used as the right operand.</param>
		/// <param name="aliasRightOperand">The alias of the right operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand, JoinHint joinType, string rightOperandEntityName, string aliasRightOperand, IPredicate onClause)
		{
			return new DynamicRelation(leftOperand, joinType, (Onetez.Dal.EntityType)Enum.Parse(typeof(Onetez.Dal.EntityType), rightOperandEntityName, false), aliasRightOperand, onClause);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperandEntityName">Name of the entity which is used as the left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperandEntityName">Name of the entity, which is used as the right operand.</param>
		/// <param name="aliasLeftOperand">The alias of the left operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="aliasRightOperand">The alias of the right operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(string leftOperandEntityName, JoinHint joinType, string rightOperandEntityName, string aliasLeftOperand, string aliasRightOperand, IPredicate onClause)
		{
			return new DynamicRelation((Onetez.Dal.EntityType)Enum.Parse(typeof(Onetez.Dal.EntityType), leftOperandEntityName, false), joinType, (Onetez.Dal.EntityType)Enum.Parse(typeof(Onetez.Dal.EntityType), rightOperandEntityName, false), aliasLeftOperand, aliasRightOperand, onClause);
		}
				
		/// <summary>Obtains the inheritance info provider instance from the singleton </summary>
		/// <returns>The singleton instance of the inheritance info provider</returns>
		public override IInheritanceInfoProvider ObtainInheritanceInfoProviderInstance()
		{
			return InheritanceInfoProviderSingleton.GetInstance();
		}

		/// <summary>Implementation of the routine which gets the factory of the Entity type with the Onetez.Dal.EntityType value passed in</summary>
		/// <param name="entityTypeValue">The entity type value.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		protected override IEntityFactoryCore GetFactoryImpl(int entityTypeValue)
		{
			return EntityFactoryFactory.GetFactory((Onetez.Dal.EntityType)entityTypeValue);
		}
#if !CF		
		/// <summary>Implementation of the routine which gets the factory of the Entity type with the .NET type passed in</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		protected override IEntityFactoryCore GetFactoryImpl(Type typeOfEntity)
		{
			return EntityFactoryFactory.GetFactory(typeOfEntity);
		}
#endif
	}
}
