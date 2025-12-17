import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { User } from '../../interfaces/user';
import { UserRes } from '../../interfaces/user-res';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private formBuilder = inject(FormBuilder);
  private svAuth = inject(AuthService);  
  private  router = inject(Router)

  public formLogin: FormGroup= this.formBuilder.group({
    userID: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  public saveUserLocalStorage(res : UserRes):void 
  {     
    localStorage.setItem('user', JSON.stringify(res));
  } 

  public loadLocalStorage():void{ 

    if(!localStorage.getItem('user')) return; 
    
    const temporal = JSON.parse(localStorage.getItem('user')!) 
    
    } 
  
  public login(){
    console.log(`userID ${this.formLogin.get("userID")?.value} pass ${this.formLogin.get("password")?.value}`)    
    
    let user: User = {
      NominaID: Number(this.formLogin.get("userID")?.value),
      Password: this.formLogin.get("password")?.value
    };

    this.svAuth.login(user).subscribe({
      next: (res: UserRes) => {
  
        this.saveUserLocalStorage(res)
  
        console.log('Login correcto', res);
  
        // Redirigir
        this.router.navigate(['/main']);
      },
      error: err => {
        console.error('Login incorrecto', err);
        if(localStorage.getItem('user')){
          localStorage.removeItem('user');
        }
        alert('Usuario o contraseña incorrectos');
      }
    })
    
  }
}
