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
}
