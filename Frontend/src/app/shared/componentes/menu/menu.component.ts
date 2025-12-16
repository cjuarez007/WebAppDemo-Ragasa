import { Component, OnInit, inject} from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {
    items: MenuItem[] | undefined;    
    private router = inject(Router);

    public logout():void{
        localStorage.removeItem('user');
        this.router.navigate(['/auth']);    
    }

    ngOnInit() {
        this.items = [
            // {
            //     label: 'Home',
            //     icon: 'pi pi-home',
            //     routerLink: ['/main/Panel-Carga']
            // },
            {
                label: 'Panel de Carga',
                icon: 'pi pi-file-arrow-up',
                routerLink: ['/main/Panel-Carga']
            },          
            {
                label: 'Panel de Incrementos',
                icon: 'pi pi-file-edit',
                routerLink: ['/main/Panel-Incrementos']
            },          
            // {
            //     label: 'Panel de Gestion',
            //     icon: 'pi pi-chart-pie',
            //     routerLink: ['/main/Panel-Incrementos']
            // },          
            // {
            //     label: 'Panel Resumen',
            //     icon: 'pi pi-chart-bar',
            //     routerLink: ['/main/Panel-Incrementos']
            // },
            {                
                label: 'finalizar sesion',
                icon: 'pi pi-sign-out',                            
                command: () => this.logout()
            },
        ]
    }
}
