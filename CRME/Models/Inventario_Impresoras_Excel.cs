﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
	public class Inventario_Impresoras_Excel
	{
	 public int Registro { get; set; }
	 public string Serie { get; set; }
	 public string Marca { get; set; }
	 public string Modelo { get; set; }
	 public string Parte { get; set; } 
	 public int Precio { get; set; }
	 public string Codigo_Inventario { get; set; }
	 public string  Estado { get; set; }
	}
}