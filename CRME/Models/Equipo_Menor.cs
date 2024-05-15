namespace CRME.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Equipo_Menor
    {
        [Key]
        public int Id { get; set; }
        public string Cod_Inventario { get; set; }
        [StringLength(500)]
        public string Descripcion { get; set; }

        public int Id_Familia { get; set; }
        public int Id_Sub_fam { get; set; }
        public int Id_condicion { get; set; }
        // pendiente campo profecional
        public bool EPI { get; set; }
        public int Id_UM { get; set; }
        public string Color { get; set; }
        public string Foto { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string serie { get; set; }
        public int Id_medida { get; set; }
        public int Em_Cve_Empresa { get; set; }
        public int Sc_Cve_Sucursal { get; set; }
        public int estatus_ID { get; set; }
        public int Dp_Cve_Departamento { get; set; }
    }
}