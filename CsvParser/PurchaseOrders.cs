using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CsvParser
{

        [XmlRoot(ElementName = "Address")]
        public class Address
        {
            [XmlElement(ElementName = "Name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "Street")]
            public string Street { get; set; }
            [XmlElement(ElementName = "City")]
            public string City { get; set; }
            [XmlElement(ElementName = "State")]
            public string State { get; set; }
            [XmlElement(ElementName = "Zip")]
            public string Zip { get; set; }
            [XmlElement(ElementName = "Country")]
            public string Country { get; set; }
            [XmlAttribute(AttributeName = "Type")]
            public string Type { get; set; }
        }

        [XmlRoot(ElementName = "Item")]
        public class Item
        {
            [XmlElement(ElementName = "ProductName")]
            public string ProductName { get; set; }
            [XmlElement(ElementName = "Quantity")]
            public string Quantity { get; set; }
            [XmlElement(ElementName = "USPrice")]
            public string USPrice { get; set; }
            [XmlElement(ElementName = "Comment")]
            public string Comment { get; set; }
            [XmlAttribute(AttributeName = "PartNumber")]
            public string PartNumber { get; set; }
            [XmlElement(ElementName = "ShipDate")]
            public string ShipDate { get; set; }
        }

        [XmlRoot(ElementName = "Items")]
        public class Items
        {
            [XmlElement(ElementName = "Item")]
            public List<Item> Item { get; set; }
        }

        [XmlRoot(ElementName = "PurchaseOrder")]
        public class PurchaseOrder
        {
            [XmlElement(ElementName = "Address")]
            public List<Address> Address { get; set; }
            [XmlElement(ElementName = "DeliveryNotes")]
            public string DeliveryNotes { get; set; }
            [XmlElement(ElementName = "Items")]
            public Items Items { get; set; }
            [XmlAttribute(AttributeName = "PurchaseOrderNumber")]
            public string PurchaseOrderNumber { get; set; }
            [XmlAttribute(AttributeName = "OrderDate")]
            public string OrderDate { get; set; }
        }

        [XmlRoot(ElementName = "PurchaseOrders")]
        public class PurchaseOrders
        {
            [XmlElement(ElementName = "PurchaseOrder")]
            public List<PurchaseOrder> PurchaseOrder { get; set; }
        }
}
