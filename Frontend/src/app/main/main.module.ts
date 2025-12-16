import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { PanelCargaComponent } from './pages/panel-carga/panel-carga.component';
import { PanelIncrementosComponent } from './pages/panel-incrementos/panel-incrementos.component';
import { SharedModule } from '../shared/shared.module';
import { DropzoneComponent } from './componentes/dropzone/dropzone.component';
import { PrimeNgModule } from '../prime-ng/prime-ng.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    PanelCargaComponent,
    PanelIncrementosComponent,
    DropzoneComponent,    
  ],
  imports: [
    CommonModule,
    MainRoutingModule,
    SharedModule,
    PrimeNgModule,
    FormsModule,
  ]
})
export class MainModule { }
