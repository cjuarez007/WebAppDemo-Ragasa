import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Import de prime NG
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FloatLabelModule } from 'primeng/floatlabel';
import { PasswordModule } from 'primeng/password';
import { CardModule } from 'primeng/card';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { TableModule } from 'primeng/table';
import { MenubarModule } from 'primeng/menubar';
import { DropdownModule  } from 'primeng/dropdown';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,  
  ],
  exports: [
    ButtonModule,
    InputTextModule,
    FloatLabelModule,
    PasswordModule,
    CardModule,
    FileUploadModule,
    ToastModule,    
    TableModule,
    MenubarModule,
    DropdownModule,
  ]
})
export class PrimeNgModule { }
