import { Component, ElementRef, OnInit, ViewChild, inject, AfterViewChecked, ChangeDetectorRef } from '@angular/core';
import { UserRes } from '../../../auth/interfaces/user-res';
import { IncrementosRes } from '../../interfaces/incrementos-res';
import { HttpClient } from '@angular/common/http';
import { IncrementosService } from '../../services/incrementos.service';
import { FormsModule } from '@angular/forms';

import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { PorcentajesEstandarService } from '../../services/porcentajes-estandar.service';
import { PorcentajesEstandar } from '../../interfaces/porcentajes-estandar';

import { FileUploadService } from '../../services/FileUploadService';
import { EnvioCorreoService } from '../../services/envio-correo.service';



@Component({
  selector: 'app-panel-incrementos',
  templateUrl: './panel-incrementos.component.html',
  styleUrl: './panel-incrementos.component.css'
})
export class PanelIncrementosComponent implements OnInit, AfterViewChecked{

  private svIncrementos = inject(IncrementosService);
  private svPorcentajesEstandar = inject(PorcentajesEstandarService)
  private cdr = inject(ChangeDetectorRef)
  private http = inject(HttpClient);
  private SvEnvioCorreo = inject(EnvioCorreoService)
  
  public textareasParaFocus: Set<string> = new Set();
  
  public user : UserRes | null = null;  
  public jefes: IncrementosRes[] = [];  
  public nomina: number = 0;
  public supJefe: boolean = false;
  public nombre:string = "";
  public excedente:number = 0;

  public porcentajesEstandar: PorcentajesEstandar[] = [];

  //Variables para filtro
  jefeSeleccionado: number | null = null;
  empleadosOriginal: IncrementosRes[] = [];
  empleadosFiltrados: IncrementosRes[] = [];

  public sendPDF() {
    const blobName = "Aumento Salarial 2026.pdf";
    // La ruta en Angular para archivos en assets es relativa a la raíz del sitio
    const rutaAssets = "assets/control_incrementos.pdf"; 
  
    const containerSasUrl =
      "https://stlabsoinf.blob.core.windows.net/test-ragasa/" + blobName + "?sp=racwd&st=2026-01-13T22:37:51Z&se=2026-03-31T06:52:51Z&spr=https&sv=2024-11-04&sr=c&sig=zyfcodcsG9CGfI2ZscmkhwNvUZdi3DE2uD1VCEyVbiU%3D";
  
    // 1. Obtenemos el archivo de la carpeta assets como un BLOB
    this.http.get(rutaAssets, { responseType: 'blob' }).subscribe({
      next: async (archivoBlob) => {
        try {
          const uploadService = new FileUploadService(containerSasUrl);
          await uploadService.uploadPdf(archivoBlob);
          // console.log("✅ PDF subido correctamente desde assets");
        } catch (error) {
          // console.error("❌ Error al subir a Azure:", error);
        }
      },
      error: (err) => {
        console.error("❌ No se pudo encontrar el archivo en assets. Revisa la ruta.", err);
      }
    });
  }
  
  guardarFila(empleado: IncrementosRes) {
    if (!this.puedeGuardar(empleado)) {
      alert('Debe proporcionar una justificación antes de guardar');
      return;
    }
  
    // Asegurar que el sueldo se calcule bien antes de enviar
    empleado.SueldoNuevo = empleado.SueldoMensual * (1 + (empleado.porcentaje_minimo_jefe / 100));
  
    this.svIncrementos.actualizarEmpleado(empleado).subscribe({
      next: res => {
        console.log('Actualizado correctamente');
        // Sincronizar con la lista original para que no se pierda al filtrar/desfiltrar
        const index = this.empleadosOriginal.findIndex(e => e.Nomina === empleado.Nomina);
        if (index !== -1) {
          this.empleadosOriginal[index] = { ...empleado };
        }
      },
      error: err => console.error('Error:', err)
    });
  }

  ngOnInit(): void {
    if(localStorage.getItem('user')) {
      this.user = JSON.parse(localStorage.getItem('user')!)  
      if (this.user != null){        
        this.nomina = this.user.usuario.NominaId; 
        if(this.user.usuario.RolId == 1){
          this.supJefe = true; 
        }
      }      
    } 

    this.svIncrementos.getJefesIncrementos(this.nomina).subscribe(
      (res) => {
        this.jefes = res.map(j => ({
          ...j,
          JustificacionJefe: j.JustificacionJefe ?? '',
          JustificacionSuperJefe: j.JustificacionSuperJefe ?? ''
        }));
    
        this.marcarFilasAleatorias(this.jefes);
      },
      (error) => {
        console.error('Error al cargar jefes:', error);
      }
    );

    this.svIncrementos.getEmpleadosIncrementos(this.nomina).subscribe(
      (res) => {
        this.empleadosOriginal = res.map(e => ({
          ...e,
          JustificacionJefe: e.JustificacionJefe ?? '',
          JustificacionSuperJefe: e.JustificacionSuperJefe ?? ''
        }));
    
        this.empleadosFiltrados = [...this.empleadosOriginal];
    
        this.marcarFilasAleatorias(this.empleadosFiltrados);
      }
    );
    

    this.svIncrementos.getUserIncrementos(this.nomina).subscribe(
      res => this.nombre = res[0].Nombre
    );
      
    this.svPorcentajesEstandar.getPorcentajesEstandar().subscribe(
      res => this.porcentajesEstandar = res 
    );
  }

  requiereJustificacion(porcentaje: number): boolean {
    if (porcentaje == null) return false;

    return porcentaje < this.porcentajesEstandar[0].Valor || porcentaje > this.porcentajesEstandar[1].Valor;
  }

  verificarLimpiezaTexto(empleado: IncrementosRes, supJefe:boolean) {    
    
    if (supJefe) {
      if(!this.requiereJustificacion(empleado.porcentaje_minimo_jefe)){
        empleado.JustificacionSuperJefe = "";
      }      
    }
    else{
      if(!this.requiereJustificacion(empleado.porcentaje_minimo)){
        empleado.JustificacionJefe = "";
      }      
    }
        
  }

  filtrarEmpleadosPorJefe() {
    if (!this.jefeSeleccionado) {
      this.empleadosFiltrados = this.empleadosOriginal;
      return;
    }

    this.empleadosFiltrados = this.empleadosOriginal.filter(
      e => e.Jefe === this.jefeSeleccionado
    );
  }

  public generarPDF() {
    const doc = new jsPDF();

    // --- Título ---
    doc.setFontSize(18);
    doc.text('Control de Incrementos', 14, 20);

    let yPos = 30;

    // --- Directos (jefes) ---
    if (this.jefes.length > 0) {
      doc.setFontSize(14);      
      yPos += 6;
      

      const fechaHoy = new Date();
      const fechaAnterior = fechaHoy.toLocaleDateString('es-MX', { day: '2-digit', month: 'long', year: 'numeric' });
      const fechaNueva = new Date(fechaHoy.getTime() + 24*60*60*1000) // +1 día
                          .toLocaleDateString('es-MX', { day: '2-digit', month: 'long', year: 'numeric' });

      autoTable(doc, {
        startY: yPos,
        head: [['Situación Anterior', 'Incremento', 'Situación Actual']],
        body: [[
          `Hasta el ${fechaAnterior}\nSueldo diario: $${(Number(this.jefes[0].SueldoMensual.toFixed(2)) / 30).toFixed(2)}\nSueldo mensual: $${this.jefes[0].SueldoMensual.toFixed(2)}`,
          `${this.jefes[0].porcentaje_minimo_jefe.toFixed(2)}%`,
          `A partir del ${fechaNueva}\nSueldo diario: $${((this.jefes[0].SueldoMensual*(1+this.jefes[0].porcentaje_minimo_jefe/100))/30).toFixed(2)}\nSueldo mensual: $${(this.jefes[0].SueldoMensual*(1+this.jefes[0].porcentaje_minimo_jefe/100)).toFixed(2)}`
        ]],
        theme: 'striped',
        headStyles: { fillColor: [156, 39, 176] },
        styles: { fontSize: 10, cellPadding: 2, overflow: 'linebreak' },
      });

      yPos = (doc as any).lastAutoTable.finalY + 60;

      const pageWidth = doc.internal.pageSize.getWidth();
      const margin = 20; // margen izquierdo y derecho
      const lineWidth = 50; // ancho de la línea de firma
      const gap = (pageWidth - margin*2 - lineWidth*3) / 2; // espacio entre las líneas

      // --- Acepta ---
      doc.setLineWidth(0.5);
      doc.line(margin, yPos, margin + lineWidth, yPos); // línea
      doc.text(`Acepta\n${this.nombre} Salazar Meléndez`, margin + lineWidth/2, yPos + 6, { align: 'center' });

      // --- Autoriza ---
      doc.line(margin + lineWidth + gap, yPos, margin + lineWidth*2 + gap, yPos);
      doc.text(`Autoriza\nMarcelo Rodríguez Peña`, margin + lineWidth + gap + lineWidth/2, yPos + 6, { align: 'center' });

      // --- Fecha Efectiva ---      
      const fechaFormateada = fechaHoy.toLocaleDateString('es-MX', { month: 'long', year: 'numeric' });
      // Línea de la firma
      doc.line(margin + (lineWidth + gap)*2, yPos, margin + (lineWidth + gap)*2 + lineWidth, yPos);  
      // Fecha arriba de la línea (por ejemplo 4 unidades arriba)
      doc.text(fechaFormateada, margin + (lineWidth + gap)*2 + lineWidth/2, yPos - 4, { align: 'center' });
      // Texto "Fecha Efectiva" debajo de la línea (por ejemplo 6 unidades abajo)
      doc.text('Fecha Efectiva', margin + (lineWidth + gap)*2 + lineWidth/2, yPos + 6, { align: 'center' });      
      
    }
    
    doc.save('control_incrementos.pdf');

    this.sendPDF()
    this.SvEnvioCorreo.enviarNotificacionPdf("ediaz@exsoinf.com", 'Aumento Salarial 2026.pdf').subscribe({
      next: (res) => console.log('¡Logic App recibida!', res),
      error: (err) => console.error('Error detallado:', err)
    });
  }
  
  get totalSueldos(): number {
    const totalJefes = this.jefes?.reduce(
      (sum, j) => sum + (j.SueldoMensual || 0),
      0
    ) || 0;
  
    const totalEmpleados = this.empleadosOriginal?.reduce(
      (sum, e) => sum + (e.SueldoMensual || 0),
      0
    ) || 0;
  
    return totalJefes + totalEmpleados;
  }
  
  get totalSueldosSugeridos(): number {

    // jefe.SueldoMensual * (1 + jefe.PorcIncrementoSugerido / 100)
    const totalJefes = this.jefes?.reduce(
      (sum, j) => sum + ((j.SueldoMensual * (1 + j.PorcIncrementoSugerido / 100)) || 0),
      0
    ) || 0;
  
    const totalEmpleados = this.empleadosOriginal?.reduce(
      (sum, e) => sum + ((e.SueldoMensual * (1 + e.PorcIncrementoSugerido / 100)) || 0),
      0
    ) || 0;
  
    return totalJefes + totalEmpleados;
  }

  get totalSueldosOtorgado(): number {

    // jefe.SueldoMensual * (1 + jefe.PorcIncrementoSugerido / 100)
    const totalJefes = this.jefes?.reduce(
      (sum, j) => sum + ((j.SueldoMensual*(1+(j.porcentaje_minimo_jefe/100)))|| 0),
      0
    ) || 0;
  
    const totalEmpleados = this.empleadosOriginal?.reduce(
      (sum, e) => sum + ((e.SueldoMensual*(1+(e.porcentaje_minimo_jefe/100))) || 0),
      0
    ) || 0;
  
    return totalJefes + totalEmpleados;
  }

  get totalSueldosExcedente(): number {    
    const totalJefes = this.jefes?.reduce(
      (sum, j) => sum + ((j.SueldoMensual * (1 + (j.PorcIncrementoSugerido+this.porcentajesEstandar[3].Valor) / 100)) || 0),
      0
    ) || 0;
  
    const totalEmpleados = this.empleadosOriginal?.reduce(
      (sum, e) => sum + ((e.SueldoMensual * (1 + (e.PorcIncrementoSugerido+this.porcentajesEstandar[3].Valor) / 100)) || 0),
      0
    ) || 0;
  
    return (totalJefes + totalEmpleados) - this.totalSueldos;
  }

  public totalSueldosExcedenteActual(sugerido:number, asignado:number): number {    
    const totalJefes = this.jefes?.reduce(
      (sum, j) => sum + ((j.SueldoMensual * (1 + (j.PorcIncrementoSugerido+this.porcentajeJefe(sugerido, asignado)) / 100)) || 0),
      0
    ) || 0;
  
    const totalEmpleados = this.empleadosOriginal?.reduce(
      (sum, e) => sum + ((e.SueldoMensual * (1 + (e.PorcIncrementoSugerido+this.porcentajeJefe(sugerido, asignado)) / 100)) || 0),
      0
    ) || 0;
  
    const exc:number = (this.totalSueldosOtorgado - this.totalSueldos) - (this.totalSueldosSugeridos - this.totalSueldos);    
    
    return (this.totalSueldosExcedente-(this.totalSueldosSugeridos - this.totalSueldos)) - exc;
  }

  public porcentajeJefe(sugerido:number, asignado:number): number{
    if (asignado > sugerido){
      this.excedente = asignado - sugerido;
    }
    else{
      return this.porcentajesEstandar[3].Valor/100;
    }

    return (this.porcentajesEstandar[3].Valor - this.excedente)/100;
  }

  public marcarFilasAleatorias(lista: any[]) {
    lista.forEach(item => {      
      item._highlight = Math.random() < 0.3;
    });
  }

  // Método para hacer focus en textarea cuando aparece
  focusTextarea(id: string) {
    this.textareasParaFocus.add(id);
  }

  // Método que se ejecuta cuando se sale del campo de porcentaje
  onPorcentajeBlur(nomina: number, porcentaje: number, tipo: 'jefe' | 'superJefe', esJefe: boolean) {
    if (this.requiereJustificacion(porcentaje)) {
      // Coincidir con el ID del HTML: 'justificacion-jefe-empleado-123'
      const suffix = esJefe ? '' : '-empleado'; 
      const id = tipo === 'jefe' 
                 ? `justificacion-jefe${suffix}-${nomina}` 
                 : `justificacion-super-jefe${suffix}-${nomina}`;
  
      const puedeEditar = (tipo === 'jefe' && !this.supJefe) || (tipo === 'superJefe' && this.supJefe);
      if (puedeEditar) {
        this.focusTextarea(id);
      }
    }
  }
  

  // Método para verificar si se puede guardar (si requiere justificación, debe estar completa)
  puedeGuardar(item: IncrementosRes): boolean {
    // Si es super jefe, verifica la justificación del super jefe
    if (this.supJefe) {
      if (this.requiereJustificacion(item.porcentaje_minimo_jefe)) {
        return !!(item.JustificacionSuperJefe && item.JustificacionSuperJefe.trim().length > 0);
      }
    } else {
      // Si es jefe normal, verifica la justificación del jefe
      if (this.requiereJustificacion(item.porcentaje_minimo)) {
        return !!(item.JustificacionJefe && item.JustificacionJefe.trim().length > 0);
      }
    }
    // Si no requiere justificación, se puede guardar
    return true;
  }

  ngAfterViewChecked(): void {
    // Hacer focus en los textareas que están pendientes
    if (this.textareasParaFocus.size > 0) {
      const idsToFocus = Array.from(this.textareasParaFocus);
      this.textareasParaFocus.clear();
      
      idsToFocus.forEach(id => {
        setTimeout(() => {
          const textarea = document.getElementById(id) as HTMLTextAreaElement;
          if (textarea && document.activeElement !== textarea && textarea.offsetParent !== null) {
            try {
              textarea.focus();
            } catch (e) {              
            }
          }
        }, 200);
      });
    }
  }
  
}
