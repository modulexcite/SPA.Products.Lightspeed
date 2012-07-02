namespace Products.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mindscape.LightSpeed;
    using Products.Entities.DTO;
    using Generic.Util.Logging;

    public class ProductRepository : RepositoryBase, IProductRepository
    {
        private readonly ILogger _logger;

        public ProductRepository(UnitOfWorkScopeBase<DbUnitOfWork> unitOfWorkScope, ILogger logger)
            : base(unitOfWorkScope)
        {
            _logger = logger;
        }

        public IEnumerable<Product> Products
        {
            get
            {
                try
                {
                    return _unitOfWorkScope.Current.Find<Product>();
                }
                catch (Exception e)
                {
                    _logger.Error("Problem with DB connectivity, can't read products.", e);
                    return null;
                }
            }
        }

        public IEnumerable<Color> Colors
        {
            get
            {
                try {
                    return _unitOfWorkScope.Current.Find<Color>();
                }
                catch (Exception e)
                {
                    _logger.Error("Problem with DB connectivity, can't read products.", e);
                    return null;
                }
            }
        }

        public T GetEntity<T>(string guid) where T : Entity
        {
            _logger.Info(string.Format("Retrieving {0}.", guid));
            return _unitOfWorkScope.Current.FindById<T>(new Guid(guid));
        }


        public bool DeleteEntity<T>(string guid) where T : Entity
        {
            _logger.Info(string.Format("Deleting {0}.", guid));
            try
            {
                var entity = _unitOfWorkScope.Current.FindById<T>(new Guid(guid));
                _unitOfWorkScope.Current.Remove(entity);
                _unitOfWorkScope.Current.SaveChanges();
                _logger.Info(string.Format("Deletion of {0} successful.", guid));

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }

        }

        public Guid? AddProduct(ProductDTO product)
        {
            _logger.Info(string.Format("Adding new product {0}.", product.Name));
            var productEntity = _unitOfWorkScope.Current.Import<Product>(product);

            _logger.Info(string.Format("New product ID assigned {0}.", productEntity.Id));
            if (product.Colors != null)
            {
                _logger.Info(string.Format("Importing product colors into DB."));
                foreach (var usercolor in product.Colors)
                {
                    var color = _unitOfWorkScope.Current.FindById<Color>(usercolor.ID);

                    if (color == null)
                    {
                        color = _unitOfWorkScope.Current.Import<Color>(usercolor);
                    }

                    productEntity.Colors.Add(color);
                }
            }

            return AddProduct(productEntity);
        }

        public Product AddProduct(ProductDTO product, bool full)
        {
            _logger.Info(string.Format("Adding new product {0}.", product.Name));
            var productEntity = _unitOfWorkScope.Current.Import<Product>(product);

            _logger.Info(string.Format("New product ID assigned {0}.", productEntity.Id));
            if (product.Colors != null)
            {
                _logger.Info(string.Format("Importing product colors into DB."));
                foreach (var usercolor in product.Colors)
                {
                    var color = _unitOfWorkScope.Current.FindById<Color>(usercolor.ID);

                    if (color == null)
                    {
                        color = _unitOfWorkScope.Current.Import<Color>(usercolor);
                    }

                    productEntity.Colors.Add(color);
                }
            }

            try
            {
                _unitOfWorkScope.Current.Add(productEntity);
                _unitOfWorkScope.Current.SaveChanges();
                _logger.Info(string.Format("Product {0} added.", productEntity.Id));

                return productEntity;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return null;
            }
        }

        public Guid? AddProduct(Product productEntity)
        {
            try
            {
                _unitOfWorkScope.Current.Add(productEntity);
                _unitOfWorkScope.Current.SaveChanges();
                _logger.Info(string.Format("Product {0} added.", productEntity.Id));

                return productEntity.Id;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return null;
            }
        }

        public bool UpdateProduct(ProductDTO product)
        {
            _logger.Info(string.Format("Updating product {0}, {1}.",product.Name, product.ID));
            var productEntity = _unitOfWorkScope.Current.FindById<Product>(product.ID);

            productEntity.Name = product.Name;
            productEntity.Price = product.Price;
            productEntity.Quantity = product.Quantity;

            if (product.Colors != null)
            {
                productEntity.Colors.Clear();

                _logger.Info(string.Format("Updating product {0}, {1} - colors.", product.Name, product.ID));
                foreach (var usercolor in product.Colors)
                {
                    var color = _unitOfWorkScope.Current.FindById<Color>(usercolor.ID);
                    productEntity.Colors.Add(color);
                }
            }

            return UpdateProduct(productEntity);
        }

        public bool UpdateProduct(Product productEntity)
        {
            try
            {
                _unitOfWorkScope.Current.SaveChanges();
                _logger.Info(string.Format("Product {0}, {1} successfully updated.", productEntity.Name, productEntity.Id));
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }

        public Guid? AddColor(ColorDTO color)
        {
            _logger.Info(string.Format("Adding color {0}.", color.Name));
            var colorEntity = _unitOfWorkScope.Current.Import<Color>(color);
            return AddColor(colorEntity);
        }

        public Guid? AddColor(Color colorEntity)
        {
            try
            {
                _unitOfWorkScope.Current.Add(colorEntity);
                _unitOfWorkScope.Current.SaveChanges();
                _logger.Info(string.Format("Color {0} successfully added.", colorEntity.Name));
   
                return colorEntity.Id;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return null;
            }
        }
    }
}
