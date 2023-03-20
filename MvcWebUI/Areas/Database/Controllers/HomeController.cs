using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Text;

namespace MvcWebUI.Areas.Database.Controllers
{
    [Area("Db")]
    public class HomeController : Controller
    {
        private readonly ETradeContext _db;

        public HomeController(ETradeContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            try
            {
                #region Mevcut verilerin silinmesi

                var productStores = _db.ProductStores.ToList(); // önce ürün mağaza listesini çekip daha sonra ürün mağaza DbSet'i üzerinden RemoveRange methodu ile silinmesini sağlıyoruz
                _db.ProductStores.RemoveRange(productStores);

                var stores = _db.Stores.ToList();
                _db.Stores.RemoveRange(stores);

                var products = _db.Products.ToList();
                _db.Products.RemoveRange(products);

                var categories = _db.Categories.ToList();
                _db.Categories.RemoveRange(categories);

                var userDetials = _db.UserDetails.ToList();
                _db.UserDetails.RemoveRange(userDetials);

                var users = _db.Users.ToList();
                _db.Users.RemoveRange(users);

                var roles = _db.Roles.ToList();
                _db.Roles.RemoveRange(roles);

                if (roles.Count > 0) // eğer veritabanında rol kaydı varsa eklenecek rollerin rol id'lerini aşağıdaki SQL komutu üzerinden 1'den başlayacak hale getiriyoruz
                                     // eğer kayıt yoksa o zaman zaten rol tablosuna daha önce veri eklenmemiştir dolayısıyla rol id'leri 1'den başlayacaktır
                {
                    _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roles', RESEED, 0)"); // ExecuteSqlRaw methodu üzerinden istenilen SQL sorgusu elle yazılıp veritabanında çalıştırılabilir
                }

                var cities = _db.Cities.ToList();
                _db.Cities.RemoveRange(cities);

                var countries = _db.Countries.ToList();
                _db.Countries.RemoveRange(countries);
                #endregion



                #region İlk verilerin oluşturulması
                _db.Stores.Add(new Store()
                {
                    Name = "Hepsiburada",
                    IsVirtual = true
                });
                _db.Stores.Add(new Store()
                {
                    Name = "Vatan",
                    IsVirtual = false
                });
                _db.SaveChanges(); // mağazaları veritabanına kaydediyoruz ki aşağıda oluşturacağımız ürünler için mağaza adı üzerinden istediğimiz mağaza kayıtlarına ulaşıp
                                   // mağaza id'lerini ürün mağaza ilişki entity'si üzerinden tablosunda doldurabilelim

                _db.Categories.Add(new Category()
                {
                    Name = "Computer",
                    Description = "Laptops, desktops and computer peripherals",
                    Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Laptop",
                        UnitPrice = 3000.5,
                        ExpirationDate = new DateTime(2032, 1, 27),
                        StockAmount = 10,
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Mouse",
                        UnitPrice = 20.5,
                        StockAmount = 50,
                        Description = "Computer peripheral",
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            },
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Keyboard",
                        UnitPrice = 40,
                        StockAmount = 45,
                        Description = "Computer peripheral",
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            },
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Monitor",
                        UnitPrice = 2500,
                        ExpirationDate = DateTime.Parse("05/19/2027"),
                        StockAmount = 20,
                        Description = "Computer peripheral",
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    }
                }
                });

                _db.Categories.Add(new Category()
                {
                    Name = "Home Theater System",
                    Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Speaker",
                        UnitPrice = 2500,
                        StockAmount = 70
                    },
                    new Product()
                    {
                        Name = "Receiver",
                        UnitPrice = 5000,
                        StockAmount = 30,
                        Description = "Home theater system component",
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Equalizer",
                        UnitPrice = 1000,
                        StockAmount = 40,
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            },
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    }
                }
                });

                _db.Countries.Add(new Country()
                {
                    Name = "United States",
                    Cities = new List<City>()
                {
                    new City()
                    {
                        Name = "Los Angeles"
                    },
                    new City()
                    {
                        Name = "New York"
                    }
                }
                });
                _db.Countries.Add(new Country()
                {
                    Name = "Turkey",
                    Cities = new List<City>()
                {
                    new City()
                    {
                        Name = "Ankara"
                    },
                    new City()
                    {
                        Name = "Istanbul"
                    },
                    new City()
                    {
                        Name = "Izmir"
                    }
                }
                });

                _db.SaveChanges();

                _db.Roles.Add(new Role()
                {
                    Name = "Admin",
                    Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Password = "cagil",
                        UserName = "cagil",
                        UserDetail = new UserDetail()
                        {
                            Address = "Cankaya",
                            CityId = _db.Cities.SingleOrDefault(c => c.Name == "Ankara").Id,
                            CountryId = _db.Countries.SingleOrDefault(c => c.Name == "Turkey").Id,
                            Email = "cagil@etrade.com",
                            Sex = Sex.Man
                        }
                    }
                }
                });
                _db.Roles.Add(new Role()
                {
                    Name = "User",
                    Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Password = "leo",
                        UserName = "leo",
                        UserDetail = new UserDetail()
                        {
                            Address = "Hollywood",
                            CityId = _db.Cities.SingleOrDefault(c => c.Name == "Los Angeles").Id,
                            CountryId = _db.Countries.SingleOrDefault(c => c.Name == "United States").Id,
                            Email = "leo@etrade.com",
                            Sex = Sex.Man
                        }
                    }
                }
                });
                #endregion



                #region DbSet'ler üzerinden yapılan değişikliklerin tek seferde veritabanına yansıtılması (Unit of Work)
                _db.SaveChanges();
                #endregion



                //return Content("Database seed successful."); // *1
                //return Content("<label style=\"color:red;\"><b>Database seed successful.</b></label>", "text/html"); // *2
                return Content("<label style=\"color:red;\"><b>Database seed successful.</b></label>", "text/html", Encoding.UTF8); // *3
                                                                                                                                    // Yeni bir view oluşturup işlem sonucunu göstermek yerine Controller base class'ının Content methodunu kullanıp işlem sonucunun boş bir sayfada gösterilmesini sağladık.
                                                                                                                                    // *1: Metinsel verinin düz yazı (plain text) olarak sayfada gösterilmesini sağlar.
                                                                                                                                    // *2: HTML verisinin sayfada HTML içerik olarak gösterilmesini sağlar.
                                                                                                                                    // *3: HTML verisinin sayfada Türkçe karakterleri destekleyen UTF8 karakter kümesi üzerinden HTML içerik olarak gösterilmesini sağlar.
            }
            catch (Exception exc)
            {
                string message = exc.Message;
                throw exc;
            }
        }

        public ContentResult GetHtmlContent()
        {
            //return new ContentResult(); // ContentResult objesini new'leyerek dönmek yerine aşağıdaki methodu kullanılmalıdır.
            return Content("<b><i>Content result.</i></b>", "text/html"); // içerik tipi text/html belirtilmelidir ki tarayıcı HTML olarak yorumlayabilsin,
                                                                          // Türkçe karakterlerde problem olursa son parametre olarak Encoding.UTF8 de kullanılabilir.
        }

        public ActionResult GetProductsXmlContent() // tarayıcıda çağrılması: ~/Products/GetProductsXmlContent, XML döndürme işlemleri genelde bu şekilde yapılmaz, web servisler üzerinden döndürülür.
        {
            List<Product> products = _db.Products.ToList();
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            xml += "<Products>";
            foreach (var product in products)
            {
                xml += "<Product>";
                xml += "<Id>" + product.Id + "</Id>";
                xml += "<Name>" + product.Name + "</Name>";
                xml += "<Description>" + product.Description + "</Description>";
                xml += "<UnitPrice>" + product.UnitPrice + "</UnitPrice>";
                xml += "<StockAmount>" + product.StockAmount + "</StockAmount>";
                xml += "<ExpirationDate>" + product.ExpirationDate + "</ExpirationDate>";
                xml += "<Category>" + product.CategoryId + "</Category>";
                xml += "</Product>";
            }
            xml += "</Products>";
            return Content(xml, "application/xml"); // XML verileri için içerik tipi application/xml belirtilmelidir ki tarayıcı XML olarak yorumlayabilsin.
        }

        public string GetString() // tarayıcıda çağrılması: ~/Products/GetString
        {
            return "String."; // sayfaya "String." düz yazısını (plain text) döner 
        }

        public EmptyResult GetEmpty() // tarayıcıda çağrılması: ~/Products/GetEmpty
        {
            return new EmptyResult(); // içerisinde hiç bir veri olmayan boş bir sayfa döner
        }

        public RedirectResult RedirectToMicrosoft() // tarayıcıda çağrılması: ~/Products/RedirectToMicrosoft
        {
            //return new RedirectResult(); // RedirectResult objesini new'leyerek dönmek yerine aşağıdaki methodu kullanılmalıdır.
            return Redirect("https://microsoft.com"); // parametre olarak belirtilen adrese yönlendirir
        }
    }
}
