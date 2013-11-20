﻿using System;
using Merchello.Core.Configuration;
using Merchello.Core.Persistence;
using Merchello.Core.Persistence.UnitOfWork;


namespace Merchello.Core.Services
{
    /// <summary>
    /// The Merchello ServiceContext, which provides access to the following services:
    /// <see cref="ICustomerService"/>, <see cref="IItemCacheService"/>, <see cref="IProductService"/>, <see cref="IProductVariantService"/>
    /// </summary>
    public class ServiceContext : IServiceContext
    {        
        private Lazy<CustomerService> _customerService;
        private Lazy<ItemCacheService> _itemCacheService;    
        //private Lazy<InvoiceService> _invoiceService;
        private Lazy<ProductService> _productService;
        private Lazy<ProductVariantService> _productVariantService;
        //private Lazy<ShippingService> _shipmentService; 
        private Lazy<RegionService> _regionService; 
        private Lazy<WarehouseService> _warehouseService;

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbUnitOfWorkProvider"></param>
        internal ServiceContext(IDatabaseUnitOfWorkProvider dbUnitOfWorkProvider)
        {
            BuildServiceContext(dbUnitOfWorkProvider, new Lazy<RepositoryFactory>(() => new RepositoryFactory()));
        }


        /// <summary>
        /// Builds the various services
        /// </summary>
        /// <param name="dbDatabaseUnitOfWorkProvider">Database unit of work provider used by the various services</param>
        /// <param name="repositoryFactory"><see cref="RepositoryFactory"/></param>
        private void BuildServiceContext(IDatabaseUnitOfWorkProvider dbDatabaseUnitOfWorkProvider,
            Lazy<RepositoryFactory> repositoryFactory)
        {
            
            if(_customerService == null)
                _customerService = new Lazy<CustomerService>(() => new CustomerService(dbDatabaseUnitOfWorkProvider, repositoryFactory.Value));

            if(_itemCacheService == null)
                _itemCacheService = new Lazy<ItemCacheService>(() => new ItemCacheService(dbDatabaseUnitOfWorkProvider, repositoryFactory.Value));
            
            if(_productVariantService == null)
                _productVariantService = new Lazy<ProductVariantService>(() => new ProductVariantService(dbDatabaseUnitOfWorkProvider, repositoryFactory.Value));

            if(_productService == null)
                _productService = new Lazy<ProductService>(() => new ProductService(dbDatabaseUnitOfWorkProvider, repositoryFactory.Value, _productVariantService.Value));

            if(_regionService == null)
                _regionService = new Lazy<RegionService>(() => new RegionService(dbDatabaseUnitOfWorkProvider, repositoryFactory.Value, MerchelloConfiguration.Current));

            if(_warehouseService == null)
                _warehouseService = new Lazy<WarehouseService>(() => new WarehouseService(dbDatabaseUnitOfWorkProvider, repositoryFactory.Value));
        }


        #region IServiceContext Members
    
        /// <summary>
        /// Gets the <see cref="ICustomerService"/>
        /// </summary>
        public ICustomerService CustomerService
        {
            get { return _customerService.Value;  }
        }

        /// <summary>
        /// Gets the <see cref="IItemCacheService"/>
        /// </summary>
        public IItemCacheService ItemCacheService
        {
            get { return _itemCacheService.Value;  }
        }

        /// <summary>
        /// Gets the <see cref="IProductService"/>
        /// </summary>
        public IProductService ProductService
        {
            get { return _productService.Value;  }
        }

        /// <summary>
        /// Gets the <see cref="IProductVariantService"/>
        /// </summary>
        public IProductVariantService ProductVariantService
        {
            get { return _productVariantService.Value; }
        }

        /// <summary>
        /// Gets the <see cref="IRegionService"/>
        /// </summary>
        internal IRegionService RegionService
        {
            get { return _regionService.Value; }
        }

        /// <summary>
        /// Gets the <see cref="IWarehouseService"/>
        /// </summary>
        public IWarehouseService WarehouseService
        {
            get { return _warehouseService.Value; }

        }
     
        #endregion
    }
}
