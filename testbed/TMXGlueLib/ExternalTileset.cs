﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5448
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
using System.Collections.Generic;
using TMXGlueLib;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class tileset {

    private tilesetImage[] imageField;
    
    private string nameField;
    
    private int tilewidthField;
    
    private int tileheightField;
    
    private int spacingField;
    
    private int marginField;
    [System.Xml.Serialization.XmlElementAttribute("tile", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public List<mapTilesetTile> tile = new List<mapTilesetTile>();
    //{
    //    get
    //    {
    //        return this.tileField;
    //    }
    //    set
    //    {
    //        if (this.tileField != null && this.tileField.Length > 0)
    //        {
    //            return;
    //        }
    //        else
    //        {
    //            this.tileField = value;
    //        }
    //    }
    //}
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("image", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public tilesetImage[] image {
        get {
            return this.imageField;
        }
        set {
            this.imageField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int tilewidth {
        get {
            return this.tilewidthField;
        }
        set {
            this.tilewidthField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int tileheight {
        get {
            return this.tileheightField;
        }
        set {
            this.tileheightField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int spacing {
        get {
            return this.spacingField;
        }
        set {
            this.spacingField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int margin {
        get {
            return this.marginField;
        }
        set {
            this.marginField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class tilesetImage {
    
    private string sourceField;
    
    private int widthField;
    
    private int heightField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string source {
        get {
            return this.sourceField;
        }
        set {
            this.sourceField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int width {
        get {
            return this.widthField;
        }
        set {
            this.widthField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int height {
        get {
            return this.heightField;
        }
        set {
            this.heightField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class NewDataSet {
    
    private tileset[] itemsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("tileset")]
    public tileset[] Items {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }
}
