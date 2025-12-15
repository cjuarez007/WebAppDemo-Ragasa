import { Component, OnInit} from '@angular/core';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {
   items: MenuItem[] | undefined;

    ngOnInit() {
        this.items = [
            {
                label: 'Home',
                icon: 'pi pi-home',
                url: './main/Panel-Carga'
            },
            {
                label: 'Panel de Carga',
                icon: 'pi pi-file-arrow-up',
                url: './main/Panel-Carga'
            },          
            {
                label: 'Panel de Incrementos',
                icon: 'pi pi-file-edit',
                url: './main/Panel-Incrementos'
            },          
            {
                label: 'Panel de Gestion',
                icon: 'pi pi-chart-pie',
                url: './main/Panel-Incrementos'
            },          
            {
                label: 'Panel Resumen',
                icon: 'pi pi-chart-bar',
                url: './main/Panel-Incrementos'
            }
        ]
    }
}
