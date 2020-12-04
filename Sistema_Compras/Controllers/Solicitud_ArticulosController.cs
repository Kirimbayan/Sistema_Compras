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
    public class Solicitud_ArticulosController : Controller
    {
        private ComprasEntities db = new ComprasEntities();

        // GET: Solicitud_Articulos
        [Authorize(Roles = "Administrador, Empleado, Consulta")]
        public ActionResult Index(string Criterio = null)
        {
            var solicitud_Articulos = db.Solicitud_Articulos.Include(a => a.Empleado).Include(a => a.Articulo);
            return View(db.Solicitud_Articulos.Where(p => Criterio == null ||
            p.Empleado.ToString().StartsWith(Criterio) ||
            p.Fecha_Solicitud.ToString().StartsWith(Criterio) ||
            p.Articulo.ToString().StartsWith(Criterio) ||
            p.Cantidad.ToString().StartsWith(Criterio) ||
            p.Unidad_Medida.ToString().StartsWith(Criterio) ||
            p.Activo.ToString().StartsWith(Criterio)).ToList());
        }


        // GET: Solicitud_Articulos/Details/5
        [Authorize(Roles = "Administrador, Empleado, Consulta")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud_Articulos solicitud_Articulos = db.Solicitud_Articulos.Find(id);
            if (solicitud_Articulos == null)
            {
                return HttpNotFound();
            }
            return View(solicitud_Articulos);
        }

        // GET: Solicitud_Articulos/Create
        [Authorize(Roles = "Administrador, Empleado")]
        public ActionResult Create()
        {
            ViewBag.Articulo = new SelectList(db.Articulos, "IdArt", "Articulo");
            ViewBag.Empleado = new SelectList(db.Empleados, "IdEmp", "Cedula");
            ViewBag.Unidad_Medida = new SelectList(db.Medidas, "IdMedida", "Unidad_de_Medida");
            return View();
        }

        // POST: Solicitud_Articulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSol,Empleado,Fecha_Solicitud,Articulo,Cantidad,Unidad_Medida,Activo")] Solicitud_Articulos solicitud_Articulos)
        {

            if (ModelState.IsValid)
            {
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Articulo = new SelectList(db.Articulos, "IdArt", "Articulo", solicitud_Articulos.Articulo);
            ViewBag.Empleado = new SelectList(db.Empleados, "IdEmp", "Cedula", solicitud_Articulos.Empleado);
            ViewBag.Unidad_Medida = new SelectList(db.Medidas, "IdMedida", "Unidad_de_Medida", solicitud_Articulos.Unidad_Medida);
            return View(solicitud_Articulos);
        }

        // GET: Solicitud_Articulos/Edit/5
        [Authorize(Roles = "Administrador, Empleado")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud_Articulos solicitud_Articulos = db.Solicitud_Articulos.Find(id);
            if (solicitud_Articulos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Articulo = new SelectList(db.Articulos, "IdArt", "Articulo", solicitud_Articulos.Articulo);
            ViewBag.Empleado = new SelectList(db.Empleados, "IdEmp", "Cedula", solicitud_Articulos.Empleado);
            ViewBag.Unidad_Medida = new SelectList(db.Medidas, "IdMedida", "Unidad_de_Medida", solicitud_Articulos.Unidad_Medida);
            return View(solicitud_Articulos);
        }

        // POST: Solicitud_Articulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSol,Empleado,Fecha_Solicitud,Articulo,Cantidad,Unidad_Medida,Activo")] Solicitud_Articulos solicitud_Articulos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitud_Articulos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Articulo = new SelectList(db.Articulos, "IdArt", "Articulo", solicitud_Articulos.Articulo);
            ViewBag.Empleado = new SelectList(db.Empleados, "IdEmp", "Cedula", solicitud_Articulos.Empleado);
            ViewBag.Unidad_Medida = new SelectList(db.Medidas, "IdMedida", "Unidad_de_Medida", solicitud_Articulos.Unidad_Medida);
            return View(solicitud_Articulos);
        }

        // GET: Solicitud_Articulos/Delete/5
        [Authorize(Roles = "Administrador, Empleado")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud_Articulos solicitud_Articulos = db.Solicitud_Articulos.Find(id);
            if (solicitud_Articulos == null)
            {
                return HttpNotFound();
            }
            return View(solicitud_Articulos);
        }

        // POST: Solicitud_Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solicitud_Articulos solicitud_Articulos = db.Solicitud_Articulos.Find(id);
            db.Solicitud_Articulos.Remove(solicitud_Articulos);
            db.SaveChanges();

            Articulos articulos = (from r in db.Articulos.Where
                                           (a => a.Unidad_Medida == solicitud_Articulos.Unidad_Medida)
                                   select r).FirstOrDefault();
            articulos.Existencia = articulos.Existencia + solicitud_Articulos.Cantidad;
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
            string filename = "Solicitud_Articulos.csv";
            string filepath = @"c:\tmp\" + filename;
            StreamWriter sw = new StreamWriter(filepath);
            sw.WriteLine("sep=,");
            sw.WriteLine("Empleado,Fecha_Solicitud,Articulo,Cantidad,Unidad_Medida,Activo"); //Encabezado 
            foreach (var i in db.Solicitud_Articulos.ToList())
            {
                sw.WriteLine(i.Empleado + "," + i.Fecha_Solicitud + "," + i.Articulo + "," + i.Cantidad + "," + i.Unidad_Medida + "," + i.Activo);
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
