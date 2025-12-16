import { Component, OnInit, inject } from '@angular/core';
import { UserRes } from '../../../auth/interfaces/user-res';
import { IncrementosRes } from '../../interfaces/incrementos-res';
import { HttpClient } from '@angular/common/http';
import { IncrementosService } from '../../services/incrementos.service';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-panel-incrementos',
  templateUrl: './panel-incrementos.component.html',
  styleUrl: './panel-incrementos.component.css'
})
export class PanelIncrementosComponent implements OnInit{
  private svIncrementos = inject(IncrementosService);
  
  public user : UserRes | null = null;
  public name : string = "";
  public jefes: IncrementosRes[] = [];
  public empleados: IncrementosRes[] = [];
  public miNn: number | null = null;

  guardarFila(empleado: IncrementosRes) {
    this.svIncrementos.actualizarEmpleado(empleado).subscribe({
      next: res => {
        console.log('Empleado actualizado:', res);
        // Opcional: mostrar mensaje de éxito
      },
      error: err => console.error('Error al actualizar:', err)
    });
  }
  
  recalcular(emp: any) {
    if (emp.SueldoActual && emp.PorcSugerido != null) {
      emp.SueldoSugerido =
        emp.SueldoActual + (emp.SueldoActual * emp.PorcSugerido / 100);
    }
  
    if (emp.SueldoActual && emp.PorcOtorgadoJefe != null) {
      emp.SueldoNuevo =
        emp.SueldoActual + (emp.SueldoActual * emp.PorcOtorgadoJefe / 100);
    }
  }
  

  ngOnInit(): void {
    if(localStorage.getItem('user')) {
      this.user = JSON.parse(localStorage.getItem('user')!)  
      if (this.user != null){
        console.log(this.user)
        this.name = `${this.user?.usuario?.Nombres} ${this.user.usuario.ApellidoPaterno} ${this.user.usuario.ApellidoMaterno}`
        this.miNn = this.user.usuario.NominaId; 
      }      
    }    
    this.svIncrementos.getJefesIncrementos().subscribe(
      res => this.jefes = res
    )

    this.svIncrementos.getEmpleadosIncrementos().subscribe(
      res => this.empleados = res
    )
  }
}
