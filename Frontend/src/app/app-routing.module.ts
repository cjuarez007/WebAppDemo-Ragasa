import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { sessionInactiveGuard } from './auth/guards/session-inactive.guard';
import { sessionActiveGuard } from './auth/guards/session-active.guard';

const routes: Routes = [

  {
    canMatch: [sessionInactiveGuard],
    path:'auth',
    loadChildren: () => import('../app/auth/auth.module').then(m => m.AuthModule)
  },
  {
    canMatch: [sessionActiveGuard],
    path:'main',
    loadChildren: () => import('../app/main/main.module').then(m => m.MainModule)
  },
  {
    path:'',
    redirectTo: 'auth',
    pathMatch: 'full'
  },
  {
    path:'**',
    redirectTo: 'auth',
    pathMatch: 'full'
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
