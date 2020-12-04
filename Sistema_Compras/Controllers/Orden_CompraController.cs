using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sistema_Compras;

namespace Sistema_Compras.Controllers
{
    public class Orden_CompraController : Controller
    {
        private ComprasEntities db = new ComprasEntities();

        // GET: Orden_Compra
        [Authorize(Roles = "Administrador, Empleado, Consulta")]
        public ActionResult Index(string Criterio = null)
        {
            var orden_Compra = db.Orden_Compra.Include(a => a.No_Orden).Include(a => a.Costo_Unitario);
            return View(db.Orden_Compra.Where(p => Criterio == null ||
            p.Articulo.ToString().StartsWith(Criterio) ||
            p.No_Orden.ToString().StartsWith(Criterio) ||
            p.Cantidad.ToString().StartsWith(Criterio) ||
            p.Marca.ToString().StartsWith(Criterio) ||
            p.Activo.ToString().StartsWith(Criterio)).ToList());
        }

        // GET: Orden_Compra/Details/5
        [Authorize(Roles = "Administrador, Empleado, Consulta")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden_Compra orden_Compra = db.Orden_Compra.Find(id);
            if (orden_Compra == null)
            {
                return HttpNotFound();
            }
            return View(orden_Compra);
        }

        // GET: Orden_Compra/Create
        [Authorize(Roles = "Administrador, Empleado")]
        public ActionResult Create()
        {
            ViewBag.Articulo = new SelectList(db.Articulos, "IdArt", "Articulo");
            ViewBag.Marca = new SelectList(db.Marcas, "IdMarca", "Nombre");
            ViewBag.Unidad_Medida = new SelectList(db.Medidas, "IdMedida", "Unidad_de_Medida");
            return View();
        }

        // POST: Orden_Compra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdOrden,No_Orden,Fecha_Orden,Activo,Articulo,Cantidad,Unidad_Medida,Marca,Costo_Unitario")] Orden_Compra orden_Compra)
        {
            if (ModelState.IsValid)
            {
                db.Orden_Compra.Add(orden_Compra);
                db.SaveChanges();

                Articulos articulos = (from r in db.Articulos.Where
                                          (a => a.Marca == orden_Compra.Marca)
                                          select r).FirstOrDefault();
                articulos.Existencia = articulos.Existencia + orden_Compra.Cantidad;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Articulo = new SelectList(db.Articulos, "IdArt", "Articulo", orden_Compra.Articulo);
            ViewBag.Marca = new SelectList(db.Marcas, "IdMarca", "Nombre", orden_Compra.Marca);
            ViewBag.Unidad_Medida = new SelectList(db.Medidas, "IdMedida", "Unidad_de_Medida", orden_Compra.Unidad_Medida);
            return View(orden_Compra);
        }

        // GET: Orden_Compra/Edit/5
        [Authorize(Roles = "Administrador, Empleado")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden_Compra orden_Compra = db.Orden_Compra.Find(id);
            if (orden_Compra == null)
            {
                return HttpNotFound();
            }
            ViewBag.Articulo = new SelectList(db.Articulos, "IdArt", "Articulo", orden_Compra.Articulo);
            ViewBag.Marca = new SelectList(db.Marcas, "IdMarca", "Nombre", orden_Compra.Marca);
            ViewBag.Unidad_Medida = new SelectList(db.Medidas, "IdMedida", "Unidad_de_Medida", orden_Compra.Unidad_Medida);
            return View(orden_Compra);
        }

        // POST: Orden_Compra/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOrden,No_Orden,Fecha_Orden,Activo,Articulo,Cantidad,Unidad_Medida,Marca,Costo_Unitario")] Orden_Compra orden_Compra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden_Compra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.Articulo = new SelectList(db.Articulos, "IdArt", "Articulo", orden_Compra.Articulo);
            ViewBag.Marca = new SelectList(db.Marcas, "IdMarca", "Nombre", orden_Compra.Marca);
            ViewBag.Unidad_Medida = new SelectList(db.Medidas, "IdMedida", "Unidad_de_Medida", orden_Compra.Unidad_Medida);
            return View(orden_Compra);
        }

        // GET: Orden_Compra/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden_Compra orden_Compra = db.Orden_Compra.Find(id);
            if (orden_Compra == null)
            {
                return HttpNotFound();
            }
            return View(orden_Compra);
        }

        // POST: Orden_Compra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orden_Compra orden_Compra = db.Orden_Compra.Find(id);
            db.Orden_Compra.Remove(orden_Compra);
            db.SaveChanges();

            Articulos articulos = (from r in db.Articulos.Where
                                    (a => a.Marca == orden_Compra.Marca)
                                   select r).FirstOrDefault();
            articulos.Existencia = articulos.Existencia - orden_Compra.Cantidad;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Para imprimir en Excel Inicio
        public ActionResult exportaExcel()
        {
            string filename = "Orden_Compra.csv";
            string filepath = @"c:\tmp\" + filename;
            StreamWriter sw = new StreamWriter(filepath);
            sw.WriteLine("sep=,");
            sw.WriteLine("No_Orden,Fecha_Orden,Activo,Articulo,Cantidad,Unidad_Medida,Marca,Costo_Unitario"); //Encabezado 
            foreach (var i in db.Orden_Compra.ToList())
            {
                sw.WriteLine(i.Articulo + "," + i.No_Orden + "," + i.Fecha_Orden + "," + i.Activo + "," + i.Articulo + "," + i.Cantidad + "," + i.Unidad_Medida + "," + i.Marca + "," + i.Costo_Unitario);
            }
            sw.Close();

            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
        //Para imprimir en Excel Fin


    }
}
