using System;

using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace Products.Entities.Models
{
  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  [Table(IdentityMethod=IdentityMethod.Guid)]
  public partial class Product : Entity<System.Guid>
  {
    #region Fields
  
    private string _name;
    private double _price;
    private int _quantity;

    #pragma warning disable 649  // "Field is never assigned to" - LightSpeed assigns these fields internally
    private readonly System.DateTime _createdOn;
    private readonly System.DateTime _updatedOn;
    #pragma warning restore 649    

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the Name entity attribute.</summary>
    public const string NameField = "Name";
    /// <summary>Identifies the Price entity attribute.</summary>
    public const string PriceField = "Price";
    /// <summary>Identifies the Quantity entity attribute.</summary>
    public const string QuantityField = "Quantity";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Product")]
    private readonly EntityCollection<ProductColor> _productColors = new EntityCollection<ProductColor>();

    private ThroughAssociation<ProductColor, Color> _colors;

    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public EntityCollection<ProductColor> ProductColors
    {
      get { return Get(_productColors); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public ThroughAssociation<ProductColor, Color> Colors
    {
      get
      {
        if (_colors == null)
        {
          _colors = new ThroughAssociation<ProductColor, Color>(_productColors);
        }
        return Get(_colors);
      }
    }
    

    [System.Diagnostics.DebuggerNonUserCode]
    public string Name
    {
      get { return Get(ref _name, "Name"); }
      set { Set(ref _name, value, "Name"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public double Price
    {
      get { return Get(ref _price, "Price"); }
      set { Set(ref _price, value, "Price"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public int Quantity
    {
      get { return Get(ref _quantity, "Quantity"); }
      set { Set(ref _quantity, value, "Quantity"); }
    }
    /// <summary>Gets the time the entity was created</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public System.DateTime CreatedOn
    {
      get { return _createdOn; }   
    }

    /// <summary>Gets the time the entity was last updated</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public System.DateTime UpdatedOn
    {
      get { return _updatedOn; }   
    }

    #endregion
  }


  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  [Table(IdentityMethod=IdentityMethod.Guid)]
  public partial class Color : Entity<System.Guid>
  {
    #region Fields
  
    private string _name;

    #pragma warning disable 649  // "Field is never assigned to" - LightSpeed assigns these fields internally
    private readonly System.DateTime _createdOn;
    private readonly System.DateTime _updatedOn;
    #pragma warning restore 649    

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the Name entity attribute.</summary>
    public const string NameField = "Name";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Color")]
    private readonly EntityCollection<ProductColor> _productColors = new EntityCollection<ProductColor>();

    private ThroughAssociation<ProductColor, Product> _product;

    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public EntityCollection<ProductColor> ProductColors
    {
      get { return Get(_productColors); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public ThroughAssociation<ProductColor, Product> Product
    {
      get
      {
        if (_product == null)
        {
          _product = new ThroughAssociation<ProductColor, Product>(_productColors);
        }
        return Get(_product);
      }
    }
    

    [System.Diagnostics.DebuggerNonUserCode]
    public string Name
    {
      get { return Get(ref _name, "Name"); }
      set { Set(ref _name, value, "Name"); }
    }
    /// <summary>Gets the time the entity was created</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public System.DateTime CreatedOn
    {
      get { return _createdOn; }   
    }

    /// <summary>Gets the time the entity was last updated</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public System.DateTime UpdatedOn
    {
      get { return _updatedOn; }   
    }

    #endregion
  }


  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  public partial class ProductColor : Entity<int>
  {
    #region Fields
  
    private System.Guid _productId;
    private System.Guid _colorId;

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the ProductId entity attribute.</summary>
    public const string ProductIdField = "ProductId";
    /// <summary>Identifies the ColorId entity attribute.</summary>
    public const string ColorIdField = "ColorId";


    #endregion
    
    #region Relationships

    [ReverseAssociation("ProductColors")]
    private readonly EntityHolder<Product> _product = new EntityHolder<Product>();
    [ReverseAssociation("ProductColors")]
    private readonly EntityHolder<Color> _color = new EntityHolder<Color>();


    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public Product Product
    {
      get { return Get(_product); }
      set { Set(_product, value); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public Color Color
    {
      get { return Get(_color); }
      set { Set(_color, value); }
    }


    [System.Diagnostics.DebuggerNonUserCode]
    public System.Guid ProductId
    {
      get { return Get(ref _productId, "ProductId"); }
      set { Set(ref _productId, value, "ProductId"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public System.Guid ColorId
    {
      get { return Get(ref _colorId, "ColorId"); }
      set { Set(ref _colorId, value, "ColorId"); }
    }

    #endregion
  }



  /// <summary>
  /// Provides a strong-typed unit of work for working with the Db model.
  /// </summary>
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  public partial class DbUnitOfWork : Mindscape.LightSpeed.UnitOfWork
  {

    public System.Linq.IQueryable<Product> Products
    {
      get { return this.Query<Product>(); }
    }
    
    public System.Linq.IQueryable<Color> Colors
    {
      get { return this.Query<Color>(); }
    }
    
    public System.Linq.IQueryable<ProductColor> ProductColors
    {
      get { return this.Query<ProductColor>(); }
    }
    
  }

}
