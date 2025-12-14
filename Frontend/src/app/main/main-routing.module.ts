import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PanelCargaComponent } from './pages/panel-carga/panel-carga.component';
import { PanelIncrementosComponent } from './pages/panel-incrementos/panel-incrementos.component';
import { LayoutComponent } from '../shared/layouts/layout/layout.component';

const routes: Routes = [
  {    
    path: '',
    component: LayoutComponent,
    children: [
      {path:'Panel-Carga', component: PanelCargaComponent},
      {path:'Panel-Incrementos', component: PanelIncrementosComponent},
      {path:'**',redirectTo:'Panel-Carga', pathMatch:'full'}

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRoutingModule { }
