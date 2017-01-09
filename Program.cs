using ConsoleApplication2.Business_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var language = "hi-IN";
            var ci = new System.Globalization.CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(ci.Name);

            //var doc = GetXDocument();
            //var doc = GetPaymentReminder(ci);
            //var doc = GetShipmentReminder(ci);
            //var doc = GetItemShippedReminder(ci);
            //var doc = GetPaymentInfoAdded();
            var doc = GetAuctionSoldSeller(ci);
            var locale = Thread.CurrentThread.CurrentUICulture.Name;
            var userCulture = new System.Globalization.CultureInfo(locale);
            //var stylesheetContent = Resource1.ResourceManager.GetString("XsltContent", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("PaymentReminder.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("ShipNowReminder.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("ItemShipped.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("PaymentInfo.Created.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("PaymentInfo.Updated.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("PaymentInfo.Deleted.Email.Body", userCulture); 
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Sold.Seller.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Sold.Bidder.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Offer.Accepted.Email.Body", userCulture); 
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Offer.Offered.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Offer.Rejected.Email.Body", userCulture);  
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Question.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Reminder.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Repost.Email.Body", userCulture);
            var stylesheetContent = Resource1.ResourceManager.GetString("Auction.WatcherRepost.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.NotSold.Email.Body", userCulture);
            //var stylesheetContent = Resource1.ResourceManager.GetString("Auction.Outbid.Email.Body", userCulture);
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true, IgnoreComments = true };

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var writer = new XmlTextWriter(sw);
            var reader = XmlReader.Create(new StringReader(doc.ToString(SaveOptions.DisableFormatting)));
            var xslt = new XslCompiledTransform();            
            xslt.Load(XmlReader.Create(new StringReader(stylesheetContent), settings)); 
            
            xslt.Transform(reader, writer);

            var content = sb.ToString();
        }

        private static XDocument GetXDocument() {
            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("bookstore",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                   new XElement("book",
                        new XAttribute("genre", "autobiography"),
                        new XAttribute("publicationdate", "1981"),
                        new XAttribute("ISBN", "1-861003-11-0"),
                        new XElement("title", "The Autobiography of Benjamin Franklin"),
                        new XElement("author",
                           new XElement("first-name", "Benjamin"),
                           new XElement("last-name", "Franklin")
                        ),
                        new XElement("price", "8.99")
                    ),

                   new XElement("book",
                        new XAttribute("genre", "novel"),
                        new XAttribute("publicationdate", "1967"),
                        new XAttribute("ISBN", "0-201-63361-2"),
                        new XElement("title", "The Confidence Man"),
                        new XElement("author",
                           new XElement("first-name", "Herman"),
                           new XElement("last-name", "Melville")
                        ),
                        new XElement("price", "11.99")
                    ),

                   new XElement("book",
                        new XAttribute("genre", "philosophy"),
                        new XAttribute("publicationdate", "1991"),
                        new XAttribute("ISBN", "1-861001-57-6"),
                        new XElement("title", "The Gorgias"),
                        new XElement("author",
                           new XElement("name", "Plato")
                        ),
                        new XElement("price", "9.99")
                    )
                )
            );
        }

        private static XDocument GetPaymentReminder(System.Globalization.CultureInfo info )
        {
            List<Item> items = new List<Item>();
            Item item = new Item { Category = "furniture", Description = "bed", Id = 1, Name = "bed 12" };
            items.Add(item);
            Invoice invoice = new Invoice { Amount = 999, DueDate = DateTime.Now, InvoiceDate = Convert.ToDateTime(DateTime.Now.ToString("d", info)), InvoiceNumber = "12121sds", item = items };
            PaymentReminder reminder = new PaymentReminder { CreatedOn = DateTime.Now, Username = "user1", PaymentReminderId = 123, invoice = invoice };

            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("PaymentReminder",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),                
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),                
                   new XElement("invoice",
                        new XElement("InvoiceDate", reminder.invoice.InvoiceDate),
                        new XElement("InvoiceNumber", reminder.invoice.InvoiceNumber),
                        new XElement("Amount",reminder.invoice.Amount)
                    ),
                   new XElement("Username", reminder.Username)
                )
            );
        }

        private static XDocument GetShipmentReminder(System.Globalization.CultureInfo info)
        {
            List<Item> items = new List<Item>();
            Item item = new Item { Category = "furniture", Description = "bed", Id = 1, Name = "bed 12", Count=2 };
            items.Add(item);
            Invoice invoice = new Invoice { Amount = 999, DueDate = DateTime.Now, InvoiceDate = Convert.ToDateTime(DateTime.Now.ToString("d", info)), InvoiceNumber = "12121sds", item = items };
            Shipment shipment = new Shipment { packagemethod = PackageMethod.Standard, ShipmentId = "1234", ShippedDate = Convert.ToDateTime(DateTime.Now.ToString("d", info)) };
            Order order = new Order { Amount = 2000, invoice = invoice, OrderDate = Convert.ToDateTime(DateTime.Now.ToString("d", info)), OrderNumber = "1234", shipment = shipment };
            ShipmentReminder reminder = new ShipmentReminder { ShipmentReminderId = 1234, Username="user123", order = order };

            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("ShipmentReminder",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XElement("order",
                        new XElement("OrderNumber", order.OrderNumber),
                        new XElement("OrderDate",order.OrderDate),
                        new XElement("invoice",
                            new XElement("InvoiceDate", reminder.order.invoice.InvoiceDate),
                            new XElement("InvoiceNumber", reminder.order.invoice.InvoiceNumber),
                            new XElement("Amount", reminder.order.invoice.Amount),
                            new XElement("item",
                            new XElement("Count", 2),
                            new XElement("Name",item.Name))
                            ),
                        new XElement("shipment",
                            new XElement("packagemethod", shipment.packagemethod))
                    ),                   
                   new XElement("Username", reminder.Username)
                )
            );
        }

        private static XDocument GetItemShippedReminder(System.Globalization.CultureInfo info)
        {
            List<Item> items = new List<Item>();
            Item item = new Item { Category = "furniture", Description = "bed", Id = 1, Name = "bed 12", Count = 2 };
            items.Add(item);
            Invoice invoice = new Invoice { Amount = 999, DueDate = DateTime.Now, InvoiceDate = Convert.ToDateTime(DateTime.Now.ToString("d", info)), InvoiceNumber = "12121sds", item = items };
            Shipment shipment = new Shipment { packagemethod = PackageMethod.Standard, ShipmentId = "1234", ShippedDate = Convert.ToDateTime(DateTime.Now.ToString("d", info)), url= "https://www.google.com" };
            Order order = new Order { Amount = 2000, invoice = invoice, OrderDate = Convert.ToDateTime(DateTime.Now.ToString("d", info)), OrderNumber = "1234", shipment = shipment };            

            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("order",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                             new XElement("Username", order.Username),
                             new XElement("OrderNumber",order.OrderNumber),
                             new XElement("url",order.shipment.url)
                )
            );
        }

        private static XDocument GetPaymentInfoAdded()
        {   
            PaymentInfo payment = new PaymentInfo {  CardNumber=12121, CardType= CardType.Visa, last4CardNumber= "xxxxxxx2345", Username="user1"};
            
            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("payment",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                             new XElement("Username", payment.Username),
                             new XElement("CardType", payment.CardType),
                             new XElement("last4CardNumber", payment.last4CardNumber)
                )
            );
        }

        private static XDocument GetAuctionSoldSeller(System.Globalization.CultureInfo info)
        {
            Item item = new Item { Category = "furniture", Description = "bed", Id = 1, Name = "bed 12", Count = 2, imagepath=@"C:\Users\radhi\Desktop\BLE\OneDrive-2016-12-27\ConsoleApplication2\image.jpg" };
            AuctionInfo auction = new AuctionInfo { Username = "user1" , AuctionAmount=658, Bidder="bidder1", Seller="Seller1", item= item, Question="Question1"};

            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("auction",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                             new XElement("Username", auction.Username),
                             new XElement("AuctionAmount", auction.AuctionAmount),
                             new XElement("Bidder", auction.Bidder),
                             new XElement("Question", auction.Question),
                             new XElement("item",
                                new XElement("Count", 2),
                                new XElement("Name", item.Name),
                                new XElement("imagepath",item.imagepath))
                ));
        }
    }
}
