using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingDomain.Models;
using ShoppingInfrastructure;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace ShoppingInfrastructure.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShoppingDbContext _context;

        public ProductsController(ShoppingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Select(p => new
                {
                    Price = p.Price,
                    AvailabilityText = p.Availability ? "є" : "немає",
                    Name = p.Name,
                    BrandName = p.Brand.BrandName
                })
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");

                worksheet.Cell(1, 1).Value = "Ціна";
                worksheet.Cell(1, 2).Value = "Наявність";
                worksheet.Cell(1, 3).Value = "Назва";
                worksheet.Cell(1, 4).Value = "Бренд";

                int row = 2;
                foreach (var item in products)
                {
                    worksheet.Cell(row, 1).Value = item.Price;
                    worksheet.Cell(row, 2).Value = item.AvailabilityText;
                    worksheet.Cell(row, 3).Value = item.Name;
                    worksheet.Cell(row, 4).Value = item.BrandName;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = $"ProductsReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var shoppingDbContext = _context.Products.Include(p => p.Brand);
            return View(await shoppingDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Price,BrandId,Availability,Name,Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName", product.BrandId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName", product.BrandId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Price,BrandId,Availability,Name,Id")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName", product.BrandId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Неможливо видалити товар, оскільки існують пов'язані записи.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["ImportMessage"] = "Файл не обрано або він порожній.";
                return RedirectToAction("Import");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var workbook = new ClosedXML.Excel.XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet("Products");
                        int row = 2; 
                        while (!worksheet.Cell(row, 1).IsEmpty())
                        {
                            var priceCell = worksheet.Cell(row, 1).GetValue<string>();
                            var availabilityCell = worksheet.Cell(row, 2).GetValue<string>();
                            var nameCell = worksheet.Cell(row, 3).GetValue<string>();
                            var brandCell = worksheet.Cell(row, 4).GetValue<string>();

                            decimal price = 0;
                            decimal.TryParse(priceCell, out price);

                            bool availability = false;
                            if (availabilityCell == "є") availability = true;
                            else if (availabilityCell.Equals("true", StringComparison.OrdinalIgnoreCase))
                                availability = true;

                            var brand = await _context.Brands
                                .FirstOrDefaultAsync(b => b.BrandName == brandCell);
                            if (brand == null && !string.IsNullOrEmpty(brandCell))
                            {
                                brand = new Brand { BrandName = brandCell };
                                _context.Brands.Add(brand);
                                await _context.SaveChangesAsync();
                            }

                            var product = new Product
                            {
                                Price = price,
                                Availability = availability,
                                Name = nameCell?.Trim(),
                                BrandId = brand?.Id ?? 0
                            };
                            _context.Products.Add(product);

                            row++;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                TempData["ImportMessage"] = "Імпорт успішно виконано!";
            }
            catch (Exception ex)
            {
                TempData["ImportMessage"] = $"Помилка імпорту: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ExportToWordOpenXml()
        {
            using (var stream = new MemoryStream())
            {
                using (var wordDoc = WordprocessingDocument.Create(
                    stream,
                    WordprocessingDocumentType.Document,
                    true))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = new Body();
                    mainPart.Document.Append(body);

                    Paragraph paragraph = new Paragraph(
                        new Run(new Text("Товари")));
                    body.Append(paragraph);

                    Table table = new Table();

                    table.AppendChild(new TableProperties(
                        new TableBorders(
                            new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                            new BottomBorder() { Val = BorderValues.Single, Size = 6 },
                            new LeftBorder() { Val = BorderValues.Single, Size = 6 },
                            new RightBorder() { Val = BorderValues.Single, Size = 6 },
                            new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 6 },
                            new InsideVerticalBorder() { Val = BorderValues.Single, Size = 6 }
                        )
                    ));

                    TableRow headerRow = new TableRow();
                    headerRow.Append(new TableCell(new Paragraph(new Run(new Text("Ціна")))));
                    headerRow.Append(new TableCell(new Paragraph(new Run(new Text("Наявність")))));
                    headerRow.Append(new TableCell(new Paragraph(new Run(new Text("Назва")))));
                    headerRow.Append(new TableCell(new Paragraph(new Run(new Text("Бренд")))));
                    table.Append(headerRow);

                    var products = _context.Products
                        .Include(p => p.Brand)
                        .ToList();

                    foreach (var product in products)
                    {
                        TableRow dataRow = new TableRow();
                        dataRow.Append(new TableCell(
                            new Paragraph(new Run(new Text(product.Price.ToString())))));
                        string availabilityText = product.Availability ? "є" : "нема";
                        dataRow.Append(new TableCell(
                            new Paragraph(new Run(new Text(availabilityText)))));
                        dataRow.Append(new TableCell(
                            new Paragraph(new Run(new Text(product.Name)))));
                        string brandName = product.Brand?.BrandName ?? "";
                        dataRow.Append(new TableCell(
                            new Paragraph(new Run(new Text(brandName)))));

                        table.Append(dataRow);
                    }

                    body.Append(table);
                }

                stream.Position = 0;
                var fileName = "ProductsReport.docx";
                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    fileName);
            }
        }

        [HttpGet]
        public IActionResult ImportDocx()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromDocx(IFormFile docxFile)
        {
            if (docxFile == null || docxFile.Length == 0)
            {
                TempData["ImportMessage"] = "Файл не обрано або він порожній.";
                return RedirectToAction("ImportDocx");
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    await docxFile.CopyToAsync(ms);
                    ms.Position = 0;

                    using (var wordDoc = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(ms, false))
                    {
                        var body = wordDoc.MainDocumentPart.Document.Body;
                        var table = body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().FirstOrDefault();
                        if (table == null)
                        {
                            TempData["ImportMessage"] = "Таблиця не знайдена в документі.";
                            return RedirectToAction("Index");
                        }

                        var rows = table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Skip(1);
                        foreach (var row in rows)
                        {
                            var cells = row.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ToList();
                            if (cells.Count < 4)
                                continue; 

                            string priceText = cells[0].InnerText;
                            string availabilityText = cells[1].InnerText;
                            string nameText = cells[2].InnerText;
                            string brandText = cells[3].InnerText;

                            decimal price = 0;
                            decimal.TryParse(priceText, out price);

                            bool availability = (availabilityText == "є" ||
                                                   availabilityText.Equals("true", StringComparison.OrdinalIgnoreCase));

                            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.BrandName == brandText);
                            if (brand == null && !string.IsNullOrEmpty(brandText))
                            {
                                brand = new Brand { BrandName = brandText };
                                _context.Brands.Add(brand);
                                await _context.SaveChangesAsync();
                            }

                            var product = new Product
                            {
                                Price = price,
                                Availability = availability,
                                Name = nameText?.Trim(),
                                BrandId = brand?.Id ?? 0
                            };
                            _context.Products.Add(product);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                TempData["ImportMessage"] = "Імпорт з .docx успішно виконано!";
            }
            catch (Exception ex)
            {
                TempData["ImportMessage"] = $"Помилка імпорту: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

    }
}
