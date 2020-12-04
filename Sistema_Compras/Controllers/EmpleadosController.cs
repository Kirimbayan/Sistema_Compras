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
    public class EmpleadosController : Controller
    {
        private ComprasEntities db = new ComprasEntities();

        // GET: Empleados
        // BARRA DE BUSQUEDA DE EMPLEADOS
        [Authorize(Roles = "Administrador, Empleado")]
        public ActionResult Index(string Criterio = null)
        {
            //var empleados = db.Empleados.Include(a => a.Nombre).Include(a => a.Departamento);
            return View(db.Empleados.Where(p => Criterio == null || 
            p.Nombre.StartsWith(Criterio) ||
            p.Cedula.StartsWith(Criterio) ||
            p.Departamento.ToString().StartsWith(Criterio) ||
            p.Activo.ToString().StartsWith(Criterio)).ToList());
        }


        // GET: Empleados/Details/5
        [Authorize(Roles = "Administrador, Empleado")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // GET: Empleados/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Departamento = new SelectList(db.Departamentos, "IdDep", "Nombre");
            return View();
        }

        // POST: Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEmp,Cedula,Nombre,Departamento,Activo,Empleado")] Empleados empleados)
        {
            if (!validaCedula(empleados.Cedula))
                ModelState.AddModelError("Cedula", "Error de validacion de Cedula");

            if (ModelState.IsValid)
            {
                db.Empleados.Add(empleados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Departamento = new SelectList(db.Departamentos, "IdDep", "Nombre", empleados.Departamento);
            return View(empleados);
        }

        // GET: Empleados/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            ViewBag.Departamento = new SelectList(db.Departamentos, "IdDep", "Nombre", empleados.Departamento);
            return View(empleados);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEmp,Cedula,Nombre,Departamento,Activo")] Empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departamento = new SelectList(db.Departamentos, "IdDep", "Nombre", empleados.Departamento);
            return View(empleados);
        }

        // GET: Empleados/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleados empleados = db.Empleados.Find(id);
            db.Empleados.Remove(empleados);
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

        //Validacion de CEDULA
        public static bool validaCedula(string pCedula)
        {
            int vnTotal = 0;
            string vcCedula = pCedula.Replace("-", "");
            int pLongCed = vcCedula.Trim().Length;
            int[] digitoMult = new int[11] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };

            if (pLongCed < 11 || pLongCed > 11)
                return false;

            for (int vDig = 1; vDig <= pLongCed; vDig++)
            {
                int vCalculo = Int32.Parse(vcCedula.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];
                if (vCalculo < 10)
                    vnTotal += vCalculo;
                else
                    vnTotal += Int32.Parse(vCalculo.ToString().Substring(0, 1)) + Int32.Parse(vCalculo.ToString().Substring(1, 1));
            }

            if (vnTotal % 10 == 0)
                return true;
            else
                return false;
        }

        //Para imprimir en Excel Inicio
        public ActionResult exportaExcel()
        {
            string filename = "Empleados.csv";
            string filepath = @"c:\tmp\" + filename;
            StreamWriter sw = new StreamWriter(filepath);
            sw.WriteLine("sep=,");
            sw.WriteLine("Cedula,Nombre,Departamento,Activo"); //Encabezado 
            foreach (var i in db.Empleados.ToList())
            {
                sw.WriteLine(i.Cedula + "," + i.Nombre + "," + i.Departamento + "," + i.Activo);
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
