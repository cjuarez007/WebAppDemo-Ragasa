import { Component, OnInit, inject } from '@angular/core';
import { UserRes } from '../../../auth/interfaces/user-res';
import { IncrementosRes } from '../../interfaces/incrementos-res';
import { HttpClient } from '@angular/common/http';
import { IncrementosService } from '../../services/incrementos.service';
import { FormsModule } from '@angular/forms';

import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-panel-incrementos',
  templateUrl: './panel-incrementos.component.html',
  styleUrl: './panel-incrementos.component.css'
})
export class PanelIncrementosComponent implements OnInit{
  private svIncrementos = inject(IncrementosService);
  
  public user : UserRes | null = null;  
  public jefes: IncrementosRes[] = [];
  public empleados: IncrementosRes[] = [];
  public nomina: number = 0;
  public supJefe: boolean = false;
  public nombre:string = "";


  guardarFila(empleado: IncrementosRes) {
    empleado.SueldoNuevo = empleado.SueldoMensual*(1+empleado.porcentaje_minimo_jefe)
    this.svIncrementos.actualizarEmpleado(empleado).subscribe({
      next: res => {
        console.log('Empleado actualizado:', res);
        // Opcional: mostrar mensaje de éxito
      },
      error: err => console.error('Error al actualizar:', err)
    });
  }

  ngOnInit(): void {
    if(localStorage.getItem('user')) {
      this.user = JSON.parse(localStorage.getItem('user')!)  
      if (this.user != null){
        console.log(this.user)        
        this.nomina = this.user.usuario.NominaId; 
        if(this.user.usuario.RolId == 1){
          this.supJefe = true; 
        }
      }      
    }    
    this.svIncrementos.getJefesIncrementos(this.nomina).subscribe(
      res => this.jefes = res
    )

    this.svIncrementos.getEmpleadosIncrementos(this.nomina).subscribe(
      res => this.empleados = res
    )

    this.svIncrementos.getUserIncrementos(this.nomina).subscribe(
      res => this.nombre = res[0].Nombre
    )
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
      doc.text('Acepta', margin + lineWidth/2, yPos + 6, { align: 'center' });

      // --- Autoriza ---
      doc.line(margin + lineWidth + gap, yPos, margin + lineWidth*2 + gap, yPos);
      doc.text('Autoriza', margin + lineWidth + gap + lineWidth/2, yPos + 6, { align: 'center' });

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
  }
}
