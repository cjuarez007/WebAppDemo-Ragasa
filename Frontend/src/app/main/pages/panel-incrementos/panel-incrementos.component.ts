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

    // --- Responsable ---
    doc.setFontSize(12);
    doc.text(`Responsable: ${this.nombre} - Nomina: ${this.user?.usuario.NominaId}`, 14, 30);

    let yPos = 40;

    // --- Directos (jefes) ---
    if (this.jefes.length > 0) {
      doc.setFontSize(14);
      doc.text('Directos', 14, yPos);
      yPos += 6;

      autoTable(doc, {
        startY: yPos,
        head: [['Nomina', 'Nombre', 'Puesto', 'Departamento', 'Sueldo Mensual', 'Sueldo Nuevo']],
        body: this.jefes.map(j => [
          j.Nomina,
          j.Nombre,
          j.Puesto,
          j.Departamento,
          j.SueldoMensual.toFixed(2),
          j.SueldoMensual*(1+j.porcentaje_minimo_jefe)
        ]),
        theme: 'striped',
        headStyles: { fillColor: [156, 39, 176] },
        styles: { fontSize: 10 }
      });

      yPos = (doc as any).lastAutoTable.finalY + 10;
    }

    // --- Indirectos (empleados) ---
    if (this.empleados.length > 0) {
      doc.setFontSize(14);
      doc.text('Indirectos', 14, yPos);
      yPos += 6;

      autoTable(doc, {
        startY: yPos,
        head: [['Nomina', 'Nombre', 'Puesto', 'Departamento', 'Sueldo Mensual', 'Sueldo Nuevo']],
        body: this.empleados.map(e => [
          e.Nomina,
          e.Nombre,
          e.Puesto,
          e.Departamento,
          e.SueldoMensual.toFixed(2),
          e.SueldoMensual*(1+e.porcentaje_minimo_jefe)
        ]),
        theme: 'striped',
        headStyles: { fillColor: [63, 81, 181] },
        styles: { fontSize: 10 }
      });

      yPos = (doc as any).lastAutoTable.finalY + 10;
    }

    
    // --- Agregar logo al final ---
    const img = new Image();
    img.src = 'assets/logo_grande.png'; // tu ruta
    img.onload = () => {
      const logoWidth = 45;
      const logoHeight = 28;
      const pageWidth = doc.internal.pageSize.getWidth();
      const x = (pageWidth - logoWidth) / 2 + 70; 

      doc.addImage(img, 'PNG', x, yPos, logoWidth, logoHeight);

      //   --- Guardar PDF ---
      doc.save('control_incrementos.pdf');
    };
  }
}
